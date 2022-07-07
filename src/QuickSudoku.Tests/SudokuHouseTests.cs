using QuickSudoku.Solvers;
using QuickSudoku.Sudoku;
using System;
using Xunit;

namespace QuickSudoku.Tests;

public class SudokuRegionTests
{
    [Fact]
    public void TestIntersections()
    {
        var puzzle = new SudokuPuzzle();

        Assert.Equal(((SudokuRegion)puzzle.Rows[0]).IntersectingRegions[0].Second, (SudokuRegion)puzzle.Columns[0]);
        Assert.Equal(((SudokuRegion)puzzle.Rows[0]).IntersectingRegions[1].Second, (SudokuRegion)puzzle.Columns[1]);
        Assert.Equal(((SudokuRegion)puzzle.Rows[0]).IntersectingRegions[2].Second, (SudokuRegion)puzzle.Columns[2]);
        Assert.Equal(((SudokuRegion)puzzle.Rows[0]).IntersectingRegions[3].Second, (SudokuRegion)puzzle.Columns[3]);
        Assert.Equal(((SudokuRegion)puzzle.Rows[0]).IntersectingRegions[4].Second, (SudokuRegion)puzzle.Columns[4]);
        Assert.Equal(((SudokuRegion)puzzle.Rows[0]).IntersectingRegions[5].Second, (SudokuRegion)puzzle.Columns[5]);
        Assert.Equal(((SudokuRegion)puzzle.Rows[0]).IntersectingRegions[6].Second, (SudokuRegion)puzzle.Columns[6]);
        Assert.Equal(((SudokuRegion)puzzle.Rows[0]).IntersectingRegions[7].Second, (SudokuRegion)puzzle.Columns[7]);
        Assert.Equal(((SudokuRegion)puzzle.Rows[0]).IntersectingRegions[8].Second, (SudokuRegion)puzzle.Columns[8]);
        Assert.Equal(((SudokuRegion)puzzle.Rows[0]).IntersectingRegions[9].Second, (SudokuRegion)puzzle.Squares[0]);
        Assert.Equal(((SudokuRegion)puzzle.Rows[0]).IntersectingRegions[10].Second, (SudokuRegion)puzzle.Squares[1]);
        Assert.Equal(((SudokuRegion)puzzle.Rows[0]).IntersectingRegions[11].Second, (SudokuRegion)puzzle.Squares[2]);

        Assert.Equal(((SudokuRegion)puzzle.Rows[1]).IntersectingRegions[0].Second, (SudokuRegion)puzzle.Columns[0]);
        Assert.Equal(((SudokuRegion)puzzle.Rows[1]).IntersectingRegions[1].Second, (SudokuRegion)puzzle.Columns[1]);
        Assert.Equal(((SudokuRegion)puzzle.Rows[1]).IntersectingRegions[9].Second, (SudokuRegion)puzzle.Squares[0]);

        Assert.Equal(((SudokuRegion)puzzle.Rows[3]).IntersectingRegions[0].Second, (SudokuRegion)puzzle.Columns[0]);
        Assert.Equal(((SudokuRegion)puzzle.Rows[3]).IntersectingRegions[9].Second, (SudokuRegion)puzzle.Squares[3]);
        Assert.Equal(((SudokuRegion)puzzle.Rows[3]).IntersectingRegions[10].Second, (SudokuRegion)puzzle.Squares[4]);
        Assert.Equal(((SudokuRegion)puzzle.Rows[3]).IntersectingRegions[11].Second, (SudokuRegion)puzzle.Squares[5]);

        Assert.Equal(((SudokuRegion)puzzle.Rows[8]).IntersectingRegions[0].Second, (SudokuRegion)puzzle.Columns[0]);
        Assert.Equal(((SudokuRegion)puzzle.Rows[8]).IntersectingRegions[8].Second, (SudokuRegion)puzzle.Columns[8]);
        Assert.Equal(((SudokuRegion)puzzle.Rows[8]).IntersectingRegions[9].Second, (SudokuRegion)puzzle.Squares[6]);
        Assert.Equal(((SudokuRegion)puzzle.Rows[8]).IntersectingRegions[10].Second, (SudokuRegion)puzzle.Squares[7]);
        Assert.Equal(((SudokuRegion)puzzle.Rows[8]).IntersectingRegions[11].Second, (SudokuRegion)puzzle.Squares[8]);

        Assert.Equal(((SudokuRegion)puzzle.Columns[0]).IntersectingRegions[0].Second, (SudokuRegion)puzzle.Rows[0]);
        Assert.Equal(((SudokuRegion)puzzle.Columns[0]).IntersectingRegions[1].Second, (SudokuRegion)puzzle.Rows[1]);
        Assert.Equal(((SudokuRegion)puzzle.Columns[0]).IntersectingRegions[2].Second, (SudokuRegion)puzzle.Rows[2]);
        Assert.Equal(((SudokuRegion)puzzle.Columns[0]).IntersectingRegions[3].Second, (SudokuRegion)puzzle.Rows[3]);
        Assert.Equal(((SudokuRegion)puzzle.Columns[0]).IntersectingRegions[4].Second, (SudokuRegion)puzzle.Rows[4]);
        Assert.Equal(((SudokuRegion)puzzle.Columns[0]).IntersectingRegions[5].Second, (SudokuRegion)puzzle.Rows[5]);
        Assert.Equal(((SudokuRegion)puzzle.Columns[0]).IntersectingRegions[6].Second, (SudokuRegion)puzzle.Rows[6]);
        Assert.Equal(((SudokuRegion)puzzle.Columns[0]).IntersectingRegions[7].Second, (SudokuRegion)puzzle.Rows[7]);
        Assert.Equal(((SudokuRegion)puzzle.Columns[0]).IntersectingRegions[8].Second, (SudokuRegion)puzzle.Rows[8]);
        Assert.Equal(((SudokuRegion)puzzle.Columns[0]).IntersectingRegions[9].Second, (SudokuRegion)puzzle.Squares[0]);
        Assert.Equal(((SudokuRegion)puzzle.Columns[0]).IntersectingRegions[10].Second, (SudokuRegion)puzzle.Squares[3]);
        Assert.Equal(((SudokuRegion)puzzle.Columns[0]).IntersectingRegions[11].Second, (SudokuRegion)puzzle.Squares[6]);

        Assert.Equal(((SudokuRegion)puzzle.Columns[1]).IntersectingRegions[0].Second, (SudokuRegion)puzzle.Rows[0]);
        Assert.Equal(((SudokuRegion)puzzle.Columns[1]).IntersectingRegions[1].Second, (SudokuRegion)puzzle.Rows[1]);
        Assert.Equal(((SudokuRegion)puzzle.Columns[1]).IntersectingRegions[9].Second, (SudokuRegion)puzzle.Squares[0]);

        Assert.Equal(((SudokuRegion)puzzle.Columns[3]).IntersectingRegions[0].Second, (SudokuRegion)puzzle.Rows[0]);
        Assert.Equal(((SudokuRegion)puzzle.Columns[3]).IntersectingRegions[9].Second, (SudokuRegion)puzzle.Squares[1]);
        Assert.Equal(((SudokuRegion)puzzle.Columns[3]).IntersectingRegions[10].Second, (SudokuRegion)puzzle.Squares[4]);
        Assert.Equal(((SudokuRegion)puzzle.Columns[3]).IntersectingRegions[11].Second, (SudokuRegion)puzzle.Squares[7]);

        Assert.Equal(((SudokuRegion)puzzle.Columns[8]).IntersectingRegions[0].Second, (SudokuRegion)puzzle.Rows[0]);
        Assert.Equal(((SudokuRegion)puzzle.Columns[8]).IntersectingRegions[8].Second, (SudokuRegion)puzzle.Rows[8]);
        Assert.Equal(((SudokuRegion)puzzle.Columns[8]).IntersectingRegions[9].Second, (SudokuRegion)puzzle.Squares[2]);
        Assert.Equal(((SudokuRegion)puzzle.Columns[8]).IntersectingRegions[10].Second, (SudokuRegion)puzzle.Squares[5]);
        Assert.Equal(((SudokuRegion)puzzle.Columns[8]).IntersectingRegions[11].Second, (SudokuRegion)puzzle.Squares[8]);

        Assert.Equal(((SudokuRegion)puzzle.Squares[0]).IntersectingRegions[0].Second, (SudokuRegion)puzzle.Rows[0]);
        Assert.Equal(((SudokuRegion)puzzle.Squares[0]).IntersectingRegions[1].Second, (SudokuRegion)puzzle.Rows[1]);
        Assert.Equal(((SudokuRegion)puzzle.Squares[0]).IntersectingRegions[2].Second, (SudokuRegion)puzzle.Rows[2]);
        Assert.Equal(((SudokuRegion)puzzle.Squares[0]).IntersectingRegions[3].Second, (SudokuRegion)puzzle.Columns[0]);
        Assert.Equal(((SudokuRegion)puzzle.Squares[0]).IntersectingRegions[4].Second, (SudokuRegion)puzzle.Columns[1]);
        Assert.Equal(((SudokuRegion)puzzle.Squares[0]).IntersectingRegions[5].Second, (SudokuRegion)puzzle.Columns[2]);

        Assert.Equal(((SudokuRegion)puzzle.Squares[1]).IntersectingRegions[0].Second, (SudokuRegion)puzzle.Rows[0]);
        Assert.Equal(((SudokuRegion)puzzle.Squares[1]).IntersectingRegions[1].Second, (SudokuRegion)puzzle.Rows[1]);
        Assert.Equal(((SudokuRegion)puzzle.Squares[1]).IntersectingRegions[2].Second, (SudokuRegion)puzzle.Rows[2]);
        Assert.Equal(((SudokuRegion)puzzle.Squares[1]).IntersectingRegions[3].Second, (SudokuRegion)puzzle.Columns[3]);
        Assert.Equal(((SudokuRegion)puzzle.Squares[1]).IntersectingRegions[4].Second, (SudokuRegion)puzzle.Columns[4]);
        Assert.Equal(((SudokuRegion)puzzle.Squares[1]).IntersectingRegions[5].Second, (SudokuRegion)puzzle.Columns[5]);

        Assert.Equal(((SudokuRegion)puzzle.Squares[2]).IntersectingRegions[3].Second, (SudokuRegion)puzzle.Columns[6]);
        Assert.Equal(((SudokuRegion)puzzle.Squares[2]).IntersectingRegions[4].Second, (SudokuRegion)puzzle.Columns[7]);
        Assert.Equal(((SudokuRegion)puzzle.Squares[2]).IntersectingRegions[5].Second, (SudokuRegion)puzzle.Columns[8]);

        Assert.Equal(((SudokuRegion)puzzle.Squares[3]).IntersectingRegions[0].Second, (SudokuRegion)puzzle.Rows[3]);
        Assert.Equal(((SudokuRegion)puzzle.Squares[3]).IntersectingRegions[1].Second, (SudokuRegion)puzzle.Rows[4]);
        Assert.Equal(((SudokuRegion)puzzle.Squares[3]).IntersectingRegions[2].Second, (SudokuRegion)puzzle.Rows[5]);
        Assert.Equal(((SudokuRegion)puzzle.Squares[3]).IntersectingRegions[3].Second, (SudokuRegion)puzzle.Columns[0]);
        Assert.Equal(((SudokuRegion)puzzle.Squares[3]).IntersectingRegions[4].Second, (SudokuRegion)puzzle.Columns[1]);
        Assert.Equal(((SudokuRegion)puzzle.Squares[3]).IntersectingRegions[5].Second, (SudokuRegion)puzzle.Columns[2]);

        Assert.Equal(((SudokuRegion)puzzle.Squares[8]).IntersectingRegions[0].Second, (SudokuRegion)puzzle.Rows[6]);
        Assert.Equal(((SudokuRegion)puzzle.Squares[8]).IntersectingRegions[1].Second, (SudokuRegion)puzzle.Rows[7]);
        Assert.Equal(((SudokuRegion)puzzle.Squares[8]).IntersectingRegions[2].Second, (SudokuRegion)puzzle.Rows[8]);
        Assert.Equal(((SudokuRegion)puzzle.Squares[8]).IntersectingRegions[3].Second, (SudokuRegion)puzzle.Columns[6]);
        Assert.Equal(((SudokuRegion)puzzle.Squares[8]).IntersectingRegions[4].Second, (SudokuRegion)puzzle.Columns[7]);
        Assert.Equal(((SudokuRegion)puzzle.Squares[8]).IntersectingRegions[5].Second, (SudokuRegion)puzzle.Columns[8]);
    }
}
