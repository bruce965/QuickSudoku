using QuickSudoku.Solvers;
using QuickSudoku.Sudoku;

namespace QuickSudoku.Generators;

public static class SudokuGenerator
{
    public static SudokuPuzzle GenerateSolved(Random random)
    {
        var puzzle = new SudokuPuzzle();
        SolveRecursive(random, puzzle);
        return puzzle;
    }

    public static SudokuPuzzle GenerateSolved()
        => GenerateSolved(Random.Shared);

    static bool SolveRecursive(Random random, SudokuPuzzle puzzle)
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
        var cellIndex = random.Next(cells.Count());
        var cell = cells.ElementAt(cellIndex);

        // start from a random candidate and try all until a valid one is found
        var candidateIndex = random.Next(cell.CandidateValues.Count);
        var candidates = original.Cells[cell.Index].CandidateValues.RingShift(candidateIndex);

        foreach (var candidate in candidates)
        {
            // assign candidate to cell
            cell.Value = candidate;

            // solve naked singles exposed by assigning this candidate
            while (SudokuSolver.SolveNakedSingles(puzzle) > 0);

            // attempt solving
            var solved = SolveRecursive(random, puzzle);

            // solved? forward result
            if (solved)
                return true;

            // not solved? undo changes and try with next candidate
            original.CopyTo(puzzle);
        }

        // no more candidates? unsolvable
        return false;
    }

    static IEnumerable<T> RingShift<T>(this IEnumerable<T> source, int count)
    {
        if (count < 0)
            throw new NotImplementedException("Negative count support not implemented.");

        var hold = new T[count];

        var i = 0;
        foreach (var el in source)
        {
            if (i < count)
                hold[i++] = el;
            else
                yield return el;
        }

        var start = 0;
        var end = count;
        if (i < count)
        {
            start = count % i;
            end = i;
        }

        for (i = start; i < end; i++)
            yield return hold[i];

        for (i = 0; i < start; i++)
            yield return hold[i];
    }
}