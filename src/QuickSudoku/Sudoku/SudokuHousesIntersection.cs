// SPDX-FileCopyrightText: Copyright 2025 Fabio Iotti
// SPDX-License-Identifier: AGPL-3.0-only

using QuickSudoku.Abstractions;

namespace QuickSudoku.Sudoku;

public struct SudokuHousesIntersection : IHousesIntersection
{
    /// <inheritdoc cref="IHousesIntersection.First"/>
    public SudokuHouse First { get; }
    
    /// <inheritdoc cref="IHousesIntersection.Second"/>
    public SudokuHouse Second { get; }

    ///// <inheritdoc cref="IHousesIntersection.Cells"/>
    //public SudokuHouse Cells { get; }  // TODO

    internal SudokuHousesIntersection(SudokuHouse first, SudokuHouse second)
    {
        First = first;
        Second = second;
    }

    #region IHousesIntersection

    IHouse IHousesIntersection.First => First;

    IHouse IHousesIntersection.Second => Second;

    //IEnumerable<ICell> IHousesIntersection.Cells => Cells;  // TODO

    #endregion
}
