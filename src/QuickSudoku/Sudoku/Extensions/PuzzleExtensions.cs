namespace QuickSudoku.Sudoku.Extensions;

public static class PuzzleExtensions
{
    /// <inheritdoc cref="QuickSudoku.Extensions.PuzzleExtensions.IsSolved"/>
    public static bool IsSolved(this SudokuPuzzle puzzle)
    {
        foreach (SudokuCell cell in puzzle.Cells)
            if (!cell.IsSolved())
                return false;

        return true;
    }

    /// <inheritdoc cref="QuickSudoku.Extensions.PuzzleExtensions.HasSolution"/>
    public static bool HasSolution(this SudokuPuzzle puzzle)
    {
        foreach (SudokuCell cell in puzzle.Cells)
            if (!cell.HasSolution())
                return false;

        return true;
    }
}
