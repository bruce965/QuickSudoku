QuickSudoku
===========

Sudoku puzzle solver, grader, and generator.

This project is an incomplete work-in-progress.


## CLI Usage

```plaintext
QuickSudoku Solver 0.1.0-alpha
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

To generate a step-by-step solution:

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

To generate a solved puzzle:

```csharp
var puzzle = SudokuGenerator.GenerateSolved();

for (var y = 0; y < 9; y++)
{
    for (var x = 0; x < 9; x++)
    {
        Console.Write("{0}", puzzle[x, y].Value ?? ' ');
    }

    Console.WriteLine();
}
```

To grade a puzzle:

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

To generate a puzzle:

```csharp
//TODO
```


## License

Source code in this repository is licensed under the [MIT license](LICENSE).
