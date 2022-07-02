using System.Diagnostics;

namespace QuickSudoku.Sudoku;

[DebuggerDisplay($"{{{nameof(DebuggerDisplay)},nq}}")]
public readonly struct SudokuCellIndex : IEquatable<SudokuCellIndex>
{
    public int Index { get; }
    
    public int X => Index % 9;
    public int Y => Index / 9;

    public int Square => (Y / 3) * 3 + X / 3;

    internal string DebuggerDisplay
        => $"[{X}, {Y}]";

    public SudokuCellIndex(int index)
    {
        Debug.Assert(index >= 0 && index < 81);

        Index = index;
    }

    public SudokuCellIndex(int x, int y)
    {
        Debug.Assert(x >= 0 && x < 9);
        Debug.Assert(y >= 0 && y < 9);

        Index = x + y * 9;
    }

    public static implicit operator int(SudokuCellIndex index) => index.Index;
    public static implicit operator SudokuCellIndex(int index) => new(index);

    public static implicit operator (int X, int Y)(SudokuCellIndex index) => (index.X, index.Y);
    public static implicit operator SudokuCellIndex((int X, int Y) index) => new(index.X, index.Y);

    #region IEquatable<SudokuCellIndex>

    bool IEquatable<SudokuCellIndex>.Equals(SudokuCellIndex other)
        => this == other;

    public override bool Equals(object? obj)
        => obj is SudokuCellIndex s && Equals(s);

    public override int GetHashCode()
        => Index;

    public static bool operator ==(SudokuCellIndex left, SudokuCellIndex right)
        => left.Index == right.Index;

    public static bool operator !=(SudokuCellIndex left, SudokuCellIndex right)
        => !(left == right);

    #endregion
}
