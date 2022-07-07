using QuickSudoku.Abstractions;

namespace QuickSudoku.Sudoku;

public struct SudokuRegionsIntersection : IRegionsIntersection
{
    /// <inheritdoc cref="IRegionsIntersection.First"/>
    public SudokuRegion First { get; }
    
    /// <inheritdoc cref="IRegionsIntersection.Second"/>
    public SudokuRegion Second { get; }

    ///// <inheritdoc cref="IRegionsIntersection.Cells"/>
    //public SudokuRegion Cells { get; }  // TODO

    internal SudokuRegionsIntersection(SudokuRegion first, SudokuRegion second)
    {
        First = first;
        Second = second;
    }

    #region IRegionsIntersection

    IRegion IRegionsIntersection.First => First;

    IRegion IRegionsIntersection.Second => Second;

    //IEnumerable<ICell> IRegionsIntersection.Cells => Cells;  // TODO

    #endregion
}
