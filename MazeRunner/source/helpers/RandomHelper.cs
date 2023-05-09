using System;

namespace MazeRunner.Helpers;

public static class RandomHelper
{
    private static readonly Random _random = new();

    public static int Next(int minValue, int maxValue)
    {
        return _random.Next(minValue, maxValue);
    }

    public static bool RandomBoolean()
    {
        return _random.Next(0, 2) is 0;
    }
}
