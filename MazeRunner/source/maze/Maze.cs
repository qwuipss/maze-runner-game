#region Usings
using System;
using System.IO;
#endregion

namespace MazeRunner;

public class Maze
{
    public int Width { get; init; }
    public int Height { get; init; }

    private readonly MazeTile[,] _cells;

    public Maze(MazeTile[,] cells)
    {
        Height = cells.GetLength(0);
        Width = cells.GetLength(1);

        _cells = cells;
    }

    public MazeTile this[int x, int y]
    {
        get
        {
            return _cells[y, x];
        }
    }

    public void LoadToFile(FileInfo fileInfo)
    {
        using var writer = new StreamWriter(fileInfo.FullName);

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                writer.Write((char)this[x, y].CellType);
            }

            writer.Write(Environment.NewLine);
        }
    }
}
