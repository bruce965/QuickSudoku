﻿// SPDX-FileCopyrightText: Copyright 2025 Fabio Iotti
// SPDX-License-Identifier: AGPL-3.0-only

namespace QuickSudoku.Utilities;

static class EnumerableExtensions
{
    /// <summary>
    /// Consume elements in a sequence.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
    /// <param name="source">A sequence of values to consume.</param>
    public static void Consume<TSource>(this IEnumerable<TSource> source)
    {
        foreach (TSource _ in source) { }
    }

    /// <summary>
    /// Shift a sequence by a certain amount of elements, moving <paramref name="count"/>
    /// values from the start of the sequence to the end.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
    /// <param name="source">A sequence of values.</param>
    /// <param name="count">How many values to shift by.</param>
    /// <returns>Shifted sequence of values.</returns>
    public static IEnumerable<TSource> RingShift<TSource>(this IEnumerable<TSource> source, int count)
    {
        if (count < 0)
            throw new NotImplementedException("Negative count support not implemented.");

        TSource[] hold = new TSource[count];

        int i = 0;
        foreach (TSource? el in source)
        {
            if (i < count)
                hold[i++] = el;
            else
                yield return el;
        }

        int start = 0;
        int end = count;
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

    /// <summary>
    /// Randomly shuffle a sequence.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
    /// <param name="source">A sequence of values.</param>
    /// <param name="random">Random source.</param>
    /// <returns>Shuffled sequence of values.</returns>
    public static IEnumerable<TSource> Shuffle<TSource>(this IEnumerable<TSource> source, Random random)
    {
        List<TSource> els = source.ToList();

        // https://en.wikipedia.org/w/index.php?title=Fisher–Yates_shuffle&oldid=1082693296#The_modern_algorithm
        for (int i = 0; i < els.Count - 1; i++)
        {
            int rand = random.Next(i, els.Count);
            (els[rand], els[i]) = (els[i], els[rand]);
        }

        return els;
    }
}
