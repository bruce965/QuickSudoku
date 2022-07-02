using QuickSudoku.Abstractions;

namespace QuickSudoku.Extensions;

public static class RegionExtensions
{
    public static bool IsSolved(this IRegion region)
        => region.Cells.All(c => c.IsSolved());

    public static bool HasSolution(this IRegion region)
        => region.Cells.All(c => c.HasSolution());

    public static bool MayContain(this IRegion region, object value)
        => region.Cells.Any(cell => cell.MayContain(value));

    public static bool Contains(this IRegion region, object value)
        => region.Cells.Any(cell => cell.Contains(value));
}
