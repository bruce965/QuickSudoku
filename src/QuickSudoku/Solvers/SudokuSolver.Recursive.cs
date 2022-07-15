using QuickSudoku.Sudoku;
using QuickSudoku.Utilities;

namespace QuickSudoku.Solvers;

partial class SudokuSolver
{
    /// <summary>
    /// Solve by recursive strategy .
    ///
    /// <para>This strategy is slow (about 1ms per-cell) and will
    /// solve the puzzle completely, assuming a solution exists.</para>
    /// </summary>
    /// <param name="puzzle">Puzzle.</param>
    /// <param name="random">
    /// For puzzles with multiple solutions, using different random generators
    /// can help explore the solutions space.
    /// </param>
    /// <returns><c>true</c> if a solution was found.</returns>
    public static bool SolveRecursive(SudokuPuzzle puzzle, Random? random = null)
    {
        var original = puzzle.Clone();

        // get cells with the smallest amount of candidates
        var cells = puzzle.Cells
            .Select(c => (Cell: c, CandidatesCount: c.CandidateValues.Count))
            .Where(c => c.CandidatesCount > 1)
            .GroupBy(c => c.CandidatesCount)
            .OrderBy(g => g.Key)
            .FirstOrDefault()?
            .Select(c => c.Cell);

        // if there are no unsolved cells left, the puzzle is solved
        if (cells == null)
            return true;

        // pick a random cell from those with the smallest amount of candidates
        var cellIndex = random?.Next(cells.Count()) ?? 0;
        var cell = cells.ElementAt(cellIndex);

        // start from a random candidate and try all until a valid one is found
        var candidateIndex = random?.Next(cell.CandidateValues.Count) ?? 0;
        var candidates = original.Cells[cell.Index].CandidateValues.RingShift(candidateIndex);

        foreach (var candidate in candidates)
        {
            // assign candidate to cell
            cell.Value = candidate;

            // solve naked singles exposed by assigning this candidate
            while (SolveNakedSingles(puzzle) > 0) { }

            // attempt solving
            var solved = SolveRecursive(puzzle, random);

            // solved? forward result
            if (solved)
                return true;

            // not solved? undo changes and try with next candidate
            original.CopyTo(puzzle);
        }

        // no more candidates? unsolvable
        return false;
    }
}
