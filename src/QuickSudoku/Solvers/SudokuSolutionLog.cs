// SPDX-FileCopyrightText: Copyright 2025 Fabio Iotti
// SPDX-License-Identifier: AGPL-3.0-only

namespace QuickSudoku.Solvers;

public interface ISudokuSolutionLog
{
    public int Difficulty { get; }

    public bool Solved { get; }

    public bool? HasMultipleSolutions { get; }

    IReadOnlyDictionary<SudokuSolutionStrategy, int> AdoptedStrategies { get; }
}

public class SudokuSolutionLog : ISudokuSolutionLog
{
    readonly Dictionary<SudokuSolutionStrategy, int> _adoptedStrategies = new();

    public int Difficulty { get; private set; }

    public bool Solved { get; set; }

    public bool? HasMultipleSolutions { get; set; }

    public IReadOnlyDictionary<SudokuSolutionStrategy, int> AdoptedStrategies => _adoptedStrategies;

    public SudokuSolutionLog() { }

    public void Push(SudokuSolutionStep step, int count = 1)
    {
        if (!_adoptedStrategies.TryGetValue(step.Strategy, out int previousCount))
            previousCount = 0;

        _adoptedStrategies[step.Strategy] = previousCount + count;
        Difficulty += step.Difficulty * count;
    }
}
