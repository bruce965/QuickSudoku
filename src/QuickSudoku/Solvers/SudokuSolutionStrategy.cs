namespace QuickSudoku.Solvers;

public enum SudokuSolutionStrategy
{
    NakedSingle,
    HiddenSingle,
    NakedPair,
    HiddenPair,
    NakedTriple,
    HiddenTriple,
    NakedQuad,
    HiddenQuad,
    IntersectionRemoval,
    Guess,
}
