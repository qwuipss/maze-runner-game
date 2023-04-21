using System;

namespace MazeRunner;

public readonly record struct MazeCell(int X, int Y, CellType CellType = CellType.Empty)
{
    public bool InBoundsOf(MazeCell[,] cells)
    {
        return X.InRange(0, cells.GetLength(0) - 1) && Y.InRange(0, cells.GetLength(1) - 1);
    }
}
