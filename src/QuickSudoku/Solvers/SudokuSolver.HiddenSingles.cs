// SPDX-FileCopyrightText: Copyright 2025 Fabio Iotti
// SPDX-License-Identifier: AGPL-3.0-only

using QuickSudoku.Sudoku;
using QuickSudoku.Sudoku.Extensions;

namespace QuickSudoku.Solvers;

partial class SudokuSolver
{
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
}
