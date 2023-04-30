namespace MazeRunner.MazeBase;

public readonly record struct Cell(int X, int Y)
{
    public bool InBoundsOf<T>(T[,] array)
    {
        return X.InRange(0, array.GetLength(1) - 1) && Y.InRange(0, array.GetLength(0) - 1);
    }
}
