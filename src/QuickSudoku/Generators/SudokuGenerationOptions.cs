// SPDX-FileCopyrightText: Copyright 2025 Fabio Iotti
// SPDX-License-Identifier: AGPL-3.0-only

using System.Collections.Immutable;
using QuickSudoku.Solvers;

namespace QuickSudoku.Generators;

/// <summary>
/// Generation options, used to generate puzzles.
/// </summary>
public record class SudokuGenerationOptions
{
    internal static readonly ICollection<SudokuSolutionStrategy> AllStrategies
        = Enum.GetValues<SudokuSolutionStrategy>();

    /// <summary>
    /// Immutable copy of the default options.
    /// </summary>
    public static SudokuGenerationOptions Default { get; } = new SudokuGenerationOptions
    {
        AllowedStrategies = AllStrategies,
        RequiredStrategies = Array.Empty<SudokuSolutionStrategy>(),
    };

    /// <summary>
    /// Source of random data used to generate the puzzle board.
    /// </summary>
    public Random Random { get; init; } = Random.Shared;

    /// <summary>
    /// Level of symmetry in the generated puzzle board.
    /// </summary>
    public SudokuSymmetry Symmetry { get; init; } = SudokuSymmetry.None;

    /// <summary>
    /// Generate puzzles below this difficulty threshold (exclusive).
    /// </summary>
    public int? MaxDifficulty { get; init; }

    /// <summary>
    /// Limit to puzzles which can be solved with strategies up to this level (inclusive),
    /// no puzzles with higher difficuly strategies will be generated.
    /// </summary>
    public SudokuSolutionStrategy LimitStrategy
    {
        get => AllowedStrategies.Max();
        set
        {
            foreach (var strategy in AllStrategies)
                if (strategy > value)
                    AllowedStrategies.Remove(strategy);
        }
    }

    /// <summary>
    /// Allow usage of certain solution strategies in generated puzzles.
    /// </summary>
    public ICollection<SudokuSolutionStrategy> AllowedStrategies { get; init; }
        = new HashSet<SudokuSolutionStrategy>(AllStrategies);

    /// <summary>
    /// Generate puzzles that requires usage of certain solution strategies.
    /// </summary>
    public ICollection<SudokuSolutionStrategy> RequiredStrategies { get; init; }
        = new HashSet<SudokuSolutionStrategy>();
}
