// SPDX-FileCopyrightText: Copyright 2025 Fabio Iotti
// SPDX-License-Identifier: AGPL-3.0-only

using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace QuickSudoku.Utilities;

static class EnumExtensions
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public readonly struct FlagIndices : IEnumerable<int>
    {
        public struct Enumerator : IEnumerator<int>
        {
            const sbyte NotStarted = -1;
            const sbyte Finished = -2;

            readonly ulong _value;
            readonly byte _count;
            sbyte _current;

            public readonly int Current
            {
                get
                {
                    if (_current < 0)
                    {
                        if (_current is NotStarted)
                            throw new InvalidOperationException("Enumeration has not started. Call MoveNext.");

                        throw new InvalidOperationException("Enumeration already finished.");
                    }

                    return _current;
                }
            }

            readonly object IEnumerator.Current => Current;

            internal Enumerator(ulong value, byte flagsCount)
            {
                _value = value;
                _count = flagsCount;
                _current = NotStarted;
            }

            public bool MoveNext()
            {
                if (_current is Finished)
                    throw new InvalidOperationException("Enumeration already finished.");

                for (; ++_current < _count;)
                {
                    ulong flag = _value & (ulong)(1 << _current);
                    if (flag != 0)
                        return true;
                }

                _current = Finished;
                return false;
            }

            public void Reset()
                => _current = NotStarted;

            readonly void IDisposable.Dispose() { }
        }

        readonly ulong _value;
        readonly byte _count;

        internal FlagIndices(ulong value, byte flagsCount)
        {
            _value = value;
            _count = flagsCount;
        }

        public Enumerator GetEnumerator()
            => new(_value, _count);

        IEnumerator<int> IEnumerable<int>.GetEnumerator()
            => GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public readonly struct Flags<T> : IEnumerable<T> where T : unmanaged, Enum
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        public struct Enumerator : IEnumerator<T>
        {
            FlagIndices.Enumerator _indexEnumerator; // not readonly

            public readonly T Current
            {
                get
                {
                    ulong x = 1UL << _indexEnumerator.Current;
                    return Unsafe.As<ulong, T>(ref x);
                }
            }

            readonly object IEnumerator.Current => Current;

            internal Enumerator(T value)
            {
                _indexEnumerator = new(AsULong(value), (byte)(Unsafe.SizeOf<T>() * 8));
            }

            public bool MoveNext()
                => _indexEnumerator.MoveNext();

            public void Reset()
                => _indexEnumerator.Reset();

            readonly void IDisposable.Dispose() { }
        }

        readonly T _value;

        internal Flags(T value)
        {
            _value = value;
        }

        public Enumerator GetEnumerator()
            => new(_value);

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
            => GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

    }

    /// <summary>
    /// Determine whether one or more bit fields are set in a value.
    /// </summary>
    /// <typeparam name="T">Enumeration type.</typeparam>
    /// <param name="value">Value.</param>
    /// <param name="flag">Bit field or fields.</param>
    /// <returns><c>true</c> if the bit field or fields are also set.</returns>
    public static bool HasFlag<T>(this T value, T flag) where T : unmanaged, Enum
    {
        ulong x = AsULong(value) & AsULong(flag);
        return x != 0;
    }

    /// <summary>
    /// Set one or more bit fields in a value.
    /// </summary>
    /// <typeparam name="T">Enumeration type.</typeparam>
    /// <param name="value">Value.</param>
    /// <param name="flag">Bit field or fields.</param>
    /// <returns>Copy of <paramref name="value"/>, with the bit field or fields also set.</returns>
    public static T AddFlag<T>(this T value, T flag) where T : unmanaged, Enum
    {
        ulong x = AsULong(value) | AsULong(flag);
        return Unsafe.As<ulong, T>(ref x);
    }

    /// <summary>
    /// Unset one or more bit fields in a value.
    /// </summary>
    /// <typeparam name="T">Enumeration type.</typeparam>
    /// <param name="value">Value.</param>
    /// <param name="flag">Bit field or fields.</param>
    /// <returns>Copy of <paramref name="value"/>, without the bit field or fields also set.</returns>
    public static T RemoveFlag<T>(this T value, T flag) where T : unmanaged, Enum
    {
        ulong x = AsULong(value) & ~AsULong(flag);
        return Unsafe.As<ulong, T>(ref x);
    }

    /// <summary>
    /// Count how many bit fields are set in a value.
    /// </summary>
    /// <typeparam name="T">Enumeration type.</typeparam>
    /// <param name="value">Value.</param>
    /// <returns>Number of bit fields set.</returns>
    public static int CountFlags<T>(this T value) where T : unmanaged, Enum
    {
        ulong i = AsULong(value);
        i -= (i >> 1) & 0x5555555555555555UL;
        i = (i & 0x3333333333333333UL) + ((i >> 2) & 0x3333333333333333UL);
        return (int)(unchecked(((i + (i >> 4)) & 0xF0F0F0F0F0F0F0FUL) * 0x101010101010101UL) >> 56);
    }

    /// <summary>
    /// Get a bit field by index among the set bit fields in a value.
    /// </summary>
    /// <typeparam name="T">Enumeration type.</typeparam>
    /// <param name="value">Value.</param>
    /// <param name="index">Index of the bit field among the set bit fields.</param>
    /// <returns>The bit field.</returns>
    /// <exception cref="IndexOutOfRangeException"><paramref name="index"/> is higher than the number of set bit fields.</exception>
    public static T GetFlag<T>(this T value, int index) where T : unmanaged, Enum
    {
        if (!TryGetFlag(value, index, out T flag))
            throw new IndexOutOfRangeException();

        return flag;
    }

    /// <summary>
    /// Get a bit field by index among the set bit fields in a value.
    /// </summary>
    /// <typeparam name="T">Enumeration type.</typeparam>
    /// <param name="value">Value.</param>
    /// <param name="index">Index of the bit field among the set bit fields.</param>
    /// <param name="flag">The bit field.</param>
    /// <returns><c>true</c> if <paramref name="index"/> is not higher than the number of set bit fields.</returns>
    public static bool TryGetFlag<T>(this T value, int index, out T flag) where T : unmanaged, Enum
    {
        if (TryGetFlagIndex(value, index, out int flagIndex))
        {
            ulong x = AsULong(value);
            ulong f = x & (ulong)(1 << flagIndex);
            
            flag = Unsafe.As<ulong, T>(ref f);
            return true;
        }

        flag = default;
        return false;
    }

    /// <summary>
    /// Get index of a bit field by index among the set bit fields in a value.
    /// </summary>
    /// <typeparam name="T">Enumeration type.</typeparam>
    /// <param name="value">Value.</param>
    /// <param name="index">Index of the bit field among the set bit fields.</param>
    /// <returns>Index of the bit field.</returns>
    /// <exception cref="IndexOutOfRangeException"><paramref name="index"/> is higher than the number of set bit fields.</exception>
    public static int GetFlagIndex<T>(this T value, int index) where T : unmanaged, Enum
    {
        if (!TryGetFlagIndex(value, index, out int flagIndex))
            throw new IndexOutOfRangeException();

        return flagIndex;
    }

    /// <summary>
    /// Get index of a bit field by index among the set bit fields in a value.
    /// </summary>
    /// <typeparam name="T">Enumeration type.</typeparam>
    /// <param name="value">Value.</param>
    /// <param name="index">Index of the bit field among the set bit fields.</param>
    /// <param name="flagIndex">Index of the bit field.</param>
    /// <returns><c>true</c> if <paramref name="index"/> is not higher than the number of set bit fields.</returns>
    public static bool TryGetFlagIndex<T>(this T value, int index, out int flagIndex) where T : unmanaged, Enum
    {
        ulong x = AsULong(value);

        int flagsCount = Unsafe.SizeOf<T>() * 8;
        for (flagIndex = 0; flagIndex < flagsCount; flagIndex++)
        {
            ulong f = x & (ulong)(1 << flagIndex);
            if (f != 0 && --index == -1)
                return true;
        }

        flagIndex = default;
        return false;
    }

    /// <summary>
    /// Get indices of the set bit fields in a value.
    /// </summary>
    /// <typeparam name="T">Enumeration type.</typeparam>
    /// <param name="value">Value.</param>
    /// <returns>Sequence of indices of the set bit fields.</returns>
    public static FlagIndices GetFlagIndices<T>(this T value) where T : unmanaged, Enum
        => new(AsULong(value), (byte)(Unsafe.SizeOf<T>() * 8));

    /// <summary>
    /// Get the set bit fields in a value.
    /// </summary>
    /// <typeparam name="T">Enumeration type.</typeparam>
    /// <param name="value">Value.</param>
    /// <returns>Sequence of the set bit fields.</returns>
    public static Flags<T> GetFlags<T>(this T value) where T : unmanaged, Enum
        => new(value);

    static ulong AsULong<T>(this T value) where T : unmanaged, Enum
    {
        return Unsafe.SizeOf<T>() switch {
            sizeof(byte) => Unsafe.As<T, byte>(ref value),
            sizeof(ushort) => Unsafe.As<T, ushort>(ref value),
            sizeof(uint) => Unsafe.As<T, uint>(ref value),
            sizeof(ulong) => Unsafe.As<T, ulong>(ref value),
            _ => throw new NotImplementedException()
        };
    }
}
