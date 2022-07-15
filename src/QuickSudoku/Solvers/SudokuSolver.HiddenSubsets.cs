using QuickSudoku.Sudoku;
using QuickSudoku.Sudoku.Extensions;

namespace QuickSudoku.Solvers;

partial class SudokuSolver
{
    /// <summary>
    /// Solve hidden subsets of a certain size in a puzzle.
    /// </summary>
    /// <param name="puzzle">Puzzle.</param>
    /// <param name="subsetSize">Size of the subset.</param>
    /// <param name="maxCount">How many hidden subsets of the defined size to be solved before stopping.</param>
    /// <returns></returns>
    public static int SolveHiddenSubsets(SudokuPuzzle puzzle, int subsetSize, int maxCount = -1)
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
    /// Solve hidden pairs in a puzzle.
    ///
    /// This method should be called only after calling another method to remove invalid candidates.
    /// </summary>
    /// <param name="puzzle">Puzzle</param>
    /// <param name="maxCount">How many hidden pairs to be solved before stopping.</param>
    /// <returns>Number of hidden pairs solved.</returns>
    public static int SolveHiddenPairs(SudokuPuzzle puzzle, int maxCount = -1)
        => SolveHiddenSubsets(puzzle, 2, maxCount);

    /// <summary>
    /// Solve hidden triples in a puzzle.
    ///
    /// This method should be called only after calling another method to remove invalid candidates.
    /// </summary>
    /// <param name="puzzle">Puzzle</param>
    /// <param name="maxCount">How many hidden triples to be solved before stopping.</param>
    /// <returns>Number of hidden triples solved.</returns>
    public static int SolveHiddenTriples(SudokuPuzzle puzzle, int maxCount = -1)
        => SolveHiddenSubsets(puzzle, 3, maxCount);

    /// <summary>
    /// Solve hidden quads in a puzzle.
    ///
    /// This method should be called only after calling another method to remove invalid candidates.
    /// </summary>
    /// <param name="puzzle">Puzzle</param>
    /// <param name="maxCount">How many hidden quads to be solved before stopping.</param>
    /// <returns>Number of hidden quads solved.</returns>
    public static int SolveHiddenQuads(SudokuPuzzle puzzle, int maxCount = -1)
        => SolveHiddenSubsets(puzzle, 4, maxCount);
}
