namespace QuickSudoku.Abstractions;

/// <summary>
/// Group of cells on the puzzle board.
/// 
/// Each value can only appear once in each region.
/// </summary>
public interface IRegion : IEquatable<IRegion>
{
    record Intersection(IRegion First, IRegion Second) : IRegionsIntersection;

    /// <summary>
    /// Puzzle this region is part of.
    /// </summary>
    IPuzzle Puzzle { get; }

    /// <summary>
    /// Values legal in this region.
    /// </summary>
    IEnumerable<object> LegalValues => Puzzle.LegalValues;

    /// <summary>
    /// Cells in this region.
    /// </summary>
    IEnumerable<ICell> Cells { get; }

    /// <summary>
    /// Regions in the puzzle intersecting with this region.
    /// </summary>
    IEnumerable<IRegionsIntersection> IntersectingRegions
        => Puzzle.Regions
            .Select(r => new Intersection(this, r))
            .Where(i => i.First != i.Second && ((IRegionsIntersection)i).Cells.Any());
}
