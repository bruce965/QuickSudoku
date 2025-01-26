// SPDX-FileCopyrightText: Copyright 2025 Fabio Iotti
// SPDX-License-Identifier: AGPL-3.0-only

using QuickSudoku.Sudoku;
using QuickSudoku.Sudoku.Extensions;

namespace QuickSudoku.Solvers;

partial class SudokuSolver
{
    /// <summary>
    /// Solve naked singles in a puzzle.
    /// </summary>
    /// <param name="puzzle">Puzzle.</param>
    /// <param name="maxCount">How many naked singles to be solved before stopping.</param>
    /// <returns>Number of naked singles solved.</returns>
    public static int SolveNakedSingles(SudokuPuzzle puzzle, int maxCount = -1)
    {
        int nakedSinglesFound = 0;

        if (maxCount is 0)
            return 0;

        foreach (SudokuCell cell in puzzle.Cells)
        {
            // if cell value is already known, skip cell
            if (cell.Value is not null)
                continue;

            foreach (int value in cell.CandidateValues)
            {
                // exclude all values in the same house
                foreach (SudokuHouse house in cell.Houses)
                {
                    if (house.Contains(value))
                    {
                        cell.CandidateValues.Remove(value);
                        break;
                    }
                }

                // if only one possible value remains, a naked single has been found
                if (cell.Value is not null)
                {
                    nakedSinglesFound++;

                    if (maxCount is not -1 && nakedSinglesFound >= maxCount)
                        return nakedSinglesFound;

                    break;
                }
            }
        }

        return nakedSinglesFound;
    }
}
