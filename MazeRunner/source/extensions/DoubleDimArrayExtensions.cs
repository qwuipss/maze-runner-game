namespace MazeRunner.Extensions;

public static class DoubleDimArrayExtensions
{
    public static T[] ToLinear<T>(this T[,] array)
    {
        var arraySize = array.GetLength(0) * array.GetLength(1);
        var linearArray = new T[arraySize];

        var index = 0;

        foreach (var item in array)
        {
            linearArray[index] = item;
            index++;
        }

        return linearArray;
    }
}