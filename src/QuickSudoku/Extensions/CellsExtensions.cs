// SPDX-FileCopyrightText: Copyright 2025 Fabio Iotti
// SPDX-License-Identifier: AGPL-3.0-only

using QuickSudoku.Abstractions;

namespace QuickSudoku.Extensions;

public static class CellsExtensions
{
    /// <summary>
    /// Solved cells.
    /// </summary>
    /// <param name="cells"></param>
    /// <returns></returns>
    public static IEnumerable<ICell> Solved(this IEnumerable<ICell> cells)
        => cells.Where(c => c.IsSolved());

    /// <summary>
    /// Unsolved cells.
    /// </summary>
    /// <param name="cells"></param>
    /// <returns></returns>
    public static IEnumerable<ICell> Unsolved(this IEnumerable<ICell> cells)
        => cells.Where(c => !c.IsSolved());
}
