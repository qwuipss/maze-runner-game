using MazeRunner.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MazeRunner.Helpers;

public static class RandomHelper
{
    private static readonly Random _random;

    static RandomHelper()
    {
        _random = new Random();
    }

    public static int Next(int minValue, int maxValue)
    {
        return _random.Next(minValue, maxValue);
    }

    public static bool RandomBoolean()
    {
        return _random.Next(0, 2) is 0;
    }

    public static T Choice<T>(IList<T> collection)
    {
        return collection[_random.Next(0, collection.Count)];
    }

    public static T Choice<T>(IDictionary<T, float> itemChancePairs)
    {
        var items = itemChancePairs.Keys.ToList();
        var chances = itemChancePairs.Values;

        if (chances.Any(chance => chance <= 0))
        {
            throw new ArgumentOutOfRangeException(nameof(itemChancePairs), $"numbers in {nameof(chances)} should be more than 0");
        }

        var chancesSum = chances.Sum();

        if (chancesSum - 1 > float.Epsilon)
        {
            throw new ArgumentOutOfRangeException(nameof(itemChancePairs), $"sum of {nameof(chances)} should be equal to 1 but was {chancesSum}");
        }

        var rangedChances = new List<float>() { 0 };

        foreach (var chance in chances)
        {
            rangedChances.Add(chance + rangedChances.Last());
        }

        var randomFloat = (float)_random.NextDouble();

        for (int i = 0; i < rangedChances.Count - 1; i++)
        {
            if (randomFloat.InRange(rangedChances[i], rangedChances[i + 1]))
            {
                return items[i];
            }
        }

        throw new NotImplementedException();
    }
}
