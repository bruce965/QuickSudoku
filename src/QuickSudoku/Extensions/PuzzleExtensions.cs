// SPDX-FileCopyrightText: Copyright 2025 Fabio Iotti
// SPDX-License-Identifier: AGPL-3.0-only

using QuickSudoku.Abstractions;

namespace QuickSudoku.Extensions;

public static class PuzzleExtensions
{
    /// <summary>
    /// Check whether all cells in a puzzle board are solved.
    /// </summary>
    /// <param name="puzzle"></param>
    /// <returns></returns>
    public static bool IsSolved(this IPuzzle puzzle)
        => puzzle.Cells.All(c => c.IsSolved());

    /// <summary>
    /// Check whether a solution may exist for a puzzle.
    /// </summary>
    /// <param name="puzzle"></param>
    /// <returns></returns>
    public static bool HasSolution(this IPuzzle puzzle)
        => puzzle.Cells.All(c => c.HasSolution());
}
