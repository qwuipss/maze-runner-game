namespace MazeRunner;

public static class Int32Extensions
{
    public static bool InRange(this int number, int bottom, int top)
    {
        return bottom <= number && number <= top;
    }
}
