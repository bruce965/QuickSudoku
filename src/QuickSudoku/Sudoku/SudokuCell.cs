using QuickSudoku.Abstractions;
using System.Collections;
using System.Diagnostics;

namespace QuickSudoku.Sudoku;

[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
public readonly struct SudokuCell : ICell, IEquatable<SudokuCell>, IEnumerable<IRegion>
{
    [DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
    public readonly struct CellCandidateValues : IValuesCollection<int>, IValuesCollection<object>
    {
        readonly SudokuCell _cell;

        internal ref SudokuDigits Ref => ref _cell.Puzzle._data[_cell.Index];

        public SudokuDigits Digits => Ref;

        internal string DebuggerDisplay
        {
            get
            {
                var self = this;
                return string.Join("", Enumerable.Range(1, 9).Select(v => self.Contains(v) ? v.ToString() : ""));
            }
        }

        internal CellCandidateValues(SudokuCell cell)
        {
            _cell = cell;
        }

        public void Set(int digit)
        {
            Debug.Assert(digit >= 1 && digit <= 9);

            Ref = (SudokuDigits)(1 << (digit - 1));
        }

        public void Add(int digit)
        {
            Debug.Assert(digit >= 1 && digit <= 9);

            Ref |= (SudokuDigits)(1 << (digit - 1));
        }

        public bool Contains(int digit)
        {
            if (digit < 1 || digit > 9)
                return false;

            return ((int)Ref & (1 << (digit - 1))) != 0;
        }

        public void Remove(int digit)
        {
            if (digit < 1 || digit > 9)
                return;

            Ref &= ~(SudokuDigits)(1 << (digit - 1));
        }

        public void Reset()
        {
            Ref = SudokuDigits.All;
        }

        #region IEnumerable<int>

        public struct Enumerator : IEnumerator<int>, IEnumerator<object>
        {
            readonly SudokuDigits _digits;
            int _value;

            public int Current
            {
                get
                {
                    Debug.Assert(_value >= 1 && _value <= 9);

                    return _value;
                }
            }

            object IEnumerator<object>.Current => Current;

            object IEnumerator.Current => Current;

            public Enumerator(SudokuDigits digits)
            {
                _digits = digits;
                _value = -1;
            }

            public bool MoveNext()
            {
                if (_value < 1)
                    _value = 1;

                for (; _value <= 9; _value++)
                    if (((int)_digits & (1 << (_value - 1))) != 0)
                        break;

                return _value < 9;
            }

            public void Reset()
            {
                _value = -1;
            }

            public void Dispose() { }
        }

        public Enumerator GetEnumerator() => new(Ref);

        IEnumerator<int> IEnumerable<int>.GetEnumerator() => GetEnumerator();

        #endregion

        #region IValuesCollection<object>

        void IValuesCollection<object>.Add(object item)
            => Add((int)item);

        bool IValuesCollection<object>.Contains(object item)
            => item is int v && Contains(v);

        void IValuesCollection<object>.Remove(object item)
        {
            if (item is int v)
                Remove(v);
        }

        IEnumerator<object> IEnumerable<object>.GetEnumerator() => GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion
    }

    public SudokuPuzzle Puzzle { get; }

    public SudokuCellIndex Index { get; }

    public SudokuRow Row => new(Puzzle, Index.Y);
    public SudokuColumn Column => new(Puzzle, Index.X);
    public SudokuSquare Square => new(Puzzle, Index.Square);

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
            Debug.Assert(value == null || (value >= 1 && value <= 9));

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
        Debug.Assert(puzzle != null);

        Puzzle = puzzle;
        Index = index;
    }

    #region ICell

    IPuzzle ICell.Puzzle => Puzzle;

    IEnumerable<IRegion> ICell.Regions => this;

    IEnumerable<object> ICell.LegalValues => SudokuPuzzle.LegalValues;

    IValuesCollection<object> ICell.CandidateValues => new CellCandidateValues(this);

    bool IEquatable<ICell>.Equals(ICell? other) => other is SudokuCell s && Equals(s);

    #endregion

    #region IEnumerable<IRegion>

    IEnumerator<IRegion> IEnumerable<IRegion>.GetEnumerator()
    {
        yield return Row;
        yield return Column;
        yield return Square;
    }

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<IRegion>)this).GetEnumerator();

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
}
