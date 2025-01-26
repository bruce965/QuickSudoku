// SPDX-FileCopyrightText: Copyright 2025 Fabio Iotti
// SPDX-License-Identifier: AGPL-3.0-only

using QuickSudoku.Abstractions;
using System.Collections;
using System.Diagnostics;

namespace QuickSudoku.Sudoku;

[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
public class SudokuPuzzle : IPuzzle, IHouse, IEquatable<SudokuPuzzle>, IEnumerable<IHouse>, IEnumerable<ICell>
{
    internal readonly SudokuDigits[] _data;

    public PuzzleRows Rows => new(this);
    public PuzzleColumns Columns => new(this);
    public PuzzleSquares Squares => new(this);
    public PuzzleHouses Houses => new(this);
    public PuzzleCells Cells => new(this);

    public SudokuCell this[SudokuCellIndex index] => new(this, index);

    public SudokuCell this[int index] => new(this, index);

    public SudokuCell this[int x, int y] => new(this, new SudokuCellIndex(x, y));

    public SudokuPuzzle()
    {
        _data = new SudokuDigits[81];
        Array.Fill(_data, SudokuDigits.All);
    }

    internal string DebuggerDisplay
        => "Sudoku Puzzle";

    protected SudokuPuzzle(SudokuDigits[] data) : this()
    {
        Debug.Assert(data.Length == 81);

        data.CopyTo(_data, 0);
    }

    public static SudokuPuzzle FromScheme(string scheme)
    {
        var puzzle = new SudokuPuzzle();

        var i = 0;
        for (var chrIndex = 0; chrIndex < scheme.Length; chrIndex++)
        {
            var chr = scheme[chrIndex];

            if (chr == '.')
            {
                i++;
            }
            else if (chr >= '1' && chr <= '9')
            {
                if (i < 81)
                {
                    var cell = puzzle[i];
                    cell.Value = chr - '0';
                }

                i++;
            }
        }

        if (i != 81)
            throw new ArgumentException("Invalid scheme, should be compromised of 81 digits or blank spots.", nameof(scheme));

        return puzzle;
    }

    public virtual SudokuPuzzle Clone()
        => new(_data);

    public virtual void CopyTo(SudokuPuzzle puzzle)
        => _data.CopyTo(puzzle._data.AsSpan());

    #region IPuzzle

    internal static readonly IReadOnlyList<object> LegalValues = new object[]
    {
        SudokuDigits.Digit1,
        SudokuDigits.Digit2,
        SudokuDigits.Digit3,
        SudokuDigits.Digit4,
        SudokuDigits.Digit5,
        SudokuDigits.Digit6,
        SudokuDigits.Digit7,
        SudokuDigits.Digit8,
        SudokuDigits.Digit9,
    };

    IEnumerable<IHouse> IPuzzle.Houses => this;

    object ICloneable.Clone() => Clone();

    void IPuzzle.CopyTo(IPuzzle puzzle)
    {
        if (puzzle is not SudokuPuzzle other)
            throw new ArgumentException("Incompatible puzzle board.", nameof(puzzle));

        CopyTo(other);
    }

    #endregion

    #region IHouse

    IPuzzle IHouse.Puzzle => this;

    IEnumerable<object> IHouse.LegalValues => LegalValues;

    IEnumerable<ICell> IHouse.Cells => this;

    public bool Equals(IHouse? other)
        => other is SudokuPuzzle s && Equals(s);

    #endregion

    #region IEnumerable<IHouse>

    IEnumerator<IHouse> IEnumerable<IHouse>.GetEnumerator()
    {
        for (var y = 0; y < 9; y++)
            yield return new SudokuRow(this, y);

        for (var x = 0; x < 9; x++)
            yield return new SudokuColumn(this, x);

        for (var i = 0; i < 9; i++)
            yield return new SudokuSquare(this, i);
    }

    #endregion

    #region IEnumerable<ICell>

    IEnumerator<ICell> IEnumerable<ICell>.GetEnumerator()
    {
        for (var i = 0; i < 81; i++)
            yield return this[i];
    }

    #endregion

    #region PuzzleRows

    public struct PuzzleRows : IReadOnlyList<SudokuRow>
    {
        public struct Enumerator : IEnumerator<SudokuRow>
        {
            readonly SudokuPuzzle _puzzle;
            int _y;

            public SudokuRow Current
            {
                get
                {
                    Debug.Assert(_y >= 0 && _y < 9);

                    return _puzzle.Rows[_y];
                }
            }

            object IEnumerator.Current => Current;

            internal Enumerator(SudokuPuzzle puzzle)
            {
                _puzzle = puzzle;
                _y = -1;
            }

            public bool MoveNext()
            {
                _y++;
                return _y < 9;
            }

            public void Reset()
            {
                _y = -1;
            }

            public void Dispose() { }
        }


        readonly SudokuPuzzle _puzzle;

        int IReadOnlyCollection<SudokuRow>.Count => 9;

        public SudokuRow this[int y] => new(_puzzle, y);

        internal PuzzleRows(SudokuPuzzle puzzle)
        {
            _puzzle = puzzle;
        }

        public Enumerator GetEnumerator() => new(_puzzle);

        IEnumerator<SudokuRow> IEnumerable<SudokuRow>.GetEnumerator() => GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    #endregion

    #region PuzzleColumns

    public struct PuzzleColumns : IReadOnlyList<SudokuColumn>
    {
        public struct Enumerator : IEnumerator<SudokuColumn>
        {
            readonly SudokuPuzzle _puzzle;
            int _x;

            public SudokuColumn Current
            {
                get
                {
                    Debug.Assert(_x >= 0 && _x < 9);

                    return _puzzle.Columns[_x];
                }
            }

            object IEnumerator.Current => Current;

            public Enumerator(SudokuPuzzle puzzle)
            {
                _puzzle = puzzle;
                _x = -1;
            }

            public bool MoveNext()
            {
                _x++;
                return _x < 9;
            }

            public void Reset()
            {
                _x = -1;
            }

            public void Dispose() { }
        }

        readonly SudokuPuzzle _puzzle;

        int IReadOnlyCollection<SudokuColumn>.Count => 9;

        public SudokuColumn this[int x] => new(_puzzle, x);

        internal PuzzleColumns(SudokuPuzzle puzzle)
        {
            _puzzle = puzzle;
        }

        public Enumerator GetEnumerator() => new(_puzzle);

        IEnumerator<SudokuColumn> IEnumerable<SudokuColumn>.GetEnumerator() => GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    #endregion

    #region PuzzleSquares

    public struct PuzzleSquares : IReadOnlyList<SudokuSquare>
    {
        public struct Enumerator : IEnumerator<SudokuSquare>
        {
            readonly SudokuPuzzle _puzzle;
            int _index;

            public SudokuSquare Current
            {
                get
                {
                    Debug.Assert(_index >= 0 && _index < 9);

                    return _puzzle.Squares[_index];
                }
            }

            object IEnumerator.Current => Current;

            public Enumerator(SudokuPuzzle puzzle)
            {
                _puzzle = puzzle;
                _index = -1;
            }

            public bool MoveNext()
            {
                _index++;
                return _index < 9;
            }

            public void Reset()
            {
                _index = -1;
            }

            public void Dispose() { }
        }

        readonly SudokuPuzzle _puzzle;

        int IReadOnlyCollection<SudokuSquare>.Count => 9;

        public SudokuSquare this[int index] => new(_puzzle, index);

        internal PuzzleSquares(SudokuPuzzle puzzle)
        {
            _puzzle = puzzle;
        }

        public Enumerator GetEnumerator() => new(_puzzle);

        IEnumerator<SudokuSquare> IEnumerable<SudokuSquare>.GetEnumerator() => GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    #endregion

    #region PuzzleHouses

    public struct PuzzleHouses : IReadOnlyList<SudokuHouse>
    {
        public struct Enumerator : IEnumerator<SudokuHouse>
        {
            readonly SudokuPuzzle _puzzle;
            int _index;

            public SudokuHouse Current
            {
                get
                {
                    Debug.Assert(_index >= 0 && _index < 27);

                    if (_index < 9)
                        return _puzzle.Rows[_index];

                    if (_index < 18)
                        return _puzzle.Columns[_index - 9];

                    return _puzzle.Squares[_index - 18];
                }
            }

            object IEnumerator.Current => Current;

            internal Enumerator(SudokuPuzzle puzzle)
            {
                _puzzle = puzzle;
                _index = -1;
            }

            public bool MoveNext()
            {
                _index++;
                return _index < 27;
            }

            public void Reset()
            {
                _index = -1;
            }

            public void Dispose() { }
        }


        readonly SudokuPuzzle _puzzle;

        int IReadOnlyCollection<SudokuHouse>.Count => 27;

        public SudokuHouse this[int index] => new(_puzzle, index);

        internal PuzzleHouses(SudokuPuzzle puzzle)
        {
            _puzzle = puzzle;
        }

        public Enumerator GetEnumerator() => new(_puzzle);

        IEnumerator<SudokuHouse> IEnumerable<SudokuHouse>.GetEnumerator() => GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    #endregion

    #region PuzzleCells

    public struct PuzzleCells : IReadOnlyList<SudokuCell>
    {
        public struct Enumerator : IEnumerator<SudokuCell>
        {
            readonly SudokuPuzzle _puzzle;
            int _index;

            public SudokuCell Current
            {
                get
                {
                    Debug.Assert(_index >= 0 && _index < 81);

                    return _puzzle[_index];
                }
            }

            object IEnumerator.Current => Current;

            public Enumerator(SudokuPuzzle puzzle)
            {
                _puzzle = puzzle;
                _index = -1;
            }

            public bool MoveNext()
            {
                _index++;
                return _index < 81;
            }

            public void Reset()
            {
                _index = -1;
            }

            public void Dispose() { }
        }

        readonly SudokuPuzzle _puzzle;

        int IReadOnlyCollection<SudokuCell>.Count => 81;

        public SudokuCell this[int index] => new(_puzzle, index);

        internal PuzzleCells(SudokuPuzzle puzzle)
        {
            _puzzle = puzzle;
        }

        public Enumerator GetEnumerator() => new(_puzzle);

        IEnumerator<SudokuCell> IEnumerable<SudokuCell>.GetEnumerator() => GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    #endregion

    #region IEquatable<SudokuPuzzle>

    bool IEquatable<SudokuPuzzle>.Equals(SudokuPuzzle? other) => this == other;

    public override bool Equals(object? obj)
        => obj is SudokuRow s && Equals(s);

    public override int GetHashCode()
        => _data.GetHashCode();  // NOTE: could probably be improved.

    public static bool operator ==(SudokuPuzzle? left, SudokuPuzzle? right)
        => left?._data == right?._data;  // NOTE: this is a by-reference comparison.

    public static bool operator !=(SudokuPuzzle? left, SudokuPuzzle? right)
        => !(left == right);

    #endregion

    IEnumerator IEnumerable.GetEnumerator() => Cells.GetEnumerator();
}
