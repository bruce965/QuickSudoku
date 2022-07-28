using QuickSudoku.Sudoku;
using QuickSudoku.Sudoku.Extensions;
using QuickSudoku.Utilities;

namespace QuickSudoku.Solvers;

partial class SudokuSolver
{
    /// <summary>
    /// Solve by recursive strategy .
    ///
    /// <para>This strategy will solve the puzzle completely, assuming a solution exists.</para>
    /// </summary>
    /// <param name="puzzle">Puzzle.</param>
    /// <param name="random">
    /// For puzzles with multiple solutions, using different random generators
    /// can help explore the solutions space.
    /// </param>
    /// <returns><c>true</c> if a solution was found.</returns>
    public static bool SolveRecursive(SudokuPuzzle puzzle, Random? random = null)
    {
        // initialize list of values that may be in each cell of the board
        Span<SudokuDigits> allowed = stackalloc SudokuDigits[81];
        foreach (ref var c in allowed)
            c = SudokuDigits.All;

        // do not allow backtracking on cells that were initially assigned in the puzzle
        Span<bool> initiallyAssigned = stackalloc bool[allowed.Length];
        for (var i = 0; i < initiallyAssigned.Length; i++)
            initiallyAssigned[i] = puzzle[i].Value != null;

        for (var i = 0; i < allowed.Length; i++)
        {
            // skip cells which already had a value in original puzzle
            if (initiallyAssigned[i])
                continue;

            var cell = puzzle[i];

            // start from a random candidate among those not yet tried in this cell
            var candidatesCount = allowed[i].Count();
            var randomShift = random == null ? 0 : random.Next(0, candidatesCount);

            foreach (var candidate in allowed[i].GetDigits().RingShift(randomShift))
            {
                // remove candidate from values allowed on this cell, to avoid trying it again
                allowed[i].Remove(candidate);

                if (!cell.Houses.Any(h => h.Contains(candidate)))
                {
                    // valid candidate, attempt to assign it to this cell
                    cell.Value = candidate;
                    break;
                }
            }

            // valid candidate found for this cell, continue
            if (cell.Value != null)
                continue;

            // reset all candidates on this cell
            allowed[i] = SudokuDigits.All;

            // return to previous cell and try with another candidate
            while (true)
            {
                if (--i == -1)
                {
                    // all possibile solutions have been tried, puzzle has no solution
                    return false;
                }

                // skip cells which were already assigned in original puzzle
                if (initiallyAssigned[i])
                    continue;

                // unset previous cell value and try again
                var previousCell = puzzle[i--];
                previousCell.Value = null;
                break;
            }
        }

        // all cells have a value
        return true;
    }
}
