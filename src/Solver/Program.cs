// SPDX-FileCopyrightText: Copyright 2025 Fabio Iotti
// SPDX-License-Identifier: AGPL-3.0-only

using QuickSudoku.Extensions;
using QuickSudoku.Generators;
using QuickSudoku.Solvers;
using QuickSudoku.Sudoku;
using System.Diagnostics;
using System.Reflection;

if (args.Length < 1)
{
    PrintHelp();

    Environment.Exit(5);
    return;
}

SudokuPuzzle puzzle;
SudokuSolutionLog log = new();
Stopwatch stopwatch = new();
TimeSpan totalTime = TimeSpan.Zero;

if (args[0] is "--generate")
{
    long timerStart = stopwatch.ElapsedTicks;
    stopwatch.Start();

    puzzle = SudokuGenerator.Generate(new()
    {
        Symmetry = SudokuSymmetry.PreferFull,
        LimitStrategy = SudokuSolutionStrategy.Guess - 1,
    });

    stopwatch.Stop();
    long timerEnd = stopwatch.ElapsedTicks;

    TimeSpan generationDuration = TimeSpan.FromTicks(timerEnd - timerStart);
    totalTime += generationDuration;

    Console.WriteLine();
    Console.WriteLine("== Generated ==");
    PrintPuzzleCompact(puzzle);
    Console.WriteLine();
    Console.WriteLine("Generation time: {0:0.000}ms", totalTime.TotalMilliseconds);

    totalTime = TimeSpan.Zero;
}
else if (args[0] is "--in")
{
    using Stream stdin = Console.OpenStandardInput();
    using StreamReader reader = new(stdin);

    string scheme = await reader.ReadToEndAsync();
    puzzle = SudokuPuzzle.FromScheme(scheme);
}
else
{
    string scheme = args[0];
    puzzle = SudokuPuzzle.FromScheme(scheme);
}

Console.WriteLine();
Console.WriteLine("== Puzzle ==");
PrintPuzzle(puzzle, null);

int stepIndex = 0;
IEnumerator<SudokuSolutionStep> solutionSteps = SudokuSolver.StepByStep(puzzle).GetEnumerator();
while (true)
{
    SudokuPuzzle previous = puzzle.Clone();

    long timerStart = stopwatch.ElapsedTicks;
    stopwatch.Start();

    bool hasNextStep = solutionSteps.MoveNext();

    stopwatch.Stop();
    long timerEnd = stopwatch.ElapsedTicks;

    if (!hasNextStep)
        break;

    TimeSpan stepDuration = TimeSpan.FromTicks(timerEnd - timerStart);
    totalTime += stepDuration;

    SudokuSolutionStep step = solutionSteps.Current;
    log.Push(step);

    Console.WriteLine();
    Console.WriteLine("== Step {0} ==", ++stepIndex);
    Console.WriteLine("Strategy: {0} (difficulty +{1})", step.Strategy, step.Difficulty);
    Console.WriteLine("Puzzle difficulty: {0}+", log.Difficulty);
    Console.WriteLine("Time: {0:0.000}ms ({1:0.000}ms total)", stepDuration.TotalMilliseconds, totalTime.TotalMilliseconds);
    Console.WriteLine();
    PrintPuzzle(puzzle, previous);
}

Console.WriteLine();
Console.WriteLine("== Result ==");
Console.WriteLine("Puzzle solved? {0}", puzzle.IsSolved() ? "yes" : "no");
Console.WriteLine("Puzzle difficulty: {0}{1}", log.Difficulty, puzzle.IsSolved() ? "" : "+");
Console.WriteLine("Total time: {0:0.000}ms", totalTime.TotalMilliseconds);
Console.WriteLine();
Console.WriteLine("Strategies:");
foreach ((SudokuSolutionStrategy strategy, int count) in log.AdoptedStrategies)
{
    Console.WriteLine("* {0} applied {1} time{2}", strategy, count, count > 1 ? "s" : "");
}
if (!log.AdoptedStrategies.Any())
    Console.WriteLine("  none");


void PrintHelp()
{
    Process process = Process.GetCurrentProcess();
    Assembly assembly = typeof(Program).Assembly;

    AssemblyProductAttribute? product = assembly.GetCustomAttribute<AssemblyProductAttribute>();
    AssemblyCopyrightAttribute? copyright = assembly.GetCustomAttribute<AssemblyCopyrightAttribute>();
    AssemblyInformationalVersionAttribute? version = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();

    Console.Write(string.Join(Environment.NewLine, new[]
    {
        $"QuickSudoku {product?.Product} {version?.InformationalVersion}",
        copyright?.Copyright,
        "",
        "Usage:",
        $"  {process.ProcessName} --generate",
        $"  cat \"(puzzle scheme)\" | {process.ProcessName} --in",
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

void PrintPuzzleCompact(SudokuPuzzle puzzle)
{
    for (int y = 0; y < 9; y++)
    {
        if (y > 0 && (y % 3) == 0)
            Console.WriteLine();

        for (int x = 0; x < 9; x++)
        {
            if (x > 0 && (x % 3) == 0)
                Console.Write(" ");

            Console.Write("{0}", puzzle[x, y].Value?.ToString() ?? ".");
        }

        Console.WriteLine();
    }
}

void PrintPuzzle(SudokuPuzzle puzzle, SudokuPuzzle? previous)
{
    for (int y = 0; y < 9; y++)
    {
        if (y > 0 && (y % 3) == 0)
            Console.WriteLine(" -----------|-------------|-----------");
        else if (y > 0)
            Console.WriteLine("            |             |");

        for (int vX = 0; vX < 3; vX++)
        {
            if (vX != 0)
                Console.WriteLine();

            for (int x = 0; x < 9; x++)
            {
                if (x > 0 && (x % 3) == 0)
                    Console.Write(" | ");
                else if (x > 0)
                    Console.Write(" ");

                for (int vY = 1; vY <= 3; vY++)
                {
                    int? val = puzzle[x, y].Value;
                    if (val is not null && (previous is null || previous[x, y].Value == val))
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
                        int v = vX * 3 + vY;
                        bool isCandidate = puzzle[x, y].CandidateValues.Contains(v);

                        if (val is not null)
                            Console.ForegroundColor = ConsoleColor.Green;

                        if (isCandidate)
                        {
                            Console.Write(v);
                        }
                        else if (previous?[x, y].CandidateValues.Contains(v) ?? false)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("x");
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.Write(".");
                        }

                        if (val is not null)
                            Console.ResetColor();
                    }
                }
            }
        }

        Console.WriteLine();
    }
}
