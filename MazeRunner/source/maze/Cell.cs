using MazeRunner.Extensions;

namespace MazeRunner.MazeBase;

public readonly record struct Cell(int X, int Y)
{
    public bool InBoundsOf<T>(T[,] mazeSkeleton)
    {
        return X.InRange(0, mazeSkeleton.GetLength(1) - 1) && Y.InRange(0, mazeSkeleton.GetLength(0) - 1);
    }

    public bool IsCornerOf<T>(T[,] mazeSkeleton)
    {
        var width = mazeSkeleton.GetLength(1);
        var height = mazeSkeleton.GetLength(0);

        return (X is 0 && Y is 0)
            || (X is 0 && Y == height - 1)
            || (X == width - 1 && Y is 0)
            || (X == width - 1 && Y == height - 1);
    }
}
