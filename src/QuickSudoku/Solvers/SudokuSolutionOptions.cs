// SPDX-FileCopyrightText: Copyright 2025 Fabio Iotti
// SPDX-License-Identifier: AGPL-3.0-only

namespace QuickSudoku.Solvers;

/// <summary>
/// Solution options, used to find solutions to puzzles.
/// </summary>
public record class SudokuSolutionOptions
{
    /// <summary>
    /// Immutable copy of the default options.
    /// </summary>
    public static SudokuSolutionOptions Default { get; } = new()
    {
        ForbiddenStrategies = Array.Empty<SudokuSolutionStrategy>(),
    };

    /// <summary>
    /// Stop looking for a solution if the puzzle is deemed
    /// to have multiple valid solutions.
    /// </summary>
    public bool EnsureSingleSolution { get; init; }  // TODO

    /// <summary>
    /// Stop looking for a solution if the difficulty gets above this threshold.
    /// </summary>
    public int? StopAtDifficulty { get; init; }

    /// <summary>
    /// Forbid usage of certain solution strategies.
    /// </summary>
    public ICollection<SudokuSolutionStrategy> ForbiddenStrategies { get; init; }
        = new HashSet<SudokuSolutionStrategy>();
}
