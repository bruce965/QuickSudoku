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
}
