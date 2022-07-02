using QuickSudoku.Abstractions;

namespace QuickSudoku.Extensions;

public static class CellExtensions
{
    public static bool IsSolved(this ICell cell)
        => cell.CandidateValues.Take(2).Count() == 1;

    public static bool HasSolution(this ICell cell)
        => cell.CandidateValues.Any();

    public static bool MayContain(this ICell cell, object value)
        => cell.CandidateValues.Contains(value);

    public static bool Contains(this ICell cell, object value)
        => cell.LegalValues.All(v => v == value || !cell.CandidateValues.Contains(v));
}
