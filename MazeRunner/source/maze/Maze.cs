#region Usings
using MazeRunner.MazeBase.Tiles;
using System;
using System.IO;
#endregion

namespace MazeRunner.MazeBase;

public class Maze
{
    public readonly MazeTile[,] Tiles;

    public Maze(MazeTile[,] cells)
    {
        Tiles = cells;
    }

    public void LoadToFile(FileInfo fileInfo)
    {
        using var writer = new StreamWriter(fileInfo.FullName);

        for (int y = 0; y < Tiles.GetLength(0); y++)
        {
            for (int x = 0; x < Tiles.GetLength(1); x++)
            {
                writer.Write((char)Tiles[y, x].TileType);
            }

            writer.Write(Environment.NewLine);
        }
    }
}
