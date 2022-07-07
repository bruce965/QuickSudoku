namespace QuickSudoku.Abstractions;

/// <summary>
/// Intersection between two regions of the puzzle board.
/// </summary>
public interface IRegionsIntersection
{
    /// <summary>
    /// First region.
    /// </summary>
    IRegion First { get; }

    /// <summary>
    /// Second region.
    /// </summary>
    IRegion Second { get; }

    /// <summary>
    /// Cells in common between the two regions.
    /// </summary>
    IEnumerable<ICell> Cells => First.Cells.Intersect(Second.Cells);
}
