namespace QuickSudoku.Sudoku.Extensions;

public static class RegionExtensions
{
    /// <inheritdoc cref="QuickSudoku.Extensions.RegionExtensions.IsSolved"/>
    public static bool IsSolved(this SudokuRegion region)
    {
        foreach (var cell in region.Cells)
            if (!cell.IsSolved())
                return false;

        return true;
    }

    /// <inheritdoc cref="QuickSudoku.Extensions.RegionExtensions.HasSolution"/>
    public static bool HasSolution(this SudokuRegion region)
    {
        foreach (var cell in region.Cells)
            if (!cell.HasSolution())
                return false;

        return true;
    }

    /// <inheritdoc cref="QuickSudoku.Extensions.RegionExtensions.MayContain"/>
    public static bool MayContain(this SudokuRegion region, int value)
    {
        foreach (var cell in region.Cells)
            if (cell.MayContain(value))
                return true;

        return false;
    }

    /// <inheritdoc cref="QuickSudoku.Extensions.RegionExtensions.Contains"/>
    public static bool Contains(this SudokuRegion region, int value)
    {
        foreach (var cell in region.Cells)
            if (cell.Contains(value))
                return true;

        return false;
    }

    #region SudokuRow

    /// <inheritdoc cref="IsSolved"/>
    public static bool IsSolved(this SudokuRow region)
        => ((SudokuRegion)region).IsSolved();

    /// <inheritdoc cref="HasSolution"/>
    public static bool HasSolution(this SudokuRow region)
        => ((SudokuRegion)region).HasSolution();

    /// <inheritdoc cref="MayContain"/>
    public static bool MayContain(this SudokuRow region, int value)
        => ((SudokuRegion)region).MayContain(value);

    /// <inheritdoc cref="Contains"/>
    public static bool Contains(this SudokuRow region, int value)
        => ((SudokuRegion)region).Contains(value);

    #endregion

    #region SudokuColumn

    /// <inheritdoc cref="IsSolved"/>
    public static bool IsSolved(this SudokuColumn region)
        => ((SudokuRegion)region).IsSolved();

    /// <inheritdoc cref="HasSolution"/>
    public static bool HasSolution(this SudokuColumn region)
        => ((SudokuRegion)region).HasSolution();

    /// <inheritdoc cref="MayContain"/>
    public static bool MayContain(this SudokuColumn region, int value)
        => ((SudokuRegion)region).MayContain(value);

    /// <inheritdoc cref="Contains"/>
    public static bool Contains(this SudokuColumn region, int value)
        => ((SudokuRegion)region).Contains(value);

    #endregion

    #region SudokuSquare

    /// <inheritdoc cref="IsSolved"/>
    public static bool IsSolved(this SudokuSquare region)
        => ((SudokuRegion)region).IsSolved();

    /// <inheritdoc cref="HasSolution"/>
    public static bool HasSolution(this SudokuSquare region)
        => ((SudokuRegion)region).HasSolution();

    /// <inheritdoc cref="MayContain"/>
    public static bool MayContain(this SudokuSquare region, int value)
        => ((SudokuRegion)region).MayContain(value);

    /// <inheritdoc cref="Contains"/>
    public static bool Contains(this SudokuSquare region, int value)
        => ((SudokuRegion)region).Contains(value);

    #endregion
}
