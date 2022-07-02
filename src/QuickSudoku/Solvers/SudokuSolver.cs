using QuickSudoku.Sudoku;
using QuickSudoku.Sudoku.Extensions;

namespace QuickSudoku.Solvers;

public static class SudokuSolver
{
    /// <summary>
    /// Solve all naked singles in a puzzle by removing all invalid candidates.
    /// </summary>
    /// <param name="puzzle"></param>
    /// <returns>Number of naked singles solved.</returns>
    public static int SolveNakedSingles(SudokuPuzzle puzzle)
    {
        var nakedSinglesFound = 0;

        foreach (var cell in puzzle.Cells)
        {
            // if cell value is already known, skip cell
            if (cell.Value != null)
                break;

            for (var value = 1; value <= 9; value++)
            {
                // if this value cannot be in this cell (because previously excluded), skip
                if (!cell.CandidateValues.Contains(value))
                    continue;

                // exclude all values on the same row
                if (cell.Row.Contains(value))
                    cell.CandidateValues.Remove(value);

                // exclude all values on the same column
                else if (cell.Column.Contains(value))
                    cell.CandidateValues.Remove(value);

                // exclude all values on the same square
                else if (cell.Square.Contains(value))
                    cell.CandidateValues.Remove(value);

                // if only one possible value remains, a naked single has been found
                if (cell.Value != null)
                {
                    nakedSinglesFound++;
                    break;
                }
            }
        }

        return nakedSinglesFound;
    }

    /// <summary>
    /// Solve a hidden single in a puzzle.
    ///
    /// This method should be called only after calling another method to remove invalid candidates.
    /// </summary>
    /// <param name="puzzle"></param>
    /// <returns><c>true</c> if a hidden single was found and solved.</returns>
    public static bool SolveHiddenSingle(SudokuPuzzle puzzle)
    {
        //int hiddenSinglesFound = 0;

        foreach (var row in puzzle.Rows)
        {
            for (var candidate = 1; candidate <= 9; candidate++)
            {
                SudokuCell? allowedIn = null;
                var skipCandidate = false;

                foreach (var cell in row.Cells)
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

                if (allowedIn != null)
                {
                    // if there is only once cell where this candidate is possible, a hidden single has been found
                    var cell = allowedIn.Value;
                    cell.Value = candidate;

                    //hiddenSinglesFound++;
                    return true;
                }
            }
        }
        
        foreach (var column in puzzle.Columns)
        {
            for (var candidate = 1; candidate <= 9; candidate++)
            {
                SudokuCell? allowedIn = null;
                var skipCandidate = false;

                foreach (var cell in column.Cells)
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

                if (allowedIn != null)
                {
                    // if there is only once cell where this candidate is possible, a hidden single has been found
                    var cell = allowedIn.Value;
                    cell.Value = candidate;

                    //hiddenSinglesFound++;
                    return true;
                }
            }
        }

        foreach (var square in puzzle.Squares)
        {
            for (var candidate = 1; candidate <= 9; candidate++)
            {
                SudokuCell? allowedIn = null;
                var skipCandidate = false;

                foreach (var cell in square.Cells)
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

                if (allowedIn != null)
                {
                    // if there is only once cell where this candidate is possible, a hidden single has been found
                    var cell = allowedIn.Value;
                    cell.Value = candidate;

                    //hiddenSinglesFound++;
                    return true;
                }
            }
        }

        //return hiddenSinglesFound;
        return false;
    }

    /// <summary>
    /// Solve a naked pair in a puzzle.
    ///
    /// This method should be called only after calling another method to remove invalid candidates.
    /// </summary>
    /// <param name="puzzle"></param>
    /// <returns><c>true</c> if a naked pair was found and solved.</returns>
    public static bool SolveNakedPair(SudokuPuzzle puzzle)
    {
        // TODO
        return false;
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

            if (SolveHiddenSingle(puzzle))
            {
                log.Push(SudokuSolutionStrategy.HiddenSingle, 2);
                continue;
            }

            if (SolveNakedPair(puzzle))
            {
                log.Push(SudokuSolutionStrategy.NakedPair, 3);
                continue;
            }

            //if (SolveHiddenPair(puzzle))
            //{
            //    log.Push(SudokuSolutionStrategy.HiddenPair, 4);
            //    continue;
            //}

            // cannot be solved with currently implemented techniques
            break;
        }

        return log;
    }
}
