using System.Collections;

namespace QuickSudoku.Sudoku.Extensions;

public static class CellsExtensions
{
    public readonly struct SolvedCells<T> : IEnumerable<SudokuCell>
        where T : struct, IReadOnlyList<SudokuCell>
    {
        public struct Enumerator : IEnumerator<SudokuCell>
        {
            readonly T _cells;
            int _index;

            internal Enumerator(T cells)
            {
                _cells = cells;
                _index = -1;
            }

            public readonly SudokuCell Current => _cells[_index];

            readonly object IEnumerator.Current => Current;

            public bool MoveNext()
            {
                while (++_index < _cells.Count && _cells[_index].IsSolved()) ;
                return _index < _cells.Count;
            }

            public void Reset()
            {
                _index = -1;
            }

            public readonly void Dispose() { }
        }

        readonly T _cells;

        internal SolvedCells(T cells)
        {
            _cells = cells;
        }

        public Enumerator GetEnumerator() => new(_cells);

        IEnumerator<SudokuCell> IEnumerable<SudokuCell>.GetEnumerator() => GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public readonly struct UnsolvedCells<T> : IEnumerable<SudokuCell>
        where T : struct, IReadOnlyList<SudokuCell>
    {
        public struct Enumerator : IEnumerator<SudokuCell>
        {
            readonly T _cells;
            int _index;

            internal Enumerator(T cells)
            {
                _cells = cells;
                _index = -1;
            }

            public readonly SudokuCell Current => _cells[_index];

            readonly object IEnumerator.Current => Current;

            public bool MoveNext()
            {
                while (++_index < _cells.Count && !_cells[_index].IsSolved()) ;
                return _index < _cells.Count;
            }

            public void Reset()
            {
                _index = -1;
            }

            public readonly void Dispose() { }
        }

        readonly T _cells;

        internal UnsolvedCells(T cells)
        {
            _cells = cells;
        }

        public Enumerator GetEnumerator() => new(_cells);

        IEnumerator<SudokuCell> IEnumerable<SudokuCell>.GetEnumerator() => GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    /// <inheritdoc cref="QuickSudoku.Extensions.CellsExtensions.Solved"/>
    public static SolvedCells<T> Solved<T>(this T cells)
        where T : struct, IReadOnlyList<SudokuCell>
        => new(cells);

    /// <inheritdoc cref="QuickSudoku.Extensions.CellsExtensions.Unsolved"/>
    public static UnsolvedCells<T> Unsolved<T>(this T cells)
        where T : struct, IReadOnlyList<SudokuCell>
        => new(cells);
}
