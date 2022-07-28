QuickSudoku
===========

Sudoku puzzle solver, grader, and generator.

This project is an incomplete work-in-progress.


## CLI Usage

```plaintext
QuickSudoku Solver 0.2.0-alpha
Copyright (c) 2022 Fabio Iotti.

Usage:
  Solver --generate
  cat "(puzzle scheme)" | Solver --in
  Solver "(puzzle scheme)"

Puzzle scheme format:
  53. .7. ...
  6.. 195 ...
  .98 ... .6.

  8.. .6. ..3
  4.. 8.3 ..1
  7.. .2. ..6

  .6. ... 28.
  ... 419 ..5
  ... .8. .79
```

To generate a new random puzzle, grade it, and print the solution:

```bash
Solver --generate
```

To print the step-by-step solution for an existing puzzle:

```bash
# Bash (Unix shell)

Solver "
  53. .7. ...
  6.. 195 ...
  .98 ... .6.

  8.. .6. ..3
  4.. 8.3 ..1
  7.. .2. ..6

  .6. ... 28.
  ... 419 ..5
  ... .8. .79
"
```

```batch
:: Windows cmd.exe

(
  echo 53. .7. ...
  echo 6.. 195 ...
  echo .98 ... .6.

  echo 8.. .6. ..3
  echo 4.. 8.3 ..1
  echo 7.. .2. ..6

  echo .6. ... 28.
  echo ... 419 ..5
  echo ... .8. .79
) | Solver --in
```


## Library Usage

To generate a new easy puzzle:

```csharp
var puzzle = SudokuGenerator.Generate(new()
{
    Symmetry = SudokuSymmetry.Full,
    LimitStrategy = SudokuSolutionStrategy.HiddenPair,
});

for (var y = 0; y < 9; y++)
{
    for (var x = 0; x < 9; x++)
    {
        Console.Write("{0}", puzzle[x, y].Value?.ToString() ?? " ");
    }

    Console.WriteLine();
}
```

To solve and grade a puzzle:

```csharp
var puzzle = SudokuPuzzle.FromScheme(@"
    53. .7. ...
    6.. 195 ...
    .98 ... .6.

    8.. .6. ..3
    4.. 8.3 ..1
    7.. .2. ..6

    .6. ... 28.
    ... 419 ..5
    ... .8. .79
");

var solution = SudokuSolver.Solve(puzzle);

Console.WriteLine("Difficulty: {0}", solution.Difficulty);
```


## License

Source code in this repository is licensed under the [MIT license](LICENSE).
