namespace QuickSudoku.Solvers;

public record class SudokuSolutionOptions
{
    public static SudokuSolutionOptions Default { get; } = new()
    {
        //StopIfMultipleSolutions = false,
        StopAtDifficulty = null,
        ForbiddenStrategies = Array.Empty<SudokuSolutionStrategy>(),
    };

    ///// <summary>
    ///// Stop if multiple solutions are found.
    ///// </summary>
    //public bool StopIfMultipleSolutions { get; init; }

    /// <summary>
    /// Stop looking for a solution if the difficulty gets above this threshold.
    /// </summary>
    public int? StopAtDifficulty { get; init; }

    /// <summary>
    /// Forbid usage of certain solution strategies.
    /// </summary>
    public ICollection<SudokuSolutionStrategy> ForbiddenStrategies { get; init; } = new HashSet<SudokuSolutionStrategy>();
}
