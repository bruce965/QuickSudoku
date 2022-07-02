namespace QuickSudoku.Solvers;

public interface ISudokuSolutionLog
{
    public int Difficulty { get; }
    public bool Solved { get; }
    IReadOnlyDictionary<SudokuSolutionStrategy, int> AdoptedStrategies { get; }
}

public class SudokuSolutionLog : ISudokuSolutionLog
{
    readonly Dictionary<SudokuSolutionStrategy, int> _adoptedStrategies = new();

    public int Difficulty { get; private set; }

    public bool Solved { get; set; }

    public IReadOnlyDictionary<SudokuSolutionStrategy, int> AdoptedStrategies => _adoptedStrategies;

    public SudokuSolutionLog() { }

    public void Push(SudokuSolutionStrategy strategy, int difficulty, int count = 1)
    {
        _adoptedStrategies[strategy] += count;
        Difficulty += difficulty * count;
    }
}
