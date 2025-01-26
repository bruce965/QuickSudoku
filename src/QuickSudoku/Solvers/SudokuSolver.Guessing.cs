// SPDX-FileCopyrightText: Copyright 2025 Fabio Iotti
// SPDX-License-Identifier: AGPL-3.0-only

using QuickSudoku.Sudoku;

namespace QuickSudoku.Solvers;

partial class SudokuSolver
{
    /// <summary>
    /// Solve a cell by guessing, taking a value from the solution.
    ///
    /// <para>Unlike for a real human, this algorithm cheats and always
    /// guesses with the correct value taken from the solution.</para>
    /// </summary>
    /// <param name="puzzle">Puzzle.</param>
    /// <param name="count">How many cells to guess before stopping.</param>
    public static void SolveGuessing(SudokuPuzzle puzzle, SudokuPuzzle solution, int count = -1)
    {
        // order cells with by smallest amount of candidates (like a human would do), then by index
        var cells = puzzle.Cells
            .Select(c => (Cell: c, CandidatesCount: c.CandidateValues.Count))
            .Where(c => c.CandidatesCount > 1)
            .OrderBy(c => c.CandidatesCount).ThenBy(c => c.Cell.Index.Index)
            .Select(c => c.Cell);

        if (count != -1)
            cells = cells.Take(count);

        foreach (var cell in cells)
        {
            cell.Value = solution.Cells[cell.Index].Value;
        }
    }
}
