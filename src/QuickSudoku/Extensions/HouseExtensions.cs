// SPDX-FileCopyrightText: Copyright 2025 Fabio Iotti
// SPDX-License-Identifier: AGPL-3.0-only

using QuickSudoku.Abstractions;

namespace QuickSudoku.Extensions;

public static class HouseExtensions
{
    /// <summary>
    /// Check whether all cells in a house are solved.
    /// </summary>
    /// <param name="house"></param>
    /// <returns></returns>
    public static bool IsSolved(this IHouse house)
        => house.Cells.All(c => c.IsSolved());

    /// <summary>
    /// Check whether a solution may exist for all cells in a house.
    /// </summary>
    /// <param name="house"></param>
    /// <returns></returns>
    public static bool HasSolution(this IHouse house)
        => house.Cells.All(c => c.HasSolution());

    /// <summary>
    /// Check whether a house contains a certain value in a solved cell.
    /// </summary>
    /// <param name="house"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool Contains(this IHouse house, object value)
        => house.Cells.Any(cell => cell.Contains(value));
}
