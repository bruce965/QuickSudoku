// SPDX-FileCopyrightText: Copyright 2025 Fabio Iotti
// SPDX-License-Identifier: AGPL-3.0-only

using System.Runtime.CompilerServices;
using QuickSudoku.Solvers;
using QuickSudoku.Sudoku;
using QuickSudoku.Utilities;

namespace QuickSudoku.Generators;

public static class SudokuGenerator
{
    /// <summary>
    /// Groups of horizontally symmetric cell indices.
    /// </summary>
    static readonly SudokuCellIndex[][] HorizontalSymmetries
        = Enumerable.Range(0, 5)
            .SelectMany(y => Enumerable.Range(0, 9).Select(x => new SudokuCellIndex(x, y)))
            .Select(i => i switch {
                { X: 4 } => new[] { i },
                { X: _ } => [i, new(8 - i.X, i.Y)]
            })
            .ToArray();

    /// <summary>
    /// Groups of vertically symmetric cell indices.
    /// </summary>
    static readonly SudokuCellIndex[][] VerticalSymmetries
        = Enumerable.Range(0, 9)
            .SelectMany(y => Enumerable.Range(0, 5).Select(x => new SudokuCellIndex(x, y)))
            .Select(i => i switch {
                { Y: 4 } => new[] { i },
                { Y: _ } => [i, new(i.X, 8 - i.Y)]
            })
            .ToArray();

    /// <summary>
    /// Groups of horizontally and vertically symmetric cell indices.
    /// </summary>
    static readonly SudokuCellIndex[][] FullSymmetries
        = Enumerable.Range(0, 5)
            .SelectMany(y => Enumerable.Range(0, 5).Select(x => new SudokuCellIndex(x, y)))
            .Select(i => i switch {
                { X: 4, Y: 4 } => new[] { i },
                { X: 4, Y: _ } => [i, new(i.X, 8 - i.Y)],
                { X: _, Y: 4 } => [i, new(8 - i.X, i.Y)],
                { X: _, Y: _ } => [i, new(8 - i.X, i.Y), new(i.X, 8 - i.Y), new(8 - i.X, 8 - i.Y)]
            })
            .ToArray();

    /// <summary>
    /// Groups of all cell indices (each group contains a single cell index).
    /// </summary>
    static readonly SudokuCellIndex[][] IndividualCells
        = Enumerable.Range(0, 9)
            .SelectMany(y => Enumerable.Range(0, 9).Select(x => new[] { new SudokuCellIndex(x, y) }))
            .ToArray();

    /// <summary>
    /// Generate a new valid random puzzle.
    ///
    /// <para>This operation is slow (300ms to several seconds depending on the options).</para>
    /// </summary>
    /// <param name="options">Generation options.</param>
    /// <returns>Generated puzzle.</returns>
    /// <exception cref="TimeoutException">Failed to generate a puzzle with the specified options.</exception>
    public static SudokuPuzzle Generate(SudokuGenerationOptions? options = null)
    {
        options ??= SudokuGenerationOptions.Default;

        for (int i = 0; i < 100; i++)
        {
            if (TryGenerate(options, out SudokuPuzzle? puzzle))
                return puzzle;
        }

        throw new TimeoutException("Failed to generate a puzzle with the specified options.");
    }

