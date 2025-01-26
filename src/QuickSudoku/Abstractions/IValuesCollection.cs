// SPDX-FileCopyrightText: Copyright 2025 Fabio Iotti
// SPDX-License-Identifier: AGPL-3.0-only

namespace QuickSudoku.Abstractions;

public interface IValuesCollection<T> : IEnumerable<T>
{
    /// <summary>
    /// Adds a value to the collection.
    /// </summary>
    /// <param name="value">The value to be added to the collection.</param>
    void Add(T value);

    /// <summary>
    /// Determines whether the collection contains a specific value.
    /// </summary>
    /// <param name="value">The value to locate in the collection.</param>
    /// <returns><c>true</c> if item is found in the collection; otherwise, <c>false</c>.</returns>
    bool Contains(T value);

    /// <summary>
    /// Removes a value from the collection.
    /// </summary>
    /// <param name="value">The value to be removed from the collection.</param>
    void Remove(T value);

    /// <summary>
    /// Adds all legal values to this collection.
    /// </summary>
    void Reset();
}
