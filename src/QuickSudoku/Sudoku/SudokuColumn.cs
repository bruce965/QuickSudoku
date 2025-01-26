// SPDX-FileCopyrightText: Copyright 2025 Fabio Iotti
// SPDX-License-Identifier: AGPL-3.0-only

using QuickSudoku.Abstractions;
using System.Collections;
using System.Diagnostics;

namespace QuickSudoku.Sudoku;

[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
public readonly struct SudokuColumn : IHouse, IEquatable<SudokuColumn>, IEnumerable<ICell>
{
    public SudokuPuzzle Puzzle { get; }

    public int X { get; }

    public ColumnCells Cells => new(this);

    public SudokuCell this[int y] => Puzzle[X, y];

    internal string DebuggerDisplay
    {
        get
        {
            var self = this;
            return $"Column {X}: {string.Join("", Enumerable.Range(0, 9).Select(y => self[y].Value?.ToString() ?? "."))}";
        }
    }

    internal SudokuColumn(SudokuPuzzle puzzle, int x)
    {
        Debug.Assert(x >= 0 && x < 9);

        Puzzle = puzzle;
        X = x;
    }

    #region IHouse

    IPuzzle IHouse.Puzzle => Puzzle;

    IEnumerable<object> IHouse.LegalValues => SudokuPuzzle.LegalValues;

    IEnumerable<ICell> IHouse.Cells => this;

    public bool Equals(IHouse? other)
        => other is SudokuColumn s && Equals(s);

    #endregion

    #region IEnumerable<ICell>

    IEnumerator<ICell> IEnumerable<ICell>.GetEnumerator()
    {
        for (var y = 0; y < 9; y++)
            yield return this[y];
    }

    #endregion

    #region ColumnCells

    public struct ColumnCells : IReadOnlyList<SudokuCell>
    {
        public struct Enumerator : IEnumerator<SudokuCell>
        {
            readonly SudokuColumn _column;
            int _x;

            public SudokuCell Current
            {
                get
                {
                    Debug.Assert(_x >= 0 && _x < 9);

                    return _column[_x];
                }
            }

            object IEnumerator.Current => Current;

            public Enumerator(SudokuColumn column)
            {
                _column = column;
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

        readonly SudokuColumn _column;

        int IReadOnlyCollection<SudokuCell>.Count => 9;

        public SudokuCell this[int y] => _column[y];

        internal ColumnCells(SudokuColumn column)
        {
            _column = column;
        }

        public Enumerator GetEnumerator() => new(_column);

        IEnumerator<SudokuCell> IEnumerable<SudokuCell>.GetEnumerator() => GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    #endregion

    #region IEquatable<SudokuColumn>

    bool IEquatable<SudokuColumn>.Equals(SudokuColumn other) => this == other;

    public override bool Equals(object? obj)
        => obj is SudokuColumn s && Equals(s);

    public override int GetHashCode()
        => X;

    public static bool operator ==(SudokuColumn left, SudokuColumn right)
        => left.Puzzle == right.Puzzle && left.X == right.X;

    public static bool operator !=(SudokuColumn left, SudokuColumn right)
        => !(left == right);

    #endregion

    IEnumerator IEnumerable.GetEnumerator() => Cells.GetEnumerator();
}
