using QuickSudoku.Sudoku;
using QuickSudoku.Sudoku.Extensions;
using QuickSudoku.Utilities;

namespace QuickSudoku.Solvers;

public static partial class SudokuSolver
{
    const int NakedSingleDifficulty = 1;
    const int FirstGuessDifficulty = 300;
    const int GuessDifficulty = 500;

    /// <summary>
    /// Known solution strategies ordered by increasing difficulty, except for naked singles and guessing.
    /// </summary>
    static readonly IReadOnlyCollection<(SudokuSolutionStrategy Strategy, int FirstUseDifficulty, int Difficulty, Func<SudokuPuzzle, int> Solve)> Strategies
        = new (SudokuSolutionStrategy, int, int, Func<SudokuPuzzle, int>)[]
    {
        (SudokuSolutionStrategy.HiddenSingle,         1,   1, puzzle => SolveHiddenSingles(puzzle, 1)),
        (SudokuSolutionStrategy.NakedPair,            5,   3, puzzle => SolveNakedPairs(puzzle, 1)),
        (SudokuSolutionStrategy.HiddenPair,           5,   3, puzzle => SolveHiddenPairs(puzzle, 1)),
        (SudokuSolutionStrategy.NakedTriple,         15,  10, puzzle => SolveHiddenPairs(puzzle, 1)),
        (SudokuSolutionStrategy.HiddenTriple,        20,  12, puzzle => SolveHiddenTriples(puzzle, 1)),
        (SudokuSolutionStrategy.NakedQuad,           30,  20, puzzle => SolveNakedQuads(puzzle, 1)),
        (SudokuSolutionStrategy.HiddenQuad,          50,  30, puzzle => SolveHiddenQuads(puzzle, 1)),
        (SudokuSolutionStrategy.IntersectionRemoval, 12,   7, puzzle => SolveIntersectionRemovals(puzzle, 1).Intersections),
    };

    /// <summary>
    /// Solve a puzzle.
    /// </summary>
    /// <param name="puzzle">Puzzle.</param>
    /// <param name="options">Solution options.</param>
    /// <returns>Log of the strategies adopted to solve the puzzle.</returns>
    public static ISudokuSolutionLog Solve(SudokuPuzzle puzzle, SudokuSolutionOptions? options = null)
    {
        options ??= SudokuSolutionOptions.Default;

        var sudokuLog = new SudokuSolutionLog();
        SolveStepByStep(puzzle, sudokuLog, options, true).Consume();

        return sudokuLog;
    }

    /// <summary>
    /// Execute one step towards the solution of a puzzle.
    /// </summary>
    /// <param name="puzzle">Puzzle.</param>
    /// <param name="options">Solution options.</param>
    /// <returns>Steps towards the solution.</returns>
    public static IEnumerable<SudokuSolutionStep> StepByStep(SudokuPuzzle puzzle, SudokuSolutionOptions? options = null)
    {
        options ??= SudokuSolutionOptions.Default;

        return SolveStepByStep(puzzle, null, options, false);
    }

    static IEnumerable<SudokuSolutionStep> SolveStepByStep(
        SudokuPuzzle puzzle,
        SudokuSolutionLog? log,
        SudokuSolutionOptions options,
        bool groupNakedSingles)
    {
        SudokuPuzzle? solution = null;

        // use the log set from outside (won't be resettable);
        // if not available, create a new log (resettable)
        log ??= new();

        while (true)
        {
            log.Solved = puzzle.IsSolved();

            if (log.Solved)
                break;

            if (options.StopAtDifficulty.HasValue && log.Difficulty > options.StopAtDifficulty.Value)
                break;

            if (!puzzle.HasSolution())
                break;

            var step = SolveStep(
                puzzle,
                log,
                options,
                ref solution,
                groupNakedSingles ? -1 : 1,
                out var nakedSinglesFound);
            
            if (!step.HasValue)
            {
                // cannot be solved with configured strategies
                break;
            }

            var count = step.Value.Strategy == SudokuSolutionStrategy.NakedSingle ? nakedSinglesFound : 1;
            log.Push(step.Value, count);

            yield return step.Value;
        }
    }

    static SudokuSolutionStep? SolveStep(
        SudokuPuzzle puzzle,
        ISudokuSolutionLog log,
        SudokuSolutionOptions options,
        ref SudokuPuzzle? solution,
        int maxNakedSingles,
        out int nakedSinglesFound)
    {
        // solve naked singles
        if (options.ForbiddenStrategies.Contains(SudokuSolutionStrategy.NakedSingle))
        {
            nakedSinglesFound = 0;
        }
        else
        {
            nakedSinglesFound = SolveNakedSingles(puzzle, maxNakedSingles);
            if (nakedSinglesFound > 0)
                return new (SudokuSolutionStrategy.NakedSingle, NakedSingleDifficulty);
        }

        // solve with other strategies
        foreach (var strategy in Strategies)
        {
            if (options.ForbiddenStrategies.Contains(strategy.Strategy))
                continue;

            var foundCount = strategy.Solve(puzzle);
            if (foundCount > 0)
            {
                var isFirstUse = !log.AdoptedStrategies.ContainsKey(strategy.Strategy);
                return new (strategy.Strategy, isFirstUse ? strategy.FirstUseDifficulty : strategy.Difficulty);
            }
        }

        // if no other known strategy remains, solve by guessing
        if (!options.ForbiddenStrategies.Contains(SudokuSolutionStrategy.Guess))
        {
            if (solution is null)
            {
                solution = puzzle.Clone();

                var hasSolution = SolveRecursive(solution);
                if (!hasSolution)
                    return null;
            }

            var isFirstGuess = !log.AdoptedStrategies.ContainsKey(SudokuSolutionStrategy.Guess);
            SolveGuessing(puzzle, solution, 1);

            return new(SudokuSolutionStrategy.Guess, isFirstGuess ? FirstGuessDifficulty : GuessDifficulty);
        }

        return null;
    }
}