    static bool TryGenerate(SudokuGenerationOptions options, out SudokuPuzzle puzzle)
    {
        options ??= SudokuGenerationOptions.Default;

        SudokuSolutionOptions solutionOptions = new()
        {
            EnsureSingleSolution = true,
            StopAtDifficulty = options.MaxDifficulty,
            ForbiddenStrategies = SudokuGenerationOptions.AllStrategies
                .Except(options.AllowedStrategies)
                .ToList(),
        };

        puzzle = GenerateSolved(options.Random);

        // try to remove symmetric cells
        RemoveSymmetricCells(
            puzzle,
            options.Random,
            options.Symmetry.HasFlag(SudokuSymmetry.Horizontal),
            options.Symmetry.HasFlag(SudokuSymmetry.Vertical),
            solutionOptions,
            out ISudokuSolutionLog? solutionLog);

        // if requested to, try to remove even more cells
        if (options.Symmetry.HasFlag(SudokuSymmetry.Loose))
        {
            RemoveSymmetricCells(
                puzzle,
                options.Random,
                false,
                false,
                solutionOptions,
                out ISudokuSolutionLog? finalSolutionLog);

            // if more cells have been removed, update the solution log
            if (finalSolutionLog is not null)
                solutionLog = finalSolutionLog;
        }

        if (solutionLog is null)
        {
            // failed to generate a puzzle (should be extremely rare, or maybe totally impossible)
            return false;
        }

        foreach (SudokuSolutionStrategy strategy in options.RequiredStrategies)
        {
            if (!solutionLog.AdoptedStrategies.ContainsKey(strategy))
            {
                // generated puzzle does not require all the requested strategies
                return false;
            }
        }

        // clean up candidates left behind
        foreach (SudokuCell cell in puzzle.Cells)
            if (cell.Value is null)
                cell.CandidateValues.Reset();

        return true;
    }

    /// <summary>
    /// Generate a new valid random solved puzzle.
    ///
    /// <para>This operation is slow (about 50ms to 100ms).</para>
    /// </summary>
    /// <param name="random">Source of randomness used to generate a puzzle.</param>
    /// <returns>Random solved puzzle.</returns>
    public static SudokuPuzzle GenerateSolved(Random? random = null)
    {
        SudokuPuzzle puzzle = new();
        SudokuSolver.SolveRecursive(puzzle, random ?? Random.Shared);

        return puzzle;
    }

    static int RemoveSymmetricCells(
        SudokuPuzzle puzzle,
        Random random,
        bool horizontalSymmetry,
        bool verticalSymmetry,
        SudokuSolutionOptions solutionOptions,
        out ISudokuSolutionLog? log)
    {
        ISudokuSolutionLog? finalSolutionLog = null;

        SudokuCellIndex[][] symmetricCells = GetSymmetricCells(horizontalSymmetry, verticalSymmetry);

        // buffer used to store the puzzle before trying to remove cells
        SudokuPuzzle removingBuffer = puzzle.Clone();

        // buffer used to store the puzzle before attempting to solve
        SudokuPuzzle solvingBuffer = puzzle.Clone();

        int removedTotal = 0;

        // randomly iterate all symmetric cell groups
        IEnumerable<SudokuCellIndex[]> candidates = symmetricCells.Shuffle(random);
        foreach (SudokuCellIndex[]? symmetryCellIndices in candidates)
        {
            // for each of the cells in each symmetric cells group...
            int removedCount = 0;
            foreach (SudokuCellIndex cellIndex in symmetryCellIndices)
            {
                SudokuCell cell = removingBuffer.Cells[cellIndex];

                int? value = cell.Value;
                if (value is null)
                    continue;

                // empty the cell's value (restore all candidates)
                cell.Value = null;
                removedCount++;

                //// reset candidates in houses the cell is in
                //foreach (SudokuHouse house in cell.Houses)
                //    foreach (SudokuCell houseCell in house.Cells)
                //        if (houseCell.Value is null)
                //            houseCell.CandidateValues.Reset();
            }

            if (removedCount is 0)
                continue;

            removingBuffer.CopyTo(solvingBuffer);

            // try to solve the puzzle
            finalSolutionLog = SudokuSolver.Solve(solvingBuffer, solutionOptions);

            // if removing last set of cells created a puzzle with multiple solutions,
            // discard this attempt and try again with next set of symmetric cells
            if (!finalSolutionLog.Solved || finalSolutionLog.HasMultipleSolutions is true)
            {
                puzzle.CopyTo(removingBuffer);
                continue;
            }

            // if puzzle is still valid, save and continue
            removingBuffer.CopyTo(puzzle);

            removedTotal += removedCount;
        }

        log = finalSolutionLog;
        return removedTotal;
    }

    static SudokuCellIndex[][] GetSymmetricCells(bool horizontal, bool vertical)
        => horizontal ? vertical ? FullSymmetries : HorizontalSymmetries : vertical ? VerticalSymmetries : IndividualCells;
}