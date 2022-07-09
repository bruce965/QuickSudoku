using QuickSudoku.Abstractions;
using QuickSudoku.Sudoku;
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

        if (maxCount == 0)
            return 0;

        foreach (var cell in puzzle.Cells)
        {
            // if cell value is already known, skip cell
            if (cell.Value != null)
                continue;

            foreach (var value in cell.CandidateValues)
            {
                // exclude all values in the same house
                foreach (var house in cell.Houses)
                {
                    if (house.Contains(value))
                    {
                        cell.CandidateValues.Remove(value);
                        break;
                    }
                }

                // if only one possible value remains, a naked single has been found
                if (cell.Value != null)
                {
                    nakedSinglesFound++;

                    if (maxCount != -1 && nakedSinglesFound >= maxCount)
                        return nakedSinglesFound;

                    break;
                }
            }
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

    // TODO: I have no idea what this technique is called, I'm keeping it in case it comes useful,
    // although it's probably just an overcomplicated version of the "naked subset" technique.
    //static int SolveNakedGroup(SudokuPuzzle puzzle, int groupSize, int maxCount = -1)
    //{
    //    int nakedGroupsFound = 0;
    //
    //    if (maxCount == 0)
    //        return 0;
    //
    //    foreach (var house in puzzle.Houses)
    //    {
    //        foreach (var intersection in house.IntersectingHouses)
    //        {
    //            for (var candidate = 1; candidate <= 9; candidate++)
    //            {
    //                if (intersection.First.Cells.Except(intersection.Second.Cells).All(c => !c.MayContain(candidate)))
    //                {
    //                    if (intersection.First.Cells.Intersect(intersection.Second.Cells).Where(c => c.MayContain(candidate)).Count() != groupSize)
    //                    {
    //                        // solve only for exact group size (naked pair, naked triple, etc...)
    //                        continue;
    //                    }
    //
    //                    // EXAMPLE:
    //                    // * `intersection.First` is top left corner
    //                    // * `intersection.Second` is leftmost column
    //                    //
    //                    // ,---,
    //                    // |123|... ...
    //                    // |.45|... ...
    //                    // |.67|... ...
    //                    // |-|-'
    //                    // |.|. ... ...
    //                    // |.|. ... ...
    //                    // |.|. ... ...
    //                    // | |
    //                    // |.|. ... ...
    //                    // |.|. ... ...
    //                    // |.|. ... ...
    //                    // '-'
    //                    //
    //                    // Since in the square digits 8 and 9 can only be in
    //                    // a cell shared with the column, 8 and 9 can be removed
    //                    // from all cells in the column that are not shared with
    //                    // the square.
    //
    //                    bool nakedPairFound = false;
    //
    //                    foreach (var cell in intersection.Second.Cells.Except(intersection.First.Cells))
    //                    {
    //                        // TODO: exclude naked triples, quads, etc...
    //
    //                        if (cell.MayContain(candidate))
    //                        {
    //                            cell.CandidateValues.Remove(candidate);
    //                            nakedPairFound = true;
    //                        }
    //                    }
    //
    //                    if (nakedPairFound)
    //                    {
    //                        nakedGroupsFound++;
    //
    //                        if (maxCount != -1 && nakedGroupsFound >= maxCount)
    //                            return nakedGroupsFound;
    //                    }
    //                }
    //            }
    //        }
    //    }
    //
    //    return nakedGroupsFound;
    //}

    /// <summary>
    /// Solve naked subsets of a certain size in a puzzle.
    /// </summary>
    /// <param name="puzzle">Puzzle.</param>
    /// <param name="subsetSize">Size of the subset.</param>
    /// <param name="maxCount">How many naked subsets of the defined size to be solved before stopping.</param>
    /// <returns>Number of naked subsets of the defined size solved.</returns>
    public static int SolveNakedSubset(SudokuPuzzle puzzle, int subsetSize, int maxCount = -1)
    {
        int nakedSubsetsFound = 0;

        if (maxCount == 0)
            return 0;

        // A naked subset of size N occurs when N digits are the sole
        // candidates in N cells of a single house.
        //
        // When a naked subset is found, those digits can be eliminated
        // from candidates in all other cells in the same house.

        foreach (var house in puzzle.Houses)
        {
            foreach (var cell in house.Cells)
            {
                // if a cell in this region has the correct number number of candidates,
                // check if a naked subset is found for these candidates
                if (cell.CandidateValues.Count == subsetSize)
                {
                    // find other cells that only contain these candidates
                    var subsetCells = house.Cells.Where(c => !((IEnumerable<int>)c.CandidateValues).Except(cell.CandidateValues).Any());

                    // if the correct amount of cells is found, a naked subset is found
                    // but it is only useful if another cell in the same house contains one of the candidates
                    if (subsetCells.Count() == subsetSize)
                    {
                        var nakedSubsetFound = false;

                        var otherCells = house.Cells.Except(subsetCells);
                        foreach (var cell2 in otherCells)
                        {
                            foreach (var candidate in cell.CandidateValues)
                            {
                                if (!cell2.CandidateValues.Contains(candidate))
                                    continue;

                                nakedSubsetFound = true;
                                cell2.CandidateValues.Remove(candidate);
                            }
                        }

                        if (nakedSubsetFound)
                        {
                            nakedSubsetsFound++;

                            if (maxCount != -1 && nakedSubsetsFound >= maxCount)
                                return nakedSubsetsFound;
                        }
                    }
                }
            }
        }

        return nakedSubsetsFound;
    }

    /// <summary>
    /// Solve hidden subsets of a certain size in a puzzle.
    /// </summary>
    /// <param name="puzzle">Puzzle.</param>
    /// <param name="subsetSize">Size of the subset.</param>
    /// <param name="maxCount">How many hidden subsets of the defined size to be solved before stopping.</param>
    /// <returns></returns>
    public static int SolveHiddenSubset(SudokuPuzzle puzzle, int subsetSize, int maxCount = -1)
    {
        int hiddenSubsetsFound = 0;

        if (maxCount == 0)
            return 0;

        // A hidden subset of size N occurs when N digits appear
        // as candidates in only N cells of a single house.
        //
        // When a naked subset is found, any other digits in those
        // cells can be eliminated from candidates.

        foreach (var house in puzzle.Houses)
        {
            for (var candidate = 1; candidate <= 9; candidate++)
            {
                var subsetCells = house.Cells.Where(c => c.MayContain(candidate));

                if (subsetCells.Count() == subsetSize)
                {
                    foreach (var cell in subsetCells)
                    {
                        // TODO
                    }
                }
            }
        }

        return hiddenSubsetsFound;
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
        => SolveNakedSubset(puzzle, 2, maxCount);

    /// <summary>
    /// Solve hidden pairs in a puzzle.
    ///
    /// This method should be called only after calling another method to remove invalid candidates.
    /// </summary>
    /// <param name="puzzle">Puzzle</param>
    /// <param name="maxCount">How many hidden pairs to be solved before stopping.</param>
    /// <returns>Number of hidden pairs solved.</returns>
    public static int SolveHiddenPairs(SudokuPuzzle puzzle, int maxCount = -1)
        => SolveHiddenSubset(puzzle, 2, maxCount);

    /// <summary>
    /// Solve naked triples in a puzzle.
    ///
    /// This method should be called only after calling another method to remove invalid candidates.
    /// </summary>
    /// <param name="puzzle">Puzzle</param>
    /// <param name="maxCount">How many naked triples to be solved before stopping.</param>
    /// <returns>Number of naked triples solved.</returns>
    public static int SolveNakedTriples(SudokuPuzzle puzzle, int maxCount = -1)
        => SolveNakedSubset(puzzle, 3, maxCount);

    /// <summary>
    /// Solve hidden triples in a puzzle.
    ///
    /// This method should be called only after calling another method to remove invalid candidates.
    /// </summary>
    /// <param name="puzzle">Puzzle</param>
    /// <param name="maxCount">How many hidden triples to be solved before stopping.</param>
    /// <returns>Number of hidden triples solved.</returns>
    public static int SolveHiddenTriples(SudokuPuzzle puzzle, int maxCount = -1)
        => SolveHiddenSubset(puzzle, 3, maxCount);

    /// <summary>
    /// Solve naked quads in a puzzle.
    ///
    /// This method should be called only after calling another method to remove invalid candidates.
    /// </summary>
    /// <param name="puzzle">Puzzle</param>
    /// <param name="maxCount">How many naked quads to be solved before stopping.</param>
    /// <returns>Number of naked quads solved.</returns>
    public static int SolveNakedQuads(SudokuPuzzle puzzle, int maxCount = -1)
        => SolveNakedSubset(puzzle, 4, maxCount);

    /// <summary>
    /// Solve hidden quads in a puzzle.
    ///
    /// This method should be called only after calling another method to remove invalid candidates.
    /// </summary>
    /// <param name="puzzle">Puzzle</param>
    /// <param name="maxCount">How many hidden quads to be solved before stopping.</param>
    /// <returns>Number of hidden quads solved.</returns>
    public static int SolveHiddenQuads(SudokuPuzzle puzzle, int maxCount = -1)
        => SolveHiddenSubset(puzzle, 4, maxCount);

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
    /// Execute one step towards the solution of a puzzle.
    /// </summary>
    /// <param name="puzzle"></param>
    /// <returns>Log of the strategies adopted to solve the puzzle.</returns>
    public static SudokuSolutionStrategy? SolveStep(SudokuPuzzle puzzle)
        => SolveStep(puzzle, 1, out _);

    /// <summary>
    /// Get the difficulty of a strategy.
    /// </summary>
    /// <param name="strategy"></param>
    /// <param name="firstUse"><c>true</c> to attribute a higher cost on first use of the strategy.</param>
    /// <returns></returns>
    public static int GetStrategyDifficulty(SudokuSolutionStrategy strategy, bool firstUse) => strategy switch
    {
        SudokuSolutionStrategy.NakedSingle => 1,
        SudokuSolutionStrategy.HiddenSingle => 1,
        SudokuSolutionStrategy.NakedPair => firstUse ? 5 : 3,
        SudokuSolutionStrategy.HiddenPair => firstUse ? 10 : 6,
        SudokuSolutionStrategy.NakedTriple => firstUse ? 15 : 10,
        SudokuSolutionStrategy.HiddenTriple => firstUse ? 20 : 12,
        SudokuSolutionStrategy.NakedQuad => firstUse ? 30 : 20,
        SudokuSolutionStrategy.HiddenQuad => firstUse ? 50 : 30,
        SudokuSolutionStrategy.IntersectionRemoval => firstUse ? 12 : 7,
        _ => 0
    };

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

            var adoptedStrategy = SolveStep(puzzle, -1, out var nakedSinglesFound);
            if (adoptedStrategy == null)
            {
                // cannot be solved with currently implemented techniques
                break;
            }

            var isFirstUse = !log.AdoptedStrategies.ContainsKey(adoptedStrategy.Value);
            var difficulty = GetStrategyDifficulty(adoptedStrategy.Value, isFirstUse);
            var count = adoptedStrategy == SudokuSolutionStrategy.NakedSingle ? nakedSinglesFound : 1;
            log.Push(adoptedStrategy.Value, difficulty, count);
        }

        return log;
    }

    static SudokuSolutionStrategy? SolveStep(SudokuPuzzle puzzle, int maxNakedSingles, out int nakedSinglesFound)
    {
        nakedSinglesFound = SolveNakedSingles(puzzle, maxNakedSingles);
        if (nakedSinglesFound > 0)
            return SudokuSolutionStrategy.NakedSingle;

        var hiddenSinglesFound = SolveHiddenSingles(puzzle, 1);
        if (hiddenSinglesFound > 0)
            return SudokuSolutionStrategy.HiddenSingle;

        var nakedPairsFound = SolveNakedPairs(puzzle, 1);
        if (nakedPairsFound > 0)
            return SudokuSolutionStrategy.NakedPair;

        var hiddenPairsFound = SolveHiddenPairs(puzzle, 1);
        if (hiddenPairsFound > 0)
            return SudokuSolutionStrategy.HiddenPair;

        var nakedTriplesFound = SolveNakedTriples(puzzle, 1);
        if (nakedTriplesFound > 0)
            return SudokuSolutionStrategy.NakedTriple;

        var hiddenTriplesFound = SolveHiddenTriples(puzzle, 1);
        if (hiddenTriplesFound > 0)
            return SudokuSolutionStrategy.HiddenTriple;

        var nakedQuadsFound = SolveNakedQuads(puzzle, 1);
        if (nakedQuadsFound > 0)
            return SudokuSolutionStrategy.NakedQuad;;

        var hiddenQuadsFound = SolveHiddenQuads(puzzle, 1);
        if (hiddenQuadsFound > 0)
            return SudokuSolutionStrategy.HiddenQuad;

        var (removedIntersections, _) = SolveIntersectionRemovals(puzzle, 1);
        if (removedIntersections > 0)
            return SudokuSolutionStrategy.IntersectionRemoval;

        return null;
    }
}
