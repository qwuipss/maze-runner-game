namespace MazeRunner;

public static class DoubleDimArrayExtensions
{
    public static T[,] Transpose<T>(this T[,] array)
    {
        var transposedArray = new T[array.GetLength(1), array.GetLength(0)];

        for (int x = 0; x < transposedArray.GetLength(0); x++)
        {
            for (int y = 0; y < transposedArray.GetLength(1); y++)
            {
                transposedArray[x, y] = array[y, x];
            }
        }

        return transposedArray;
    }
}
