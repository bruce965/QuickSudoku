﻿// SPDX-FileCopyrightText: Copyright 2025 Fabio Iotti
// SPDX-License-Identifier: AGPL-3.0-only

using QuickSudoku.Abstractions;
using System.Collections;
using System.Diagnostics;

namespace QuickSudoku.Sudoku;

[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
public readonly struct SudokuCell : ICell, IEquatable<SudokuCell>, IEnumerable<IHouse>
{
    [DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
    public readonly struct CellCandidateValues : IValuesCollection<int>
    {
        readonly SudokuCell _cell;

        internal ref SudokuDigits Ref => ref _cell.Puzzle._data[_cell.Index];

        public SudokuDigits Digits => Ref;

        public int Count
        {
            get
            {
                // https://stackoverflow.com/a/12171691
                int value = (int)Ref;

                int count = 0;
                while (value != 0)
                {
                    count++;
                    value &= value - 1;
                }

                return count;
            }
        }

        internal string DebuggerDisplay
        {
            get
            {
                CellCandidateValues self = this;
                return string.Join("", Enumerable.Range(1, 9).Select(v => self.Contains(v) ? v.ToString() : ""));
            }
        }

        internal CellCandidateValues(SudokuCell cell)
        {
            _cell = cell;
        }

        public void Set(int digit)
        {
            Ref = SudokuDigitsExtensions.Digit(digit);
        }

        public void Add(int digit)
            => Ref.Add(digit);

        public bool Contains(int digit)
            => Ref.Contains(digit);

        public void Remove(int digit)
            => Ref.Remove(digit);

        public void Reset()
            => Ref = SudokuDigits.All;

        #region IEnumerable<int>

        public struct Enumerator : IEnumerator<int>, IEnumerator<object>
        {
            readonly SudokuDigits _digits;
            int _value;

            public readonly int Current
            {
                get
                {
                    Debug.Assert(_value >= 1 && _value <= 9);

                    return _value;
                }
            }

            readonly object IEnumerator<object>.Current => Current;

            readonly object IEnumerator.Current => Current;

            public Enumerator(SudokuDigits digits)
            {
                _digits = digits;
                _value = 0;
            }

            public bool MoveNext()
            {
                _value++;

                for (; _value <= 9; _value++)
                    if (((int)_digits & (1 << (_value - 1))) != 0)
                        break;

                return _value <= 9;
            }

            public void Reset()
            {
                _value = 0;
            }

            public readonly void Dispose() { }
        }

        public Enumerator GetEnumerator() => new(Ref);

        IEnumerator<int> IEnumerable<int>.GetEnumerator() => GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion
    }

    readonly record struct CellCandidateValuesCollection(CellCandidateValues Values) : IValuesCollection<object>
    {
        public void Add(object item)
            => Values.Add((int)item);

        public bool Contains(object item)
            => item is int v && Values.Contains(v);

        public void Remove(object item)
        {
            if (item is int v)
                Values.Remove(v);
        }

        public void Reset()
            => Values.Reset();

        public IEnumerator<object> GetEnumerator() => Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public SudokuPuzzle Puzzle { get; }

    public SudokuCellIndex Index { get; }

    public SudokuRow Row => new(Puzzle, Index.Y);
    public SudokuColumn Column => new(Puzzle, Index.X);
    public SudokuSquare Square => new(Puzzle, Index.Square);
    public CellHouses Houses => new(this);

    /// <summary>
    /// Get or set digits allowed on this cell.
    /// </summary>
    public CellCandidateValues CandidateValues => new(this);

    /// <summary>
    /// Get or set exact value of this cell.
    /// </summary>
    public int? Value
    {
        get => CandidateValues.Ref switch
        {
            SudokuDigits.Digit1 => 1,
            SudokuDigits.Digit2 => 2,
            SudokuDigits.Digit3 => 3,
            SudokuDigits.Digit4 => 4,
            SudokuDigits.Digit5 => 5,
            SudokuDigits.Digit6 => 6,
            SudokuDigits.Digit7 => 7,
            SudokuDigits.Digit8 => 8,
            SudokuDigits.Digit9 => 9,
            _ => null,
        };
        set
        {
            Debug.Assert(value is null or (>= 1 and <= 9));

            if (value.HasValue)
                CandidateValues.Set(value.Value);
            else
                CandidateValues.Reset();
        }
    }

    internal string DebuggerDisplay
        => $"{Index.DebuggerDisplay}: {CandidateValues.DebuggerDisplay}";

    internal SudokuCell(SudokuPuzzle puzzle, SudokuCellIndex index)
    {
        Puzzle = puzzle;
        Index = index;
    }

    #region ICell

    IPuzzle ICell.Puzzle => Puzzle;

    IEnumerable<IHouse> ICell.Houses => this;

    IEnumerable<object> ICell.LegalValues => SudokuPuzzle.LegalValues;

    IValuesCollection<object> ICell.CandidateValues => new CellCandidateValuesCollection(CandidateValues);

    bool IEquatable<ICell>.Equals(ICell? other) => other is SudokuCell s && Equals(s);

    #endregion

    #region IEnumerable<IHouse>

    IEnumerator<IHouse> IEnumerable<IHouse>.GetEnumerator()
    {
        yield return Row;
        yield return Column;
        yield return Square;
    }

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<IHouse>)this).GetEnumerator();

    #endregion

    #region IEquatable<SudokuCell>

    bool IEquatable<SudokuCell>.Equals(SudokuCell other) => this == other;

    public override bool Equals(object? obj)
        => obj is SudokuCell s && Equals(s);

    public override int GetHashCode()
        => Index;

    public static bool operator ==(SudokuCell left, SudokuCell right)
        => left.Puzzle == right.Puzzle && left.Index == right.Index;

    public static bool operator !=(SudokuCell left, SudokuCell right)
        => !(left == right);

    #endregion

    #region CellHouses

    public readonly struct CellHouses : IReadOnlyList<SudokuHouse>
    {
        public struct Enumerator : IEnumerator<SudokuHouse>
        {
            readonly SudokuCell _cell;
            int _index;

            public readonly SudokuHouse Current
            {
                get
                {
                    return _index switch
                    {
                        0 => _cell.Row,
                        1 => _cell.Column,
                        2 => _cell.Square,
                        _ => throw new InvalidOperationException()
                    };
                }
            }

            readonly object IEnumerator.Current => Current;

            internal Enumerator(SudokuCell cell)
            {
                _cell = cell;
                _index = -1;
            }

            public bool MoveNext()
            {
                _index++;
                return _index < 3;
            }

            public void Reset()
            {
                _index = -1;
            }

            public readonly void Dispose() { }
        }


        readonly SudokuCell _cell;

        int IReadOnlyCollection<SudokuHouse>.Count => 3;

        public SudokuHouse this[int index] => index switch
        {
            0 => _cell.Row,
            1 => _cell.Column,
            2 => _cell.Square,
            _ => throw new ArgumentOutOfRangeException(nameof(index))
        };

        internal CellHouses(SudokuCell cell)
        {
            _cell = cell;
        }

        public Enumerator GetEnumerator() => new(_cell);

        IEnumerator<SudokuHouse> IEnumerable<SudokuHouse>.GetEnumerator() => GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    #endregion
}
