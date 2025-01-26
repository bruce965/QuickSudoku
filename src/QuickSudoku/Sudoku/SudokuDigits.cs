// SPDX-FileCopyrightText: Copyright 2025 Fabio Iotti
// SPDX-License-Identifier: AGPL-3.0-only

using QuickSudoku.Utilities;
using System.Collections;
using System.ComponentModel;

namespace QuickSudoku.Sudoku;

[Flags]
public enum SudokuDigits : ushort
{
    None = 0,
    Digit1 = 1 << 0,
    Digit2 = 1 << 1,
    Digit3 = 1 << 2,
    Digit4 = 1 << 3,
    Digit5 = 1 << 4,
    Digit6 = 1 << 5,
    Digit7 = 1 << 6,
    Digit8 = 1 << 7,
    Digit9 = 1 << 8,
    All = Digit1 | Digit2 | Digit3 | Digit4 | Digit5 | Digit6 | Digit7 | Digit8 | Digit9,
}

public static class SudokuDigitsExtensions
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public readonly struct Enumerable : IEnumerable<int>
    {
        public struct Enumerator : IEnumerator<int>
        {
            const sbyte NotStarted = -1;
            const sbyte Finished = -2;

            readonly ushort _value;
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

                    return _current + 1;
                }
            }

            readonly object IEnumerator.Current => Current;

            internal Enumerator(ushort value)
            {
                _value = value;
                _current = NotStarted;
            }

            public bool MoveNext()
            {
                if (_current is Finished)
                    throw new InvalidOperationException("Enumeration already finished.");

                for (; ++_current < 9;)
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

        readonly SudokuDigits _digits;

        internal Enumerable(SudokuDigits digits)
        {
            _digits = digits;
        }

        Enumerator GetEnumerator() => new((ushort)_digits);

        IEnumerator<int> IEnumerable<int>.GetEnumerator()
            => GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }

    public static SudokuDigits Digit(int digit)
    {
        if (digit < 1 || digit > 9)
            throw new ArgumentException(null, nameof(digit));

        return (SudokuDigits)(1 << (digit - 1));
    }

    public static void Add(this ref SudokuDigits digits, int digit)
    {
        digits = digits.AddFlag(Digit(digit));
    }

    public static bool Contains(this SudokuDigits digits, int digit)
    {
        if (digit < 1 || digit > 9)
            return false;

        return digits.HasFlag(Digit(digit));
    }

    public static void Remove(this ref SudokuDigits digits, int digit)
    {
        if (digit < 1 || digit > 9)
            return;

        digits = digits.RemoveFlag(Digit(digit));
    }

    public static int Count(this SudokuDigits digits)
        => digits.CountFlags();

    public static int At(this SudokuDigits digits, int index)
        => digits.GetFlagIndex(index) + 1;

    public static Enumerable GetDigits(this SudokuDigits digits)
        => new(digits);
}