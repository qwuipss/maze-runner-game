namespace MazeRunner.Extensions;

public static class SingleExtensions
{
    public static bool InRange(this float number, float bottom, float top)
    {
        return bottom <= number && number <= top;
    }
}
