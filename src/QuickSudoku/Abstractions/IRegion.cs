namespace QuickSudoku.Abstractions;

/// <summary>
/// Group of cells on the puzzle board.
/// 
/// Each value can only appear once in each region.
/// </summary>
public interface IRegion : IEquatable<IRegion>
{
    /// <summary>
    /// Puzzle this region is part of.
    /// </summary>
    IPuzzle Puzzle { get; }

    /// <summary>
    /// Values legal in this region.
    /// </summary>
    IEnumerable<object> LegalValues { get; }

    /// <summary>
    /// Cells in this region.
    /// </summary>
    IEnumerable<ICell> Cells { get; }
}
