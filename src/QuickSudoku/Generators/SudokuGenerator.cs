using QuickSudoku.Solvers;
using QuickSudoku.Sudoku;

namespace QuickSudoku.Generators;

public static class SudokuGenerator
{
    /// <summary>
    /// Generate a new valid random solved puzzle.
    ///
    /// <para>This operation is slow (about 50ms to 100ms).</para>
    /// </summary>
    /// <param name="random">Source of randomness used to generate a puzzle.</param>
    /// <returns>Random solved puzzle.</returns>
    public static SudokuPuzzle GenerateSolved(Random? random = null)
    {
        var puzzle = new SudokuPuzzle();
        SudokuSolver.SolveRecursive(puzzle, random ?? Random.Shared);

        return puzzle;
    }
}