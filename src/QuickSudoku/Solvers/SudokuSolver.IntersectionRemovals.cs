// SPDX-FileCopyrightText: Copyright 2025 Fabio Iotti
// SPDX-License-Identifier: AGPL-3.0-only

using QuickSudoku.Sudoku;

namespace QuickSudoku.Solvers;

partial class SudokuSolver
{
    /// <summary>
    /// Solve throgh intersection removal in a puzzle.
    ///
    /// This method should be called only after calling another method to remove invalid candidates.
    /// </summary>
    /// <param name="puzzle">Puzzle</param>
    /// <param name="maxIntersectionsCount">How many intersections to be removed before stopping.</param>
    /// <returns>Number of intersections and candidates removed.</returns>
    public static (int Intersections, int Candidates) SolveIntersectionRemovals(SudokuPuzzle puzzle, int maxIntersectionsCount = -1)
    {
        // TODO
        return (0, 0);
    }
}
