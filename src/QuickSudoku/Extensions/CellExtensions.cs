// SPDX-FileCopyrightText: Copyright 2025 Fabio Iotti
// SPDX-License-Identifier: AGPL-3.0-only

using QuickSudoku.Abstractions;

namespace QuickSudoku.Extensions;

public static class CellExtensions
{
    /// <summary>
    /// Check whether the value in a cell is known.
    /// </summary>
    /// <param name="cell"></param>
    /// <returns></returns>
    public static bool IsSolved(this ICell cell)
        => cell.CandidateValues.Take(2).Count() == 1;

    /// <summary>
    /// Check whether a solution may exist for a cell.
    /// </summary>
    /// <param name="cell"></param>
    /// <returns></returns>
    public static bool HasSolution(this ICell cell)
        => cell.CandidateValues.Any();

    /// <summary>
    /// Check whether a cell may contain a certain value.
    /// </summary>
    /// <param name="cell"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool MayContain(this ICell cell, object value)
        => cell.CandidateValues.Contains(value);

    /// <summary>
    /// Check whether a cell is solved with a certain value.
    /// </summary>
    /// <param name="cell"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool Contains(this ICell cell, object value)
        => cell.IsSolved() && cell.LegalValues.All(v => v == value);
}
