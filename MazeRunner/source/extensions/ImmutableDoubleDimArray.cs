#region Usings
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
#endregion

namespace MazeRunner.Extensions;

public class ImmutableDoubleDimArray<T> : IEnumerable<T>
{
    private readonly T[,] _array;

    public ImmutableDoubleDimArray(T[,] array)
    {
        _array = array;
    }

    public T this[int y, int x]
    {
        get
        {
            return _array[y, x];
        }
    }

    public IEnumerator<T> GetEnumerator()
    {
        foreach (var item in _array)
        {
            yield return item;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int GetLength(int dimension)
    {
        return _array.GetLength(dimension);
    }
}
