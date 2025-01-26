// SPDX-FileCopyrightText: Copyright 2025 Fabio Iotti
// SPDX-License-Identifier: AGPL-3.0-only

namespace QuickSudoku.Abstractions;

/// <summary>
/// Cell in the puzzle.
/// </summary>
public interface ICell : IEquatable<ICell>
{
    /// <summary>
    /// Puzzle board this cell is part of.
    /// </summary>
    IPuzzle Puzzle { get; }

    /// <summary>
    /// Houses this cell is part of.
    /// </summary>
    IEnumerable<IHouse> Houses => Puzzle.Houses.Where(r => r.Cells.Contains(this));

    /// <summary>
    /// Values legal on this cell.
    /// </summary>
    IEnumerable<object> LegalValues => Puzzle.LegalValues;

    /// <summary>
    /// Candidate values in this cell.
    /// </summary>
    IValuesCollection<object> CandidateValues { get; }
}
