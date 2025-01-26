namespace QuickSudoku.Sudoku.Extensions;

public static class CellExtensions
{
    /// <inheritdoc cref="QuickSudoku.Extensions.CellExtensions.IsSolved"/>
    public static bool IsSolved(this SudokuCell cell)
        => cell.Value is not null;

    /// <inheritdoc cref="QuickSudoku.Extensions.CellExtensions.HasSolution"/>
    public static bool HasSolution(this SudokuCell cell)
        => cell.CandidateValues.Ref is not SudokuDigits.None;

    /// <inheritdoc cref="QuickSudoku.Extensions.CellExtensions.MayContain"/>
    public static bool MayContain(this SudokuCell cell, int value)
        => cell.CandidateValues.Contains(value);

    /// <inheritdoc cref="QuickSudoku.Extensions.CellExtensions.Contains"/>
    public static bool Contains(this SudokuCell cell, int value)
        => cell.Value == value;
}
