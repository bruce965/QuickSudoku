using QuickSudoku.Abstractions;

namespace QuickSudoku.Extensions;

public static class PuzzleExtensions
{
    public static bool IsSolved(this IPuzzle puzzle)
        => puzzle.Cells.All(c => c.IsSolved());

    public static bool HasSolution(this IPuzzle puzzle)
        => puzzle.Cells.All(c => c.HasSolution());
}
