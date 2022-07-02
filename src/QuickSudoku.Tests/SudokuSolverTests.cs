using QuickSudoku.Solvers;
using QuickSudoku.Sudoku;
using System;
using Xunit;

namespace QuickSudoku.Tests;

public class SudokuSolverTests
{
    [Fact]
    public void TestNakedSingles()
    {
        var puzzle = SudokuPuzzle.FromScheme(@"
           >... 123 ...
            ..8 ... ...
            .7. ... ...

            4.. ... ...
            5.. ... ...
            6.. ... ...

            ... ... ...
            ... ... ...
            ... ... ...
        ");

        var allocBefore = GC.GetTotalAllocatedBytes(true);

        var nakedSinglesCount = SudokuSolver.SolveNakedSingles(puzzle);

        var allocAfter = GC.GetTotalAllocatedBytes(true);

        Assert.Equal(1, nakedSinglesCount);

        Assert.Equal(9, puzzle[0, 0].Value);

        Assert.True(allocAfter == allocBefore, "No memory should have been allocated.");
    }

    [Fact]
    public void TestHiddenSingle()
    {
        var puzzle = SudokuPuzzle.FromScheme(@"
           >... ... ...
            ... 1.. ...
            ... ... 1..

            ... ... ...
            .1. ... ...
            ... ... ...

            ..1 ... ...
            ... ... ...
            ... ... ...
        ");

        // remove candidates through other methods before attempting to solve hidden singles
        SudokuSolver.SolveNakedSingles(puzzle);

        var allocBefore = GC.GetTotalAllocatedBytes(true);

        var nakedSingleFound = SudokuSolver.SolveHiddenSingle(puzzle);

        var allocAfter = GC.GetTotalAllocatedBytes(true);

        Assert.True(nakedSingleFound);

        Assert.Equal(1, puzzle[0, 0].Value);

        Assert.True(allocAfter == allocBefore, "No memory should have been allocated.");
    }

    [Fact]
    public void TestNakedPair()
    {
        var puzzle = SudokuPuzzle.FromScheme(@"
             v
           >... 123 456
           >... 456 123
            ... ... .89

            7.. ... ...
            ... ... ...
            ... ... ...

            ... ... ...
            ... ... ...
            ... ... ...
        ");

        // remove candidates through other methods before attempting to solve naked pairs
        SudokuSolver.SolveNakedSingles(puzzle);

        Assert.Equal(SudokuDigits.Digit8 | SudokuDigits.Digit9, puzzle[0, 0].CandidateValues.Digits);
        Assert.Equal(SudokuDigits.Digit8 | SudokuDigits.Digit9, puzzle[0, 1].CandidateValues.Digits);

        Assert.True(puzzle[1, 0].CandidateValues.Contains(8));
        Assert.True(puzzle[1, 0].CandidateValues.Contains(9));

        var allocBefore = GC.GetTotalAllocatedBytes(true);

        var nakedPairFound = SudokuSolver.SolveNakedPair(puzzle);

        var allocAfter = GC.GetTotalAllocatedBytes(true);

        Assert.True(nakedPairFound);

        Assert.False(puzzle[1, 0].CandidateValues.Contains(8));
        Assert.False(puzzle[1, 0].CandidateValues.Contains(9));

        Assert.True(allocAfter == allocBefore, "No memory should have been allocated.");
    }
}
