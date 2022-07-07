﻿using QuickSudoku.Sudoku;
using QuickSudoku.Sudoku.Extensions;

namespace QuickSudoku.Solvers;

public static class SudokuSolver
{
    /// <summary>
    /// Solve naked singles in a puzzle.
    /// </summary>
    /// <param name="puzzle">Puzzle.</param>
    /// <param name="maxCount">How many naked singles to be solved before stopping.</param>
    /// <returns>Number of naked singles solved.</returns>
    public static int SolveNakedSingles(SudokuPuzzle puzzle, int maxCount = -1)
    {
        var nakedSinglesFound = 0;

        foreach (var cell in puzzle.Cells)
        {
            if (maxCount != -1 && nakedSinglesFound >= maxCount)
                return nakedSinglesFound;

            // if cell value is already known, skip cell
            if (cell.Value != null)
                continue;

            foreach (var value in cell.CandidateValues)
            {
                // exclude all values in the same house
                foreach (var house in cell.Houses)
                    if (house.Contains(value))
                        cell.CandidateValues.Remove(value);
            }

            // if only one possible value remains, a naked single has been found
            if (cell.Value != null)
                nakedSinglesFound++;
        }

        return nakedSinglesFound;
    }

    /// <summary>
    /// Solve hidden singles in a puzzle.
    ///
    /// This method should be called only after calling another method to remove invalid candidates.
    /// </summary>
    /// <param name="puzzle">Puzzle.</param>
    /// <param name="maxCount">How many hidden singles to be solved before stopping.</param>
    /// <returns>Number of hidden singles solved.</returns>
    public static int SolveHiddenSingles(SudokuPuzzle puzzle, int maxCount = -1)
    {
        int hiddenSinglesFound = 0;

        foreach (var house in puzzle.Houses)
        {
            for (var candidate = 1; candidate <= 9; candidate++)
            {
                if (maxCount != -1 && hiddenSinglesFound >= maxCount)
                    return hiddenSinglesFound;

                SudokuCell? allowedIn = null;
                var skipCandidate = false;

                foreach (var cell in house.Cells)
                {
                    if (cell.MayContain(candidate))
                    {
                        if (allowedIn != null)
                        {
                            // allowed in multiple cells? not a hidden single
                            skipCandidate = true;
                            break;
                        }

                        allowedIn = cell;
                    }
                }

                if (skipCandidate)
                    continue;

                if (allowedIn != null && !allowedIn.Value.IsSolved())
                {
                    // if there is only once cell where this candidate is possible, a hidden single has been found
                    var cell = allowedIn.Value;
                    cell.Value = candidate;

                    hiddenSinglesFound++;
                }
            }
        }
        
        return hiddenSinglesFound;
    }

    static int SolveNakedGroups(SudokuPuzzle puzzle, int groupSize, int maxCount = -1)
    {
        int nakedGroupsFound = 0;

        if (maxCount == 0)
            return 0;

        foreach (var house in puzzle.Houses)
        {
            foreach (var intersection in house.IntersectingHouses)
            {
                for (var candidate = 1; candidate <= 9; candidate++)
                {
                    if (intersection.First.Cells.Except(intersection.Second.Cells).All(c => !c.MayContain(candidate)))
                    {
                        if (intersection.First.Cells.Intersect(intersection.Second.Cells).Where(c => c.MayContain(candidate)).Count() != groupSize)
                        {
                            // solve only for exact group size (naked pair, naked triple, etc...)
                            continue;
                        }

                        // EXAMPLE:
                        // * `intersection.First` is top left corner
                        // * `intersection.Second` is leftmost column
                        //
                        // ,---,
                        // |123|... ...
                        // |.45|... ...
                        // |.67|... ...
                        // |-|-'
                        // |.|. ... ...
                        // |.|. ... ...
                        // |.|. ... ...
                        // | |
                        // |.|. ... ...
                        // |.|. ... ...
                        // |.|. ... ...
                        // '-'
                        //
                        // Since in the square digits 8 and 9 can only be in
                        // a cell shared with the column, 8 and 9 can be removed
                        // from all cells in the column that are not shared with
                        // the square.

                        bool nakedPairFound = false;

                        foreach (var cell in intersection.Second.Cells.Except(intersection.First.Cells))
                        {
                            // TODO: exclude naked triples, quads, etc...

                            if (cell.MayContain(candidate))
                            {
                                cell.CandidateValues.Remove(candidate);
                                nakedPairFound = true;
                            }
                        }

                        if (nakedPairFound)
                        {
                            nakedGroupsFound++;

                            if (maxCount != -1 && nakedGroupsFound >= maxCount)
                                return nakedGroupsFound;
                        }
                    }
                }
            }
        }

        return nakedGroupsFound;
    }

    /// <summary>
    /// Solve naked pairs in a puzzle.
    ///
    /// This method should be called only after calling another method to remove invalid candidates.
    /// </summary>
    /// <param name="puzzle">Puzzle</param>
    /// <param name="maxCount">How many naked pairs to be solved before stopping.</param>
    /// <returns>Number of naked pairs solved.</returns>
    public static int SolveNakedPairs(SudokuPuzzle puzzle, int maxCount = -1)
        => SolveNakedGroups(puzzle, 2, maxCount);

    /// <summary>
    /// Solve hidden pairs in a puzzle.
    ///
    /// This method should be called only after calling another method to remove invalid candidates.
    /// </summary>
    /// <param name="puzzle">Puzzle</param>
    /// <param name="maxCount">How many hidden pairs to be solved before stopping.</param>
    /// <returns>Number of hidden pairs solved.</returns>
    public static int SolveHiddenPairs(SudokuPuzzle puzzle, int maxCount = -1)
    {
        // TODO
        return 0;
    }

