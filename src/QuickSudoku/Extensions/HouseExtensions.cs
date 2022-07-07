using QuickSudoku.Abstractions;

namespace QuickSudoku.Extensions;

public static class RegionExtensions
{
    /// <summary>
    /// Check whether all cells in a region are solved.
    /// </summary>
    /// <param name="region"></param>
    /// <returns></returns>
    public static bool IsSolved(this IRegion region)
        => region.Cells.All(c => c.IsSolved());

    /// <summary>
    /// Check whether a solution may exist for all cells in a region.
    /// </summary>
    /// <param name="region"></param>
    /// <returns></returns>
    public static bool HasSolution(this IRegion region)
        => region.Cells.All(c => c.HasSolution());

    /// <summary>
    /// Check whether a region contains a certain value in a solved cell.
    /// </summary>
    /// <param name="region"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool Contains(this IRegion region, object value)
        => region.Cells.Any(cell => cell.Contains(value));
}
