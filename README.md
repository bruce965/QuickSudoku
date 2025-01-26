QuickSudoku
===========

Sudoku puzzle solver, grader, and generator.

This project is an incomplete work-in-progress.


## CLI Usage

```plaintext
QuickSudoku Solver 0.2.1-alpha
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
SudokuPuzzle puzzle = SudokuGenerator.Generate(new()
{
    Symmetry = SudokuSymmetry.Full,
    LimitStrategy = SudokuSolutionStrategy.HiddenPair,
});

for (int y = 0; y < 9; y++)
{
    for (int x = 0; x < 9; x++)
    {
        Console.Write("{0}", puzzle[x, y].Value?.ToString() ?? " ");
    }

    Console.WriteLine();
}
```

To solve and grade a puzzle:

```csharp
SudokuPuzzle puzzle = SudokuPuzzle.FromScheme(@"
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

ISudokuSolutionLog solution = SudokuSolver.Solve(puzzle);

Console.WriteLine("Difficulty: {0}", solution.Difficulty);
```


## License

Copyright (c) 2024 Fabio Iotti

This program is free software: you can redistribute it and/or modify it under
the terms of the GNU Affero General Public License as published by the Free
Software Foundation, version 3 of the License.

This program is distributed in the hope that it will be useful, but WITHOUT ANY
WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A
PARTICULAR PURPOSE. See the GNU Affero General Public License for more details.

You should have received a copy of the GNU Affero General Public License along
with this program. If not, see https://www.gnu.org/licenses/.

Some files may be licensed under different terms, refer to the header at the
beginning of each individual file for file-specific licenses.
