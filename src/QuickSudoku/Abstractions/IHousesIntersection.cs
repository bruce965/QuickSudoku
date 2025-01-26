// SPDX-FileCopyrightText: Copyright 2025 Fabio Iotti
// SPDX-License-Identifier: AGPL-3.0-only

namespace QuickSudoku.Abstractions;

/// <summary>
/// Intersection between two houses of the puzzle board.
/// </summary>
public interface IHousesIntersection
{
    /// <summary>
    /// First house.
    /// </summary>
    IHouse First { get; }

    /// <summary>
    /// Second house.
    /// </summary>
    IHouse Second { get; }

    /// <summary>
    /// Cells in common between the two houses.
    /// </summary>
    IEnumerable<ICell> Cells => First.Cells.Intersect(Second.Cells);
}
