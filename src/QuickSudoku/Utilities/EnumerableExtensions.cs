namespace QuickSudoku.Utilities;

static class EnumerableExtensions
{
    /// <summary>
    /// Consume elements in a sequence.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of source.</typeparam>
    /// <param name="source">A sequence of values to consume.</param>
    public static void Consume<TSource>(this IEnumerable<TSource> source)
    {
        foreach (var _ in source) { }
    }

    /// <summary>
    /// Shift a sequence by a certain amount of elements, moving <paramref name="count"/>
    /// values from the start of the sequence to the end.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of source.</typeparam>
    /// <param name="source">A sequence of values.</param>
    /// <param name="count">How many values to shift by.</param>
    /// <returns>Shifted sequence of values.</returns>
    public static IEnumerable<TSource> RingShift<TSource>(this IEnumerable<TSource> source, int count)
    {
        if (count < 0)
            throw new NotImplementedException("Negative count support not implemented.");

        var hold = new TSource[count];

        var i = 0;
        foreach (var el in source)
        {
            if (i < count)
                hold[i++] = el;
            else
                yield return el;
        }

        var start = 0;
        var end = count;
        if (i < count)
        {
            start = count % i;
            end = i;
        }

        for (i = start; i < end; i++)
            yield return hold[i];

        for (i = 0; i < start; i++)
            yield return hold[i];
    }
}
