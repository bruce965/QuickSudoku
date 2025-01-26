// SPDX-FileCopyrightText: Copyright 2025 Fabio Iotti
// SPDX-License-Identifier: AGPL-3.0-only

using QuickSudoku.Solvers;
using QuickSudoku.Sudoku;
using System;
using Xunit;

namespace QuickSudoku.Tests;

public class SudokuHouseTests
{
    [Fact]
    public void TestIntersections()
    {
        SudokuPuzzle puzzle = new();

        Assert.Equal(((SudokuHouse)puzzle.Rows[0]).IntersectingHouses[0].Second, (SudokuHouse)puzzle.Columns[0]);
        Assert.Equal(((SudokuHouse)puzzle.Rows[0]).IntersectingHouses[1].Second, (SudokuHouse)puzzle.Columns[1]);
        Assert.Equal(((SudokuHouse)puzzle.Rows[0]).IntersectingHouses[2].Second, (SudokuHouse)puzzle.Columns[2]);
        Assert.Equal(((SudokuHouse)puzzle.Rows[0]).IntersectingHouses[3].Second, (SudokuHouse)puzzle.Columns[3]);
        Assert.Equal(((SudokuHouse)puzzle.Rows[0]).IntersectingHouses[4].Second, (SudokuHouse)puzzle.Columns[4]);
        Assert.Equal(((SudokuHouse)puzzle.Rows[0]).IntersectingHouses[5].Second, (SudokuHouse)puzzle.Columns[5]);
        Assert.Equal(((SudokuHouse)puzzle.Rows[0]).IntersectingHouses[6].Second, (SudokuHouse)puzzle.Columns[6]);
        Assert.Equal(((SudokuHouse)puzzle.Rows[0]).IntersectingHouses[7].Second, (SudokuHouse)puzzle.Columns[7]);
        Assert.Equal(((SudokuHouse)puzzle.Rows[0]).IntersectingHouses[8].Second, (SudokuHouse)puzzle.Columns[8]);
        Assert.Equal(((SudokuHouse)puzzle.Rows[0]).IntersectingHouses[9].Second, (SudokuHouse)puzzle.Squares[0]);
        Assert.Equal(((SudokuHouse)puzzle.Rows[0]).IntersectingHouses[10].Second, (SudokuHouse)puzzle.Squares[1]);
        Assert.Equal(((SudokuHouse)puzzle.Rows[0]).IntersectingHouses[11].Second, (SudokuHouse)puzzle.Squares[2]);

        Assert.Equal(((SudokuHouse)puzzle.Rows[1]).IntersectingHouses[0].Second, (SudokuHouse)puzzle.Columns[0]);
        Assert.Equal(((SudokuHouse)puzzle.Rows[1]).IntersectingHouses[1].Second, (SudokuHouse)puzzle.Columns[1]);
        Assert.Equal(((SudokuHouse)puzzle.Rows[1]).IntersectingHouses[9].Second, (SudokuHouse)puzzle.Squares[0]);

        Assert.Equal(((SudokuHouse)puzzle.Rows[3]).IntersectingHouses[0].Second, (SudokuHouse)puzzle.Columns[0]);
        Assert.Equal(((SudokuHouse)puzzle.Rows[3]).IntersectingHouses[9].Second, (SudokuHouse)puzzle.Squares[3]);
        Assert.Equal(((SudokuHouse)puzzle.Rows[3]).IntersectingHouses[10].Second, (SudokuHouse)puzzle.Squares[4]);
        Assert.Equal(((SudokuHouse)puzzle.Rows[3]).IntersectingHouses[11].Second, (SudokuHouse)puzzle.Squares[5]);

        Assert.Equal(((SudokuHouse)puzzle.Rows[8]).IntersectingHouses[0].Second, (SudokuHouse)puzzle.Columns[0]);
        Assert.Equal(((SudokuHouse)puzzle.Rows[8]).IntersectingHouses[8].Second, (SudokuHouse)puzzle.Columns[8]);
        Assert.Equal(((SudokuHouse)puzzle.Rows[8]).IntersectingHouses[9].Second, (SudokuHouse)puzzle.Squares[6]);
        Assert.Equal(((SudokuHouse)puzzle.Rows[8]).IntersectingHouses[10].Second, (SudokuHouse)puzzle.Squares[7]);
        Assert.Equal(((SudokuHouse)puzzle.Rows[8]).IntersectingHouses[11].Second, (SudokuHouse)puzzle.Squares[8]);

        Assert.Equal(((SudokuHouse)puzzle.Columns[0]).IntersectingHouses[0].Second, (SudokuHouse)puzzle.Rows[0]);
        Assert.Equal(((SudokuHouse)puzzle.Columns[0]).IntersectingHouses[1].Second, (SudokuHouse)puzzle.Rows[1]);
        Assert.Equal(((SudokuHouse)puzzle.Columns[0]).IntersectingHouses[2].Second, (SudokuHouse)puzzle.Rows[2]);
        Assert.Equal(((SudokuHouse)puzzle.Columns[0]).IntersectingHouses[3].Second, (SudokuHouse)puzzle.Rows[3]);
        Assert.Equal(((SudokuHouse)puzzle.Columns[0]).IntersectingHouses[4].Second, (SudokuHouse)puzzle.Rows[4]);
        Assert.Equal(((SudokuHouse)puzzle.Columns[0]).IntersectingHouses[5].Second, (SudokuHouse)puzzle.Rows[5]);
        Assert.Equal(((SudokuHouse)puzzle.Columns[0]).IntersectingHouses[6].Second, (SudokuHouse)puzzle.Rows[6]);
        Assert.Equal(((SudokuHouse)puzzle.Columns[0]).IntersectingHouses[7].Second, (SudokuHouse)puzzle.Rows[7]);
        Assert.Equal(((SudokuHouse)puzzle.Columns[0]).IntersectingHouses[8].Second, (SudokuHouse)puzzle.Rows[8]);
        Assert.Equal(((SudokuHouse)puzzle.Columns[0]).IntersectingHouses[9].Second, (SudokuHouse)puzzle.Squares[0]);
        Assert.Equal(((SudokuHouse)puzzle.Columns[0]).IntersectingHouses[10].Second, (SudokuHouse)puzzle.Squares[3]);
        Assert.Equal(((SudokuHouse)puzzle.Columns[0]).IntersectingHouses[11].Second, (SudokuHouse)puzzle.Squares[6]);

        Assert.Equal(((SudokuHouse)puzzle.Columns[1]).IntersectingHouses[0].Second, (SudokuHouse)puzzle.Rows[0]);
        Assert.Equal(((SudokuHouse)puzzle.Columns[1]).IntersectingHouses[1].Second, (SudokuHouse)puzzle.Rows[1]);
        Assert.Equal(((SudokuHouse)puzzle.Columns[1]).IntersectingHouses[9].Second, (SudokuHouse)puzzle.Squares[0]);

        Assert.Equal(((SudokuHouse)puzzle.Columns[3]).IntersectingHouses[0].Second, (SudokuHouse)puzzle.Rows[0]);
        Assert.Equal(((SudokuHouse)puzzle.Columns[3]).IntersectingHouses[9].Second, (SudokuHouse)puzzle.Squares[1]);
        Assert.Equal(((SudokuHouse)puzzle.Columns[3]).IntersectingHouses[10].Second, (SudokuHouse)puzzle.Squares[4]);
        Assert.Equal(((SudokuHouse)puzzle.Columns[3]).IntersectingHouses[11].Second, (SudokuHouse)puzzle.Squares[7]);

        Assert.Equal(((SudokuHouse)puzzle.Columns[8]).IntersectingHouses[0].Second, (SudokuHouse)puzzle.Rows[0]);
        Assert.Equal(((SudokuHouse)puzzle.Columns[8]).IntersectingHouses[8].Second, (SudokuHouse)puzzle.Rows[8]);
        Assert.Equal(((SudokuHouse)puzzle.Columns[8]).IntersectingHouses[9].Second, (SudokuHouse)puzzle.Squares[2]);
        Assert.Equal(((SudokuHouse)puzzle.Columns[8]).IntersectingHouses[10].Second, (SudokuHouse)puzzle.Squares[5]);
        Assert.Equal(((SudokuHouse)puzzle.Columns[8]).IntersectingHouses[11].Second, (SudokuHouse)puzzle.Squares[8]);

        Assert.Equal(((SudokuHouse)puzzle.Squares[0]).IntersectingHouses[0].Second, (SudokuHouse)puzzle.Rows[0]);
        Assert.Equal(((SudokuHouse)puzzle.Squares[0]).IntersectingHouses[1].Second, (SudokuHouse)puzzle.Rows[1]);
        Assert.Equal(((SudokuHouse)puzzle.Squares[0]).IntersectingHouses[2].Second, (SudokuHouse)puzzle.Rows[2]);
        Assert.Equal(((SudokuHouse)puzzle.Squares[0]).IntersectingHouses[3].Second, (SudokuHouse)puzzle.Columns[0]);
        Assert.Equal(((SudokuHouse)puzzle.Squares[0]).IntersectingHouses[4].Second, (SudokuHouse)puzzle.Columns[1]);
        Assert.Equal(((SudokuHouse)puzzle.Squares[0]).IntersectingHouses[5].Second, (SudokuHouse)puzzle.Columns[2]);

        Assert.Equal(((SudokuHouse)puzzle.Squares[1]).IntersectingHouses[0].Second, (SudokuHouse)puzzle.Rows[0]);
        Assert.Equal(((SudokuHouse)puzzle.Squares[1]).IntersectingHouses[1].Second, (SudokuHouse)puzzle.Rows[1]);
        Assert.Equal(((SudokuHouse)puzzle.Squares[1]).IntersectingHouses[2].Second, (SudokuHouse)puzzle.Rows[2]);
        Assert.Equal(((SudokuHouse)puzzle.Squares[1]).IntersectingHouses[3].Second, (SudokuHouse)puzzle.Columns[3]);
        Assert.Equal(((SudokuHouse)puzzle.Squares[1]).IntersectingHouses[4].Second, (SudokuHouse)puzzle.Columns[4]);
        Assert.Equal(((SudokuHouse)puzzle.Squares[1]).IntersectingHouses[5].Second, (SudokuHouse)puzzle.Columns[5]);

        Assert.Equal(((SudokuHouse)puzzle.Squares[2]).IntersectingHouses[3].Second, (SudokuHouse)puzzle.Columns[6]);
        Assert.Equal(((SudokuHouse)puzzle.Squares[2]).IntersectingHouses[4].Second, (SudokuHouse)puzzle.Columns[7]);
        Assert.Equal(((SudokuHouse)puzzle.Squares[2]).IntersectingHouses[5].Second, (SudokuHouse)puzzle.Columns[8]);

        Assert.Equal(((SudokuHouse)puzzle.Squares[3]).IntersectingHouses[0].Second, (SudokuHouse)puzzle.Rows[3]);
        Assert.Equal(((SudokuHouse)puzzle.Squares[3]).IntersectingHouses[1].Second, (SudokuHouse)puzzle.Rows[4]);
        Assert.Equal(((SudokuHouse)puzzle.Squares[3]).IntersectingHouses[2].Second, (SudokuHouse)puzzle.Rows[5]);
        Assert.Equal(((SudokuHouse)puzzle.Squares[3]).IntersectingHouses[3].Second, (SudokuHouse)puzzle.Columns[0]);
        Assert.Equal(((SudokuHouse)puzzle.Squares[3]).IntersectingHouses[4].Second, (SudokuHouse)puzzle.Columns[1]);
        Assert.Equal(((SudokuHouse)puzzle.Squares[3]).IntersectingHouses[5].Second, (SudokuHouse)puzzle.Columns[2]);

        Assert.Equal(((SudokuHouse)puzzle.Squares[8]).IntersectingHouses[0].Second, (SudokuHouse)puzzle.Rows[6]);
        Assert.Equal(((SudokuHouse)puzzle.Squares[8]).IntersectingHouses[1].Second, (SudokuHouse)puzzle.Rows[7]);
        Assert.Equal(((SudokuHouse)puzzle.Squares[8]).IntersectingHouses[2].Second, (SudokuHouse)puzzle.Rows[8]);
        Assert.Equal(((SudokuHouse)puzzle.Squares[8]).IntersectingHouses[3].Second, (SudokuHouse)puzzle.Columns[6]);
        Assert.Equal(((SudokuHouse)puzzle.Squares[8]).IntersectingHouses[4].Second, (SudokuHouse)puzzle.Columns[7]);
        Assert.Equal(((SudokuHouse)puzzle.Squares[8]).IntersectingHouses[5].Second, (SudokuHouse)puzzle.Columns[8]);
    }
}
