// SPDX-FileCopyrightText: Copyright 2025 Fabio Iotti
// SPDX-License-Identifier: AGPL-3.0-only

namespace QuickSudoku.Abstractions;

/// <summary>
/// Generic puzzle board.
/// </summary>
public interface IPuzzle : IHouse, ICloneable
{
    /// <summary>
    /// Houses on this puzzle board.
    /// </summary>
    IEnumerable<IHouse> Houses { get; }

    /// <inheritdoc cref="ICloneable.Clone"/>
    new IPuzzle Clone() => (IPuzzle)((ICloneable)this).Clone();

    /// <summary>
    /// Copy the state of this puzzle board to another puzzle board of the same type.
    /// </summary>
    /// <param name="puzzle">Puzzle board this puzzle board should be copied over.</param>
    void CopyTo(IPuzzle puzzle);
}
