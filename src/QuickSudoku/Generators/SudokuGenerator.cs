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
            .Select(i => i.X == 4
                ? new[] { i }
                : new[] { i, new(8 - i.X, i.Y) }
            )
            .ToArray();

    /// <summary>
    /// Groups of vertically symmetric cell indices.
    /// </summary>
    static readonly SudokuCellIndex[][] VerticalSymmetries
        = Enumerable.Range(0, 9)
            .SelectMany(y => Enumerable.Range(0, 5).Select(x => new SudokuCellIndex(x, y)))
            .Select(i => i.Y == 4
                ? new[] { i }
                : new[] { i, new(i.X, 8 - i.Y) }
            )
            .ToArray();

    /// <summary>
    /// Groups of horizontally and vertically symmetric cell indices.
    /// </summary>
    static readonly SudokuCellIndex[][] FullSymmetries
        = Enumerable.Range(0, 5)
            .SelectMany(y => Enumerable.Range(0, 5).Select(x => new SudokuCellIndex(x, y)))
            .Select(i => i.X == 4
                ? i.Y == 4
                    ? new[] { i }
                    : new[] { i, new(i.X, 8 - i.Y) }
                : i.Y == 4
                    ? new[] { i, new(8 - i.X, i.Y) }
                    : new[] { i, new(8 - i.X, i.Y), new(i.X, 8 - i.Y), new(8 - i.X, 8 - i.Y) }
            )
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

        for (var i = 0; i < 100; i++)
        {
            if (TryGenerate(options, out var puzzle))
                return puzzle;
        }

        throw new TimeoutException("Failed to generate a puzzle with the specified options.");
    }

    static bool TryGenerate(SudokuGenerationOptions options, out SudokuPuzzle puzzle)
    {
        options ??= SudokuGenerationOptions.Default;

        var solutionOptions = new SudokuSolutionOptions
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
            out var solutionLog);

        // if requested to, try to remove even more cells
        if (options.Symmetry.HasFlag(SudokuSymmetry.Loose))
        {
            RemoveSymmetricCells(
                puzzle,
                options.Random,
                false,
                false,
                solutionOptions,
                out var finalSolutionLog);

            // if more cells have been removed, update the solution log
            if (finalSolutionLog != null)
                solutionLog = finalSolutionLog;
        }

        if (solutionLog == null)
        {
            // failed to generate a puzzle (should be extremely rare, or maybe totally impossible)
            return false;
        }

        foreach (var strategy in options.RequiredStrategies)
        {
            if (!solutionLog.AdoptedStrategies.ContainsKey(strategy))
            {
                // generated puzzle does not require all the requested strategies
                return false;
            }
        }

        // clean up candidates left behind
        foreach (var cell in puzzle.Cells)
            if (cell.Value == null)
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
        var puzzle = new SudokuPuzzle();
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

        var symmetricCells = GetSymmetricCells(horizontalSymmetry, verticalSymmetry);

        // buffer uset to store the puzzle before trying to remove cells
        var removingBuffer = puzzle.Clone();

        // buffer used to store the puzzle before attempting to solve
        var solvingBuffer = puzzle.Clone();

        var removedTotal = 0;

        // randomly iterate all symmetric cell groups
        var candidates = symmetricCells.Shuffle(random);
        foreach (var symmetryCellIndices in candidates)
        {
            // for each of the cells in each symmetric cells group...
            var removedCount = 0;
            foreach (var cellIndex in symmetryCellIndices)
            {
                var cell = removingBuffer.Cells[cellIndex];

                var value = cell.Value;
                if (value == null)
                    continue;

                // empty the cell's value (restore all candidates)
                cell.Value = null;
                removedCount++;

                //// reset candidates in houses the cell is in
                //foreach (var house in cell.Houses)
                //    foreach (var houseCell in house.Cells)
                //        if (houseCell.Value == null)
                //            houseCell.CandidateValues.Reset();
            }

            if (removedCount == 0)
                continue;

            removingBuffer.CopyTo(solvingBuffer);

            // try to solve the puzzle
            finalSolutionLog = SudokuSolver.Solve(solvingBuffer, solutionOptions);

            // if removing last set of cells created a puzzle with multiple solutions,
            // discard this attempt and try again with next set of symmetric cells
            if (!finalSolutionLog.Solved || finalSolutionLog.HasMultipleSolutions == true)
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