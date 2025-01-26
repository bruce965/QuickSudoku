namespace QuickSudoku.Sudoku.Extensions;

public static class HouseExtensions
{
    /// <inheritdoc cref="QuickSudoku.Extensions.HouseExtensions.IsSolved"/>
    public static bool IsSolved(this SudokuHouse house)
    {
        foreach (SudokuCell cell in house.Cells)
            if (!cell.IsSolved())
                return false;

        return true;
    }

    /// <inheritdoc cref="QuickSudoku.Extensions.HouseExtensions.HasSolution"/>
    public static bool HasSolution(this SudokuHouse house)
    {
        foreach (SudokuCell cell in house.Cells)
            if (!cell.HasSolution())
                return false;

        return true;
    }

    /// <inheritdoc cref="QuickSudoku.Extensions.HouseExtensions.Contains"/>
    public static bool Contains(this SudokuHouse house, int value)
    {
        foreach (SudokuCell cell in house.Cells)
            if (cell.Contains(value))
                return true;

        return false;
    }

    #region SudokuRow

    /// <inheritdoc cref="IsSolved"/>
    public static bool IsSolved(this SudokuRow house)
        => ((SudokuHouse)house).IsSolved();

    /// <inheritdoc cref="HasSolution"/>
    public static bool HasSolution(this SudokuRow house)
        => ((SudokuHouse)house).HasSolution();

    /// <inheritdoc cref="Contains"/>
    public static bool Contains(this SudokuRow house, int value)
        => ((SudokuHouse)house).Contains(value);

    #endregion

    #region SudokuColumn

    /// <inheritdoc cref="IsSolved"/>
    public static bool IsSolved(this SudokuColumn house)
        => ((SudokuHouse)house).IsSolved();

    /// <inheritdoc cref="HasSolution"/>
    public static bool HasSolution(this SudokuColumn house)
        => ((SudokuHouse)house).HasSolution();

    /// <inheritdoc cref="Contains"/>
    public static bool Contains(this SudokuColumn house, int value)
        => ((SudokuHouse)house).Contains(value);

    #endregion

    #region SudokuSquare

    /// <inheritdoc cref="IsSolved"/>
    public static bool IsSolved(this SudokuSquare house)
        => ((SudokuHouse)house).IsSolved();

    /// <inheritdoc cref="HasSolution"/>
    public static bool HasSolution(this SudokuSquare house)
        => ((SudokuHouse)house).HasSolution();

    /// <inheritdoc cref="Contains"/>
    public static bool Contains(this SudokuSquare house, int value)
        => ((SudokuHouse)house).Contains(value);

    #endregion
}
