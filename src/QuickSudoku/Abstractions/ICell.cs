namespace QuickSudoku.Abstractions;

/// <summary>
/// Cell in the puzzle.
/// </summary>
public interface ICell : IEquatable<ICell>
{
    /// <summary>
    /// Puzzle board this cell is part of.
    /// </summary>
    IPuzzle Puzzle { get; }

    /// <summary>
    /// Regions this cell is part of.
    /// </summary>
    IEnumerable<IRegion> Regions { get; }

    /// <summary>
    /// Values legal on this cell.
    /// </summary>
    IEnumerable<object> LegalValues { get; }

    /// <summary>
    /// Candidate values in this cell.
    /// </summary>
    IValuesCollection<object> CandidateValues { get; }
}
