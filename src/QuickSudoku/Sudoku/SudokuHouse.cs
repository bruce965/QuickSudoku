using QuickSudoku.Abstractions;
using System.Collections;
using System.Diagnostics;

namespace QuickSudoku.Sudoku;

[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
public readonly struct SudokuRegion : IRegion, IEquatable<SudokuRegion>, IEnumerable<ICell>
{
    public SudokuPuzzle Puzzle { get; }

    // 0-8 = row, 9-17 = column, 18-26 = square
    readonly int _index;

    public RegionCells Cells => new(this);

    public RegionIntersections IntersectingRegions => new(this);

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

    internal SudokuRegion(SudokuPuzzle puzzle, int index)
    {
        Debug.Assert(index >= 0 && index < 27);

        Puzzle = puzzle;
        _index = index;
    }

    #region IRegion

    IPuzzle IRegion.Puzzle => Puzzle;

    IEnumerable<object> IRegion.LegalValues => SudokuPuzzle.LegalValues;

    IEnumerable<ICell> IRegion.Cells => this;

    public bool Equals(IRegion? other)
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

    #region RegionCells

    public struct RegionCells : IReadOnlyList<SudokuCell>
    {
        public struct Enumerator : IEnumerator<SudokuCell>
        {
            readonly SudokuRegion _row;
            int _index;

            public SudokuCell Current
            {
                get
                {
                    Debug.Assert(_index >= 0 && _index < 9);

                    return _row[_index];
                }
            }

            object IEnumerator.Current => Current;

            public Enumerator(SudokuRegion region)
            {
                _row = region;
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

        readonly SudokuRegion _region;

        int IReadOnlyCollection<SudokuCell>.Count => 9;

        public SudokuCell this[int index] => _region[index];

        internal RegionCells(SudokuRegion region)
        {
            _region = region;
        }

        public Enumerator GetEnumerator() => new(_region);

        IEnumerator<SudokuCell> IEnumerable<SudokuCell>.GetEnumerator() => GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    #endregion

    #region RegionIntersections

    int _intersectionsCount => _index < 18 ? 12 : 6;  // row/col = 12, square = 6

    public struct RegionIntersections : IReadOnlyList<SudokuRegionsIntersection>
    {
        public struct Enumerator : IEnumerator<SudokuRegionsIntersection>
        {
            readonly SudokuRegion _region;
            int _index;

            public SudokuRegionsIntersection Current
            {
                get
                {
                    Debug.Assert(_index >= 0 && _index < _region._intersectionsCount);

                    return new RegionIntersections(_region)[_index];
                }
            }

            object IEnumerator.Current => Current;

            public Enumerator(SudokuRegion region)
            {
                _region = region;
                _index = -1;
            }

            public bool MoveNext()
            {
                _index++;
                return _index < _region._intersectionsCount;
            }

            public void Reset()
            {
                _index = -1;
            }

            public void Dispose() { }
        }

        readonly SudokuRegion _region;

        int IReadOnlyCollection<SudokuRegionsIntersection>.Count => _region._intersectionsCount;

        public SudokuRegionsIntersection this[int index]
        {
            get => (_region._index, index) switch
            {
                // row with 9 columns and 3 squares
                (int r, int i) when r < 9 && i < 9 => new(_region, _region.Puzzle.Columns[i]),
                (int r, int i) when r < 9 && i < 18 => new(_region, _region.Puzzle.Squares[(r / 3) * 3 + i - 9]),

                // column with 9 rows and 3 squares
                (int r, int i) when r < 18 && i < 9 => new(_region, _region.Puzzle.Rows[i]),
                (int r, int i) when r < 18 && i < 18 => new(_region, _region.Puzzle.Squares[(i - 9) * 3 + (r - 9) / 3]),

                // square with 3 rows and 3 columns
                (int r, int i) when r < 27 && i < 3 => new(_region, _region.Puzzle.Rows[((r - 18) / 3) * 3 + i]),
                (int r, int i) when r < 27 && i < 6 => new(_region, _region.Puzzle.Columns[((r - 18) % 3) * 3 + i - 3]),

                (_, _) => throw new IndexOutOfRangeException()
            };
        }

        internal RegionIntersections(SudokuRegion region)
        {
            _region = region;
        }

        public Enumerator GetEnumerator() => new(_region);

        IEnumerator<SudokuRegionsIntersection> IEnumerable<SudokuRegionsIntersection>.GetEnumerator() => GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    #endregion

    #region IEquatable<SudokuRegion>

    bool IEquatable<SudokuRegion>.Equals(SudokuRegion other) => this == other;

    public override bool Equals(object? obj)
        => obj is SudokuRegion s && Equals(s);

    public override int GetHashCode()
        => _index;

    public static bool operator ==(SudokuRegion left, SudokuRegion right)
        => left.Puzzle == right.Puzzle && left._index == right._index;

    public static bool operator !=(SudokuRegion left, SudokuRegion right)
        => !(left == right);

    #endregion

    public static implicit operator SudokuRegion(SudokuRow row)
        => new(row.Puzzle, row.Y);

    public static implicit operator SudokuRegion(SudokuColumn column)
        => new(column.Puzzle, 9 + column.X);

    public static implicit operator SudokuRegion(SudokuSquare square)
        => new(square.Puzzle, 18 + square.Index);

    IEnumerator IEnumerable.GetEnumerator() => Cells.GetEnumerator();
}
