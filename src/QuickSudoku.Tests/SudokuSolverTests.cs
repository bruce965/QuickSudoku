// SPDX-FileCopyrightText: Copyright 2025 Fabio Iotti
// SPDX-License-Identifier: AGPL-3.0-only

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
        SudokuPuzzle puzzle = SudokuPuzzle.FromScheme(@"
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

        long allocBefore = GC.GetTotalAllocatedBytes(true);

        int nakedSinglesCount = SudokuSolver.SolveNakedSingles(puzzle);

        long allocAfter = GC.GetTotalAllocatedBytes(true);

        Assert.Equal(1, nakedSinglesCount);

        Assert.Equal(9, puzzle[0, 0].Value);

        Assert.True(allocAfter == allocBefore, "No memory should have been allocated.");
    }

    [Fact]
    public void TestHiddenSingle()
    {
        SudokuPuzzle puzzle = SudokuPuzzle.FromScheme(@"
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

        long allocBefore = GC.GetTotalAllocatedBytes(true);

        int nakedSinglesFound = SudokuSolver.SolveHiddenSingles(puzzle);

        long allocAfter = GC.GetTotalAllocatedBytes(true);

        Assert.Equal(1, nakedSinglesFound);

        Assert.Equal(1, puzzle[0, 0].Value);

        Assert.True(allocAfter == allocBefore, "No memory should have been allocated.");
    }

    [Fact]
    public void TestNakedPair()
    {
        SudokuPuzzle puzzle = SudokuPuzzle.FromScheme(@"
           >.12 ... ...
           >.34 ... ...
            567 ... ...

           >... ... ...
            ... ... ...
            ... ... ...

            ... ... ...
            ... ... ...
            ... ... ...
        ");

        // remove candidates through other methods before attempting to solve naked pairs
        SudokuSolver.SolveNakedSingles(puzzle);

        // ensure that previous methods did not already find the naked pair
        Assert.Equal(SudokuDigits.Digit8 | SudokuDigits.Digit9, puzzle[0, 0].CandidateValues.Digits);
        Assert.Equal(SudokuDigits.Digit8 | SudokuDigits.Digit9, puzzle[0, 1].CandidateValues.Digits);

        Assert.True(puzzle[0, 3].CandidateValues.Contains(8));
        Assert.True(puzzle[0, 3].CandidateValues.Contains(9));

        long allocBefore = GC.GetTotalAllocatedBytes(true);

        int nakedPairsFound = SudokuSolver.SolveNakedPairs(puzzle);

        long allocAfter = GC.GetTotalAllocatedBytes(true);

        Assert.Equal(1, nakedPairsFound);

        Assert.False(puzzle[0, 3].CandidateValues.Contains(8));
        Assert.False(puzzle[0, 3].CandidateValues.Contains(9));

        //Assert.True(allocAfter == allocBefore, "No memory should have been allocated.");
    }

    [Fact]
    public void TestNakedTriple()
    {
        SudokuPuzzle puzzle = SudokuPuzzle.FromScheme(@"
           >.12 ... ...
           >.34 ... ...
            .56 ... ...

           >... ... ...
            ... ... ...
            ... ... ...

            ... ... ...
            ... ... ...
            ... ... ...
        ");

        // remove candidates through other methods before attempting to solve naked pairs
        SudokuSolver.SolveNakedSingles(puzzle);

        // ensure that previous methods did not already find the naked pair
        Assert.Equal(SudokuDigits.Digit7 | SudokuDigits.Digit8 | SudokuDigits.Digit9, puzzle[0, 0].CandidateValues.Digits);
        Assert.Equal(SudokuDigits.Digit7 | SudokuDigits.Digit8 | SudokuDigits.Digit9, puzzle[0, 1].CandidateValues.Digits);

        Assert.True(puzzle[0, 3].CandidateValues.Contains(7));
        Assert.True(puzzle[0, 3].CandidateValues.Contains(8));
        Assert.True(puzzle[0, 3].CandidateValues.Contains(9));

        long allocBefore = GC.GetTotalAllocatedBytes(true);

        int nakedPairsFound = SudokuSolver.SolveNakedTriples(puzzle);

        long allocAfter = GC.GetTotalAllocatedBytes(true);

        Assert.Equal(1, nakedPairsFound);

        Assert.False(puzzle[0, 3].CandidateValues.Contains(7));
        Assert.False(puzzle[0, 3].CandidateValues.Contains(8));
        Assert.False(puzzle[0, 3].CandidateValues.Contains(9));

        //Assert.True(allocAfter == allocBefore, "No memory should have been allocated.");
    }

    //[Fact]
    //public void TestNakedPair()
    //{
    //    SudokuPuzzle puzzle = SudokuPuzzle.FromScheme(@"
    //        123 ... ...
    //        .45 ... ...
    //        .67 ... ...
    //
    //       >... ... ...
    //       >... ... ...
    //       >... ... ...
    //
    //       >... ... ...
    //       >... ... ...
    //       >... ... ...
    //    ");
    //
    //    // remove candidates through other methods before attempting to solve naked pairs
    //    SudokuSolver.SolveNakedSingles(puzzle);
    //
    //    // ensure that previous methods did not already find the naked pairs
    //    Assert.Equal(SudokuDigits.Digit8 | SudokuDigits.Digit9, puzzle[0, 1].CandidateValues.Digits);
    //    Assert.Equal(SudokuDigits.Digit8 | SudokuDigits.Digit9, puzzle[0, 2].CandidateValues.Digits);
    //
    //    Assert.True(puzzle[0, 3].CandidateValues.Contains(8));
    //    Assert.True(puzzle[0, 3].CandidateValues.Contains(9));
    //
    //    long allocBefore = GC.GetTotalAllocatedBytes(true);
    //
    //    int nakedPairsFound = SudokuSolver.SolveNakedPairs(puzzle);
    //
    //    long allocAfter = GC.GetTotalAllocatedBytes(true);
    //
    //    Assert.Equal(2, nakedPairsFound);
    //
    //    Assert.False(puzzle[0, 3].CandidateValues.Contains(8));
    //    Assert.False(puzzle[0, 3].CandidateValues.Contains(9));
    //
    //    //Assert.True(allocAfter == allocBefore, "No memory should have been allocated.");
    //}

    //[Fact]
    //public void TestNakedTriple()
    //{
    //    SudokuPuzzle puzzle = SudokuPuzzle.FromScheme(@"
    //        .12 ... ...
    //        .34 ... ...
    //        .56 ... ...
    //
    //       >... ... ...
    //       >... ... ...
    //       >... ... ...
    //
    //       >... ... ...
    //       >... ... ...
    //       >... ... ...
    //    ");
    //
    //    // remove candidates through other methods before attempting to solve naked triples
    //    SudokuSolver.SolveNakedSingles(puzzle);
    //    SudokuSolver.SolveNakedPairs(puzzle);
    //
    //    Assert.Equal(SudokuDigits.Digit7 | SudokuDigits.Digit8 | SudokuDigits.Digit9, puzzle[0, 1].CandidateValues.Digits);
    //    Assert.Equal(SudokuDigits.Digit7 | SudokuDigits.Digit8 | SudokuDigits.Digit9, puzzle[0, 2].CandidateValues.Digits);
    //
    //    // ensure that previous methods did not already find the naked triples
    //    Assert.True(puzzle[0, 3].CandidateValues.Contains(7));
    //    Assert.True(puzzle[0, 3].CandidateValues.Contains(8));
    //    Assert.True(puzzle[0, 3].CandidateValues.Contains(9));
    //
    //    long allocBefore = GC.GetTotalAllocatedBytes(true);
    //
    //    int nakedTriplesFound = SudokuSolver.SolveNakedTriples(puzzle);
    //
    //    long allocAfter = GC.GetTotalAllocatedBytes(true);
    //
    //    Assert.Equal(3, nakedTriplesFound);
    //
    //    Assert.False(puzzle[0, 3].CandidateValues.Contains(7));
    //    Assert.False(puzzle[0, 3].CandidateValues.Contains(8));
    //    Assert.False(puzzle[0, 3].CandidateValues.Contains(9));
    //
    //    //Assert.True(allocAfter == allocBefore, "No memory should have been allocated.");
    //}
}
