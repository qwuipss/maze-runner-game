#region Usings
using System;
using System.IO;
#endregion

namespace MazeRunner;

public class Maze
{
    public int Width { get; init; }
    public int Height { get; init; }

    private readonly CellType[,] _cells;

    public Maze(CellType[,] cells)
    {
        Width = cells.GetLength(0);
        Height = cells.GetLength(1);

        _cells = cells;
    }

    public void LoadToFile(FileInfo fileInfo)
    {
        using var writer = new StreamWriter(fileInfo.FullName);

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                writer.Write((char)_cells[x, y]);
            }

            writer.Write(Environment.NewLine);
        }
    }
}
