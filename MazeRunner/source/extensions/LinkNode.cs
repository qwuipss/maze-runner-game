using System.Collections;
using System.Collections.Generic;

namespace MazeRunner.Extensions;

public class LinkNode<T> : IEnumerable<T>
{
    public int LinkNumber { get; init; }

    public T Value { get; init; }

#nullable enable
    public LinkNode<T>? Previous { get; set; }

    public LinkNode(T item, LinkNode<T>? previous)
    {
        Value = item;
        Previous = previous;
        LinkNumber = previous?.LinkNumber + 1 ?? 0;
    }
#nullable disable

    public LinkNode()
    {
    }

    public IEnumerator<T> GetEnumerator()
    {
        var currentNode = this;

        while (currentNode is not null)
        {
            yield return currentNode.Value;

            currentNode = currentNode.Previous;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
