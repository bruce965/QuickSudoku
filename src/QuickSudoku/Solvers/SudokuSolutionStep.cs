namespace QuickSudoku.Solvers;

/// <summary>
/// Step towards the solution.
/// </summary>
public struct SudokuSolutionStep
{
    /// <summary>
    /// Strategy adopted for this step.
    /// </summary>
    public SudokuSolutionStrategy Strategy { get; init; }

    /// <summary>
    /// Difficulty of this step.
    /// </summary>
    public int Difficulty { get; init; }

    internal SudokuSolutionStep(SudokuSolutionStrategy strategy, int difficulty)
    {
        Strategy = strategy;
        Difficulty = difficulty;
    }
}
