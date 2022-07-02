using QuickSudoku.Abstractions;
using System.Collections;
using System.Diagnostics;

namespace QuickSudoku.Sudoku;

[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
public readonly struct SudokuSquare : IRegion, IEquatable<SudokuSquare>, IEnumerable<ICell>
{
    public SudokuPuzzle Puzzle { get; }

    public int Index { get; }

    public int X => Index % 3;
    public int Y => Index / 3;

    public SquareCells Cells => new(this);

    public SudokuCell this[int x, int y]
    {
        get
        {
            Debug.Assert(x >= 0 && x < 3);
            Debug.Assert(y >= 0 && y < 3);

            return Puzzle[X * 3 + x, Y * 3 + y];
        }
    }

    public SudokuCell this[int index]
    {
        get
        {
            Debug.Assert(index >= 0 && index < 9);

            return this[index % 3, index / 3];
        }
    }

    internal string DebuggerDisplay
    {
        get
        {
            var self = this;
            return $"Square {X}-{Y}: {string.Join("", Enumerable.Range(0, 9).Select(y => self[y].Value?.ToString() ?? ".").Zip(new[] { "", "", " ", "", "", " ", "", "", "" }).SelectMany(t => new[] { t.First, t.Second }))}";
        }
    }

    internal SudokuSquare(SudokuPuzzle puzzle, int index)
    {
        Debug.Assert(index >= 0 && index < 9);

        Puzzle = puzzle;
        Index = index;
    }

    #region IRegion

    IPuzzle IRegion.Puzzle => Puzzle;

    IEnumerable<object> IRegion.LegalValues => SudokuPuzzle.LegalValues;

    IEnumerable<ICell> IRegion.Cells => this;

    public bool Equals(IRegion? other)
        => other is SudokuSquare s && Equals(s);

    #endregion

    #region IEnumerable<ICell>

    IEnumerator<ICell> IEnumerable<ICell>.GetEnumerator()
    {
        for (var y = 0; y < 9; y++)
            yield return this[y];
    }

    #endregion

    #region SquareCells

    public struct SquareCells : IReadOnlyList<SudokuCell>
    {
        public struct Enumerator : IEnumerator<SudokuCell>
        {
            readonly SudokuSquare _square;
            int _index;

            public SudokuCell Current
            {
                get
                {
                    Debug.Assert(_index >= 0 && _index < 9);

                    return _square[_index];
                }
            }

            object IEnumerator.Current => Current;

            public Enumerator(SudokuSquare square)
            {
                _square = square;
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

        readonly SudokuSquare _square;

        int IReadOnlyCollection<SudokuCell>.Count => 9;

        public SudokuCell this[int index] => _square[index];

        internal SquareCells(SudokuSquare square)
        {
            _square = square;
        }

        public Enumerator GetEnumerator() => new(_square);

        IEnumerator<SudokuCell> IEnumerable<SudokuCell>.GetEnumerator() => GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    #endregion

    #region IEquatable<SudokuSquare>

    bool IEquatable<SudokuSquare>.Equals(SudokuSquare other) => this == other;

    public override bool Equals(object? obj)
        => obj is SudokuSquare s && Equals(s);

    public override int GetHashCode()
        => Index;

    public static bool operator ==(SudokuSquare left, SudokuSquare right)
        => left.Puzzle == right.Puzzle && left.Index == right.Index;

    public static bool operator !=(SudokuSquare left, SudokuSquare right)
        => !(left == right);

    #endregion

    IEnumerator IEnumerable.GetEnumerator() => Cells.GetEnumerator();
}
