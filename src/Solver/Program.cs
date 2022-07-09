using QuickSudoku.Extensions;
using QuickSudoku.Solvers;
using QuickSudoku.Sudoku;
using System.Diagnostics;

if (args.Length < 1)
{
    PrintHelp();
    Environment.Exit(5);
}

string scheme;
if (args[0] == "--in")
{
    using var stdin = Console.OpenStandardInput();
    using var reader = new StreamReader(stdin);
    scheme = await reader.ReadToEndAsync();
}
else
{
    scheme = args[0];
}

var puzzle = SudokuPuzzle.FromScheme(scheme);

Console.WriteLine();
Console.WriteLine("== Puzzle ==");
PrintPuzzle(puzzle, null);

var stepIndex = 0;
var log = new SudokuSolutionLog();
var stopwatch = new Stopwatch();
var totalTime = TimeSpan.Zero;
while (!log.Solved)
{
    var previous = puzzle.Clone();

    var timerStart = stopwatch.ElapsedTicks;
    stopwatch.Start();
    
    var stepStrategy = SudokuSolver.SolveStep(puzzle);

    stopwatch.Stop();
    var timerEnd = stopwatch.ElapsedTicks;

    var stepDuration = TimeSpan.FromTicks(timerEnd - timerStart);
    totalTime += stepDuration;

    Console.WriteLine();
    Console.WriteLine("== Step {0} ==", ++stepIndex);

    if (stepStrategy == null)
        break;

    var strategyFirstUse = !log.AdoptedStrategies.ContainsKey(stepStrategy.Value);
    var stepDifficulty = SudokuSolver.GetStrategyDifficulty(stepStrategy.Value, strategyFirstUse);
    log.Push(stepStrategy.Value, stepDifficulty, 1);

    Console.WriteLine("Strategy: {0} (difficulty +{1})", stepStrategy, stepDifficulty);
    Console.WriteLine("Puzzle difficulty: {0}+", log.Difficulty);
    Console.WriteLine("Time: {0:0.000}ms ({1:0.000}ms total)", stepDuration.TotalMilliseconds, totalTime.TotalMilliseconds);
    Console.WriteLine();
    PrintPuzzle(puzzle, previous);
}

Console.WriteLine();
Console.WriteLine("== Result ==", ++stepIndex);
Console.WriteLine("Puzzle solved? {0}", log.Solved ? "yes" : "no");
Console.WriteLine("Puzzle difficulty: {0}{1}", log.Difficulty, log.Solved ? "" : "+");
Console.WriteLine("Total time: {0:0.000}ms", totalTime.TotalMilliseconds);
Console.WriteLine();
Console.WriteLine("Strategies:");
foreach (var strategy in log.AdoptedStrategies)
{
    Console.WriteLine("* {0} applied {1} time{2}", strategy.Key, strategy.Value, strategy.Value > 1 ? "s" : "");
}

void PrintHelp()
{
    var process = Process.GetCurrentProcess();
    var assemlby = typeof(Program).Assembly;
    var versionInfo = FileVersionInfo.GetVersionInfo(assemlby.Location);

    Console.Write(string.Join(Environment.NewLine, new[]
    {
        $"QuickSudoku {versionInfo.ProductVersion}",
        $"{versionInfo.LegalCopyright}",
        "",
        "Usage:",
        $"  {process.ProcessName} --in | cat \"(puzzle scheme)\"",
        $"  {process.ProcessName} \"(puzzle scheme)\"",
        "",
        "Puzzle scheme format:",
        "  53. .7. ...",
        "  6.. 195 ...",
        "  .98 ... .6.",
        "",
        "  8.. .6. ..3",
        "  4.. 8.3 ..1",
        "  7.. .2. ..6",
        "",
        "  .6. ... 28.",
        "  ... 419 ..5",
        "  ... .8. .79",
        "",
    }));
}

void PrintPuzzle(SudokuPuzzle puzzle, SudokuPuzzle? previous)
{
    for (var y = 0; y < 9; y++)
    {
        if (y > 0 && (y % 3) == 0)
            Console.WriteLine(" -----------|-------------|-----------");
        else if (y > 0)
            Console.WriteLine("            |             |");

        for (var vX = 0; vX < 3; vX++)
        {
            if (vX != 0)
                Console.WriteLine();

            for (var x = 0; x < 9; x++)
            {
                if (x > 0 && (x % 3) == 0)
                    Console.Write(" | ");
                else if (x > 0)
                    Console.Write(" ");

                for (var vY = 1; vY <= 3; vY++)
                {
                    var val = puzzle[x, y].Value;
                    if (val != null && (previous is null || previous[x, y].Value == val))
                    {
                        if (vX == 1 && vY == 2)
                        {
                            Console.Write(val);
                        }
                        else
                        {
                            Console.Write(" ");
                        }
                    }
                    else
                    {
                        var v = (vX * 3 + vY);
                        var isCandidate = puzzle[x, y].CandidateValues.Contains(v);

                        if (val != null)
                            Console.ForegroundColor = ConsoleColor.Green;

                        if (isCandidate)
                        {
                            Console.Write(v);
                        }
                        else if (previous?[x, y].CandidateValues.Contains(v) == true)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("x");
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.Write(".");
                        }

                        if (val != null)
                            Console.ResetColor();
                    }
                }
            }
        }

        Console.WriteLine();
    }
}
