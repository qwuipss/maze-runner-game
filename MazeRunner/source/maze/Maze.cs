#region Usings
using MazeRunner.MazeBase.Tiles;
using System;
using System.IO;
#endregion

namespace MazeRunner.MazeBase;

public class Maze
{
    public int Width { get; init; }
    public int Height { get; init; }

    public readonly MazeTile[,] Tiles;

    public Maze(MazeTile[,] cells)
    {
        Height = cells.GetLength(0);
        Width = cells.GetLength(1);

        Tiles = cells;
    }

    public MazeTile this[int x, int y]
    {
        get
        {
            return Tiles[y, x];
        }
        set
        {
            Tiles[y, x] = value;
        }
    }

    public void LoadToFile(FileInfo fileInfo)
    {
        using var writer = new StreamWriter(fileInfo.FullName);

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                writer.Write((char)this[x, y].TileType);
            }

            writer.Write(Environment.NewLine);
        }
    }
}
