namespace QuickSudoku.Abstractions;

/// <summary>
/// Generic puzzle board.
/// </summary>
public interface IPuzzle : IHouse, ICloneable
{
    /// <summary>
    /// Houses on this puzzle board.
    /// </summary>
    IEnumerable<IHouse> Houses { get; }

    /// <inheritdoc cref="ICloneable.Clone"/>
    new IPuzzle Clone() => (IPuzzle)((ICloneable)this).Clone();
}
