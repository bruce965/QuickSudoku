namespace QuickSudoku.Sudoku;

[Flags]
public enum SudokuDigits : ushort
{
    None = 0,
    Digit1 = 1 << 0,
    Digit2 = 1 << 1,
    Digit3 = 1 << 2,
    Digit4 = 1 << 3,
    Digit5 = 1 << 4,
    Digit6 = 1 << 5,
    Digit7 = 1 << 6,
    Digit8 = 1 << 7,
    Digit9 = 1 << 8,
    All = Digit1 | Digit2 | Digit3 | Digit4 | Digit5 | Digit6 | Digit7 | Digit8 | Digit9,
}
