#region Usings
using System;
#endregion

namespace MazeRunner;

public static class RandomHelper
{
    private static readonly Random _random = new();

    public static bool Roll(double value)
    {
        return value > _random.NextDouble();
    }
}
