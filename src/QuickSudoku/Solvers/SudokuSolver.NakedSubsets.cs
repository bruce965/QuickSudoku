// SPDX-FileCopyrightText: Copyright 2025 Fabio Iotti
// SPDX-License-Identifier: AGPL-3.0-only

using QuickSudoku.Sudoku;

namespace QuickSudoku.Solvers;

partial class SudokuSolver
{
    /// <summary>
    /// Solve naked subsets of a certain size in a puzzle.
    /// </summary>
    /// <param name="puzzle">Puzzle.</param>
    /// <param name="subsetSize">Size of the subset.</param>
    /// <param name="maxCount">How many naked subsets of the defined size to be solved before stopping.</param>
    /// <returns>Number of naked subsets of the defined size solved.</returns>
    public static int SolveNakedSubsets(SudokuPuzzle puzzle, int subsetSize, int maxCount = -1)
    {
        int nakedSubsetsFound = 0;

        if (maxCount == 0)
            return 0;

        // A naked subset of size N occurs when N digits are the sole
        // candidates in N cells of a single house.
        //
        // When a naked subset is found, those digits can be eliminated
        // from candidates in all other cells in the same house.

        foreach (var house in puzzle.Houses)
        {
            foreach (var cell in house.Cells)
            {
                // if a cell in this region has the correct number number of candidates,
                // check if a naked subset is found for these candidates
                if (cell.CandidateValues.Count == subsetSize)
                {
                    // find other cells that only contain these candidates
                    var subsetCells = house.Cells.Where(c => !((IEnumerable<int>)c.CandidateValues).Except(cell.CandidateValues).Any());

                    // if the correct amount of cells is found, a naked subset is found
                    // but it is only useful if another cell in the same house contains one of the candidates
                    if (subsetCells.Count() == subsetSize)
                    {
                        var nakedSubsetFound = false;

                        var otherCells = house.Cells.Except(subsetCells);
                        foreach (var cell2 in otherCells)
                        {
                            foreach (var candidate in cell.CandidateValues)
                            {
                                if (!cell2.CandidateValues.Contains(candidate))
                                    continue;

                                nakedSubsetFound = true;
                                cell2.CandidateValues.Remove(candidate);
                            }
                        }

                        if (nakedSubsetFound)
                        {
                            nakedSubsetsFound++;

                            if (maxCount != -1 && nakedSubsetsFound >= maxCount)
                                return nakedSubsetsFound;
                        }
                    }
                }
            }
        }

        return nakedSubsetsFound;
    }

    /// <summary>
    /// Solve naked pairs in a puzzle.
    ///
    /// This method should be called only after calling another method to remove invalid candidates.
    /// </summary>
    /// <param name="puzzle">Puzzle</param>
    /// <param name="maxCount">How many naked pairs to be solved before stopping.</param>
    /// <returns>Number of naked pairs solved.</returns>
    public static int SolveNakedPairs(SudokuPuzzle puzzle, int maxCount = -1)
        => SolveNakedSubsets(puzzle, 2, maxCount);

    /// <summary>
    /// Solve naked triples in a puzzle.
    ///
    /// This method should be called only after calling another method to remove invalid candidates.
    /// </summary>
    /// <param name="puzzle">Puzzle</param>
    /// <param name="maxCount">How many naked triples to be solved before stopping.</param>
    /// <returns>Number of naked triples solved.</returns>
    public static int SolveNakedTriples(SudokuPuzzle puzzle, int maxCount = -1)
        => SolveNakedSubsets(puzzle, 3, maxCount);

    /// <summary>
    /// Solve naked quads in a puzzle.
    ///
    /// This method should be called only after calling another method to remove invalid candidates.
    /// </summary>
    /// <param name="puzzle">Puzzle</param>
    /// <param name="maxCount">How many naked quads to be solved before stopping.</param>
    /// <returns>Number of naked quads solved.</returns>
    public static int SolveNakedQuads(SudokuPuzzle puzzle, int maxCount = -1)
        => SolveNakedSubsets(puzzle, 4, maxCount);
}
