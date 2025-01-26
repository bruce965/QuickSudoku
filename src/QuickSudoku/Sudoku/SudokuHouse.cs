// SPDX-FileCopyrightText: Copyright 2025 Fabio Iotti
// SPDX-License-Identifier: AGPL-3.0-only

using QuickSudoku.Abstractions;
using System.Collections;
using System.Diagnostics;

namespace QuickSudoku.Sudoku;

[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
public readonly struct SudokuHouse : IHouse, IEquatable<SudokuHouse>, IEnumerable<ICell>
{
    public SudokuPuzzle Puzzle { get; }

    // 0-8 = row, 9-17 = column, 18-26 = square
    readonly int _index;

    public HouseCells Cells => new(this);

    public HouseIntersections IntersectingHouses => new(this);

    public SudokuCell this[int index]
    {
        get
        {
            if (_index < 9)
                return Puzzle.Rows[_index][index];

            if (_index < 18)
                return Puzzle.Columns[_index - 9][index];

            return Puzzle.Squares[_index - 18][index];
        }
    }

    internal string DebuggerDisplay
    {
        get
        {
            if (_index < 9)
                return Puzzle.Rows[_index].DebuggerDisplay;

            if (_index < 18)
                return Puzzle.Columns[_index - 9].DebuggerDisplay;

            return Puzzle.Squares[_index - 18].DebuggerDisplay;
        }
    }

    internal SudokuHouse(SudokuPuzzle puzzle, int index)
    {
        Debug.Assert(index >= 0 && index < 27);

        Puzzle = puzzle;
        _index = index;
    }

    #region IHouse

    IPuzzle IHouse.Puzzle => Puzzle;

    IEnumerable<object> IHouse.LegalValues => SudokuPuzzle.LegalValues;

    IEnumerable<ICell> IHouse.Cells => this;

    public bool Equals(IHouse? other)
        => other is SudokuRow s && Equals(s);

    #endregion

    #region IEnumerable<ICell>

    IEnumerator<ICell> IEnumerable<ICell>.GetEnumerator()
    {
        if (_index < 9)
            return ((IEnumerable<ICell>)Puzzle.Rows[_index]).GetEnumerator();
        
        if (_index < 18)
            return ((IEnumerable<ICell>)Puzzle.Columns[_index - 9]).GetEnumerator();

        return ((IEnumerable<ICell>)Puzzle.Squares[_index - 18]).GetEnumerator();
    }

    #endregion

    #region HouseCells

    public readonly struct HouseCells : IReadOnlyList<SudokuCell>
    {
        public struct Enumerator : IEnumerator<SudokuCell>
        {
            readonly SudokuHouse _row;
            int _index;

            public readonly SudokuCell Current
            {
                get
                {
                    Debug.Assert(_index >= 0 && _index < 9);

                    return _row[_index];
                }
            }

            readonly object IEnumerator.Current => Current;

            public Enumerator(SudokuHouse house)
            {
                _row = house;
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

            public readonly void Dispose() { }
        }

        readonly SudokuHouse _house;

        int IReadOnlyCollection<SudokuCell>.Count => 9;

        public SudokuCell this[int index] => _house[index];

        internal HouseCells(SudokuHouse house)
        {
            _house = house;
        }

        public Enumerator GetEnumerator() => new(_house);

        IEnumerator<SudokuCell> IEnumerable<SudokuCell>.GetEnumerator() => GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    #endregion

    #region HouseIntersections

    int _intersectionsCount => _index < 18 ? 12 : 6; // row/col = 12, square = 6

    public readonly struct HouseIntersections : IReadOnlyList<SudokuHousesIntersection>
    {
        public struct Enumerator : IEnumerator<SudokuHousesIntersection>
        {
            readonly SudokuHouse _house;
            int _index;

            public readonly SudokuHousesIntersection Current
            {
                get
                {
                    Debug.Assert(_index >= 0 && _index < _house._intersectionsCount);

                    return new HouseIntersections(_house)[_index];
                }
            }

            readonly object IEnumerator.Current => Current;

            public Enumerator(SudokuHouse house)
            {
                _house = house;
                _index = -1;
            }

            public bool MoveNext()
            {
                _index++;
                return _index < _house._intersectionsCount;
            }

            public void Reset()
            {
                _index = -1;
            }

            public readonly void Dispose() { }
        }

        readonly SudokuHouse _house;

        int IReadOnlyCollection<SudokuHousesIntersection>.Count => _house._intersectionsCount;

        public SudokuHousesIntersection this[int index]
        {
            get => (_house._index, index) switch
            {
                // row with 9 columns and 3 squares
                (int r, int i) when r < 9 && i < 9 => new(_house, _house.Puzzle.Columns[i]),
                (int r, int i) when r < 9 && i < 18 => new(_house, _house.Puzzle.Squares[(r / 3) * 3 + i - 9]),

                // column with 9 rows and 3 squares
                (int r, int i) when r < 18 && i < 9 => new(_house, _house.Puzzle.Rows[i]),
                (int r, int i) when r < 18 && i < 18 => new(_house, _house.Puzzle.Squares[(i - 9) * 3 + (r - 9) / 3]),

                // square with 3 rows and 3 columns
                (int r, int i) when r < 27 && i < 3 => new(_house, _house.Puzzle.Rows[((r - 18) / 3) * 3 + i]),
                (int r, int i) when r < 27 && i < 6 => new(_house, _house.Puzzle.Columns[((r - 18) % 3) * 3 + i - 3]),

                (_, _) => throw new IndexOutOfRangeException()
            };
        }

        internal HouseIntersections(SudokuHouse house)
        {
            _house = house;
        }

        public Enumerator GetEnumerator() => new(_house);

        IEnumerator<SudokuHousesIntersection> IEnumerable<SudokuHousesIntersection>.GetEnumerator() => GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    #endregion

    #region IEquatable<SudokuHouse>

    bool IEquatable<SudokuHouse>.Equals(SudokuHouse other) => this == other;

    public override bool Equals(object? obj)
        => obj is SudokuHouse s && Equals(s);

    public override int GetHashCode()
        => _index;

    public static bool operator ==(SudokuHouse left, SudokuHouse right)
        => left.Puzzle == right.Puzzle && left._index == right._index;

    public static bool operator !=(SudokuHouse left, SudokuHouse right)
        => !(left == right);

    #endregion

    public static implicit operator SudokuHouse(SudokuRow row)
        => new(row.Puzzle, row.Y);

    public static implicit operator SudokuHouse(SudokuColumn column)
        => new(column.Puzzle, 9 + column.X);

    public static implicit operator SudokuHouse(SudokuSquare square)
        => new(square.Puzzle, 18 + square.Index);

    IEnumerator IEnumerable.GetEnumerator() => Cells.GetEnumerator();
}
