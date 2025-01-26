// SPDX-FileCopyrightText: Copyright 2025 Fabio Iotti
// SPDX-License-Identifier: AGPL-3.0-only

using QuickSudoku.Abstractions;
using System.Collections;
using System.Diagnostics;

namespace QuickSudoku.Sudoku;

[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
public readonly struct SudokuRow : IHouse, IEquatable<SudokuRow>, IEnumerable<ICell>
{
    public SudokuPuzzle Puzzle { get; }

    public int Y { get; }

    public RowCells Cells => new(this);

    public SudokuCell this[int x] => Puzzle[x, Y];

    internal string DebuggerDisplay
    {
        get
        {
            SudokuRow self = this;
            return $"Row {Y}: {string.Join("", Enumerable.Range(0, 9).Select(x => self[x].Value?.ToString() ?? "."))}";
        }
    }

    internal SudokuRow(SudokuPuzzle puzzle, int y)
    {
        Debug.Assert(y >= 0 && y < 9);

        Puzzle = puzzle;
        Y = y;
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
        for (int x = 0; x < 9; x++)
            yield return this[x];
    }

    #endregion

    #region RowCells

    public readonly struct RowCells : IReadOnlyList<SudokuCell>
    {
        public struct Enumerator : IEnumerator<SudokuCell>
        {
            readonly SudokuRow _row;
            int _y;

            public readonly SudokuCell Current
            {
                get
                {
                    Debug.Assert(_y >= 0 && _y < 9);

                    return _row[_y];
                }
            }

            readonly object IEnumerator.Current => Current;

            public Enumerator(SudokuRow row)
            {
                _row = row;
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

            public readonly void Dispose() { }
        }

        readonly SudokuRow _row;

        int IReadOnlyCollection<SudokuCell>.Count => 9;

        public SudokuCell this[int x] => _row[x];

        internal RowCells(SudokuRow row)
        {
            _row = row;
        }

        public Enumerator GetEnumerator() => new(_row);

        IEnumerator<SudokuCell> IEnumerable<SudokuCell>.GetEnumerator() => GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    #endregion

    #region IEquatable<SudokuRow>

    bool IEquatable<SudokuRow>.Equals(SudokuRow other) => this == other;

    public override bool Equals(object? obj)
        => obj is SudokuRow s && Equals(s);

    public override int GetHashCode()
        => Y;

    public static bool operator ==(SudokuRow left, SudokuRow right)
        => left.Puzzle == right.Puzzle && left.Y == right.Y;

    public static bool operator !=(SudokuRow left, SudokuRow right)
        => !(left == right);

    #endregion

    IEnumerator IEnumerable.GetEnumerator() => Cells.GetEnumerator();
}
