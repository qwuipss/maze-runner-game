#region Usings
using System;
#endregion

namespace MazeRunner.Helpers;

public static class RandomHelper
{
    private static readonly Random _random = new(17);

    public static bool RollChance(double chance)
    {
        return chance > _random.NextDouble();
    }

    public static int Next(int minValue, int maxValue)
    {
        return _random.Next(minValue, maxValue);
    }
}
