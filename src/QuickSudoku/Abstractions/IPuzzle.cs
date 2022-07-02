namespace QuickSudoku.Abstractions;

/// <summary>
/// Generic puzzle board.
/// </summary>
public interface IPuzzle : IRegion, ICloneable
{
    /// <summary>
    /// Regions on this puzzle board.
    /// </summary>
    IEnumerable<IRegion> Regions { get; }

    /// <inheritdoc cref="ICloneable.Clone"/>
    new IPuzzle Clone() => (IPuzzle)((ICloneable)this).Clone();
}
