namespace QuickSudoku.Generators;

/// <summary>
/// Level of symmetry in the generated puzzle board.
/// </summary>
[Flags]
public enum SudokuSymmetry
{
    /// <summary>
    /// Symmetry will not be considered while generating the puzzle board.
    /// </summary>
    None = 0,
    /// <summary>
    /// The generated puzzle board will be horizontally symmetric.
    /// </summary>
    Horizontal = 1,
    /// <summary>
    /// The generated puzzle board will be verticlly symmetric.
    /// </summary>
    Vertical = 2,
    /// <summary>
    /// The generated puzzle board will be horizontally and vertically symmetric.
    /// </summary>
    Full = Horizontal | Vertical,
    /// <summary>
    /// Treat other symmetry flags as loose,
    /// some cells are allowed to ignore the symmetry.
    /// </summary>
    Loose = 4,
    /// <summary>
    /// The generated puzzle board will be mostly horizontally symmetric,
    /// but some cells are allowed to break the symmetry.
    /// </summary>
    PreferHorizontal = Loose | Horizontal,
    /// <summary>
    /// The generated puzzle board will be mostly vertically symmetric,
    /// but some cells are allowed to break the symmetry.
    /// </summary>
    PreferVertical = Loose | Vertical,
    /// <summary>
    /// The generated puzzle board will be mostly horizontally and vertically symmetric,
    /// but some cells are allowed to break the symmetry.
    /// </summary>
    PreferFull = PreferHorizontal | PreferVertical,
    
}
