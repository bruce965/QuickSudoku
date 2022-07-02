namespace QuickSudoku.Sudoku.Extensions;

public static class RegionExtensions
{
    #region SudokuRow

    /// <inheritdoc cref="QuickSudoku.Extensions.RegionExtensions.IsSolved"/>
    public static bool IsSolved(this SudokuRow row)
    {
        foreach (var cell in row.Cells)
            if (!cell.IsSolved())
                return false;

        return true;
    }

    /// <inheritdoc cref="QuickSudoku.Extensions.RegionExtensions.HasSolution"/>
    public static bool HasSolution(this SudokuRow row)
    {
        foreach (var cell in row.Cells)
            if (!cell.HasSolution())
                return false;

        return true;
    }

    /// <inheritdoc cref="QuickSudoku.Extensions.RegionExtensions.MayContain"/>
    public static bool MayContain(this SudokuRow row, int value)
    {
        foreach (var cell in row.Cells)
            if (cell.MayContain(value))
                return true;

        return false;
    }

    /// <inheritdoc cref="QuickSudoku.Extensions.RegionExtensions.Contains"/>
    public static bool Contains(this SudokuRow row, int value)
    {
        foreach (var cell in row.Cells)
            if (cell.Contains(value))
                return true;

        return false;
    }

    #endregion

    #region SudokuColumn

    /// <inheritdoc cref="QuickSudoku.Extensions.RegionExtensions.IsSolved"/>
    public static bool IsSolved(this SudokuColumn column)
    {
        foreach (var cell in column.Cells)
            if (!cell.IsSolved())
                return false;

        return true;
    }

    /// <inheritdoc cref="QuickSudoku.Extensions.RegionExtensions.HasSolution"/>
    public static bool HasSolution(this SudokuColumn column)
    {
        foreach (var cell in column.Cells)
            if (!cell.HasSolution())
                return false;

        return true;
    }

    /// <inheritdoc cref="QuickSudoku.Extensions.RegionExtensions.MayContain"/>
    public static bool MayContain(this SudokuColumn column, int value)
    {
        foreach (var cell in column.Cells)
            if (cell.MayContain(value))
                return true;

        return false;
    }

    /// <inheritdoc cref="QuickSudoku.Extensions.RegionExtensions.Contains"/>
    public static bool Contains(this SudokuColumn column, int value)
    {
        foreach (var cell in column.Cells)
            if (cell.Contains(value))
                return true;

        return false;
    }

    #endregion

    #region SudokuSquare

    /// <inheritdoc cref="QuickSudoku.Extensions.RegionExtensions.IsSolved"/>
    public static bool IsSolved(this SudokuSquare square)
    {
        foreach (var cell in square.Cells)
            if (!cell.IsSolved())
                return false;

        return true;
    }

    /// <inheritdoc cref="QuickSudoku.Extensions.RegionExtensions.HasSolution"/>
    public static bool HasSolution(this SudokuSquare square)
    {
        foreach (var cell in square.Cells)
            if (!cell.HasSolution())
                return false;

        return true;
    }

    /// <inheritdoc cref="QuickSudoku.Extensions.RegionExtensions.MayContain"/>
    public static bool MayContain(this SudokuSquare square, int value)
    {
        foreach (var cell in square.Cells)
            if (cell.MayContain(value))
                return true;

        return false;
    }

    /// <inheritdoc cref="QuickSudoku.Extensions.RegionExtensions.Contains"/>
    public static bool Contains(this SudokuSquare square, int value)
    {
        foreach (var cell in square.Cells)
            if (cell.Contains(value))
                return true;

        return false;
    }

    #endregion
}
