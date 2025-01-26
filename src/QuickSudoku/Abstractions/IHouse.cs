// SPDX-FileCopyrightText: Copyright 2025 Fabio Iotti
// SPDX-License-Identifier: AGPL-3.0-only

namespace QuickSudoku.Abstractions;

/// <summary>
/// Group of cells on the puzzle board.
/// 
/// Each value can only appear once in each house.
/// </summary>
public interface IHouse : IEquatable<IHouse>
{
    record Intersection(IHouse First, IHouse Second) : IHousesIntersection;

    /// <summary>
    /// Puzzle this house is part of.
    /// </summary>
    IPuzzle Puzzle { get; }

    /// <summary>
    /// Values legal in this house.
    /// </summary>
    IEnumerable<object> LegalValues => Puzzle.LegalValues;

    /// <summary>
    /// Cells in this house.
    /// </summary>
    IEnumerable<ICell> Cells { get; }

    /// <summary>
    /// Houses in the puzzle intersecting with this house.
    /// </summary>
    IEnumerable<IHousesIntersection> IntersectingHouses
        => Puzzle.Houses
            .Select(r => new Intersection(this, r))
            .Where(i => i.First != i.Second && ((IHousesIntersection)i).Cells.Any());
}
