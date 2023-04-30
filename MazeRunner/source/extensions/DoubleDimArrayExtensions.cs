namespace MazeRunner.Extensions;

public static class DoubleDimArrayExtensions
{
    public static ImmutableDoubleDimArray<T> ToImmutableDoubleDimArray<T>(this T[,] array)
    {
        return new ImmutableDoubleDimArray<T>(array);
    }
}
