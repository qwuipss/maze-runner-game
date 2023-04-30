#region Usings
using MazeRunner.Extensions;
#endregion

namespace MazeRunner.MazeBase;

public readonly record struct Cell(int X, int Y)
{
    public bool InBoundsOf<T>(ImmutableDoubleDimArray<T> mazeSkeleton)
    {
        return X.InRange(0, mazeSkeleton.GetLength(1) - 1) && Y.InRange(0, mazeSkeleton.GetLength(0) - 1);
    }
}
