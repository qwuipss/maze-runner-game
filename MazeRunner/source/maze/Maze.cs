#region Usings
using MazeRunner.MazeBase.Tiles;
using System;
using System.Collections.Generic;
using System.IO;
#endregion

namespace MazeRunner.MazeBase;

public class Maze
{
    public readonly MazeTile[,] Skeleton;
    public readonly Dictionary<Cell, MazeTrap> Traps;

    public Maze(MazeTile[,] skeleton)
    {
        Skeleton = skeleton;
        Traps = new();
    }

    public void InsertTrap(MazeTrap trap, Cell cell)
    {
        Traps.Add(cell, trap);
    }

    public bool IsFloor(Cell cell)
    {
        return Skeleton[cell.Y, cell.X].TileType is TileType.Floor
            && !Traps.ContainsKey(cell);
    }

    public int GetFloorsCount()
    {
        var floorsCount = 0;

        for (int y = 0; y < Skeleton.GetLength(0); y++)
        {
            for (int x = 0; x < Skeleton.GetLength(1); x++)
            {
                if (IsFloor(new Cell(x, y)))
                {
                    floorsCount++;
                }
            }
        }

        return floorsCount;
    }

    public void LoadToFile(FileInfo fileInfo)
    {
        using var writer = new StreamWriter(fileInfo.FullName);

        for (int y = 0; y < Skeleton.GetLength(0); y++)
        {
            for (int x = 0; x < Skeleton.GetLength(1); x++)
            {
                writer.Write((char)Skeleton[y, x].TileType);
            }

            writer.Write(Environment.NewLine);
        }
    }
}