    /// <summary>
    /// Solve naked triples in a puzzle.
    ///
    /// This method should be called only after calling another method to remove invalid candidates.
    /// </summary>
    /// <param name="puzzle">Puzzle</param>
    /// <param name="maxCount">How many naked triples to be solved before stopping.</param>
    /// <returns>Number of naked triples solved.</returns>
    public static int SolveNakedTriples(SudokuPuzzle puzzle, int maxCount = -1)
        => SolveNakedGroups(puzzle, 3, maxCount);

    /// <summary>
    /// Solve hidden triples in a puzzle.
    ///
    /// This method should be called only after calling another method to remove invalid candidates.
    /// </summary>
    /// <param name="puzzle">Puzzle</param>
    /// <param name="maxCount">How many hidden triples to be solved before stopping.</param>
    /// <returns>Number of hidden triples solved.</returns>
    public static int SolveHiddenTriples(SudokuPuzzle puzzle, int maxCount = -1)
    {
        // TODO
        return 0;
    }

    /// <summary>
    /// Solve naked quads in a puzzle.
    ///
    /// This method should be called only after calling another method to remove invalid candidates.
    /// </summary>
    /// <param name="puzzle">Puzzle</param>
    /// <param name="maxCount">How many naked quads to be solved before stopping.</param>
    /// <returns>Number of naked quads solved.</returns>
    public static int SolveNakedQuads(SudokuPuzzle puzzle, int maxCount = -1)
        => SolveNakedGroups(puzzle, 4, maxCount);

    /// <summary>
    /// Solve hidden quads in a puzzle.
    ///
    /// This method should be called only after calling another method to remove invalid candidates.
    /// </summary>
    /// <param name="puzzle">Puzzle</param>
    /// <param name="maxCount">How many hidden quads to be solved before stopping.</param>
    /// <returns>Number of hidden quads solved.</returns>
    public static int SolveHiddenQuads(SudokuPuzzle puzzle, int maxCount = -1)
    {
        // TODO
        return 0;
    }

    /// <summary>
    /// Solve throgh intersection removal in a puzzle.
    ///
    /// This method should be called only after calling another method to remove invalid candidates.
    /// </summary>
    /// <param name="puzzle">Puzzle</param>
    /// <param name="maxIntersectionsCount">How many intersections to be removed before stopping.</param>
    /// <returns>Number of intersections and candidates removed.</returns>
    public static (int Intersections, int Candidates) SolveIntersectionRemovals(SudokuPuzzle puzzle, int maxIntersectionsCount = -1)
    {
        // TODO
        return (0, 0);
    }

    /// <summary>
    /// Solve a puzzle.
    /// </summary>
    /// <param name="puzzle"></param>
    /// <returns>Log of the strategies adopted to solve the puzzle.</returns>
    public static ISudokuSolutionLog Solve(SudokuPuzzle puzzle, int stopAtDifficulty = -1)
    {
        var log = new SudokuSolutionLog();

        while (true)
        {
            log.Solved = puzzle.IsSolved();

            if (log.Solved)
                break;

            if (stopAtDifficulty != -1 && log.Difficulty > stopAtDifficulty)
                break;

            if (!puzzle.HasSolution())
                break;

            var nakedSinglesFound = SolveNakedSingles(puzzle);
            if (nakedSinglesFound > 0)
            {
                log.Push(SudokuSolutionStrategy.NakedSingle, 1, nakedSinglesFound);
                continue;
            }

            var hiddenSinglesFound = SolveHiddenSingles(puzzle, 1);
            if (hiddenSinglesFound > 0)
            {
                log.Push(SudokuSolutionStrategy.HiddenSingle, 2, hiddenSinglesFound);
                continue;
            }

            var nakedPairsFound = SolveNakedPairs(puzzle, 1);
            if (nakedPairsFound > 0)
            {
                log.Push(SudokuSolutionStrategy.NakedPair, 3, nakedPairsFound);
                continue;
            }

            var hiddenPairsFound = SolveHiddenPairs(puzzle, 1);
            if (hiddenPairsFound > 0)
            {
                log.Push(SudokuSolutionStrategy.HiddenPair, 4, hiddenPairsFound);
                continue;
            }

            var nakedTriplesFound = SolveNakedTriples(puzzle, 1);
            if (nakedTriplesFound > 0)
            {
                log.Push(SudokuSolutionStrategy.NakedTriple, 3, nakedTriplesFound);
                continue;
            }

            var hiddenTriplesFound = SolveHiddenTriples(puzzle, 1);
            if (hiddenTriplesFound > 0)
            {
                log.Push(SudokuSolutionStrategy.HiddenTriple, 4, hiddenTriplesFound);
                continue;
            }

            var nakedQuadsFound = SolveNakedQuads(puzzle, 1);
            if (nakedQuadsFound > 0)
            {
                log.Push(SudokuSolutionStrategy.NakedQuad, 3, nakedQuadsFound);
                continue;
            }

            var hiddenQuadsFound = SolveHiddenQuads(puzzle, 1);
            if (hiddenQuadsFound > 0)
            {
                log.Push(SudokuSolutionStrategy.HiddenQuad, 4, hiddenQuadsFound);
                continue;
            }

            var (removedIntersections, _) = SolveIntersectionRemovals(puzzle, 1);
            if (removedIntersections > 0)
            {
                log.Push(SudokuSolutionStrategy.IntersectionRemoval, 4, hiddenQuadsFound);
                continue;
            }

            // cannot be solved with currently implemented techniques
            break;
        }

        return log;
    }
}
