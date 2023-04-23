namespace MazeRunner;

public readonly record struct Cell(int X, int Y)
{
    public bool InBoundsOf(MazeTile[,] cells)
    {
        return X.InRange(0, cells.GetLength(0) - 1) && Y.InRange(0, cells.GetLength(1) - 1);
    }
}
