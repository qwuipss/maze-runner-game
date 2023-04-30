#region Usings
using MazeRunner.MazeBase.Tiles;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
#endregion

namespace MazeRunner.MazeBase;

public class Maze
{
    public ImmutableDictionary<Cell, MazeTrap> Traps
    {
        get
        {
            return _traps.ToImmutableDictionary();
        }
    }

    public MazeTile[,] Skeleton { get; init; }

    private readonly Dictionary<Cell, MazeTrap> _traps;

    public Maze(MazeTile[,] skeleton)
    {
        Skeleton = skeleton;

        _traps = new();
    }

    public void InsertTrap(MazeTrap trap, Cell cell)
    {
        _traps.Add(cell, trap);
    }

    public bool IsFloor(Cell cell)
    {
        return Skeleton[cell.Y, cell.X].TileType is TileType.Floor
           && !_traps.ContainsKey(cell);
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
                if (_traps.TryGetValue(new Cell(x, y), out var trap))
                {
                    writer.Write((char)trap.TileType);
                }
                else
                {
                    writer.Write((char)Skeleton[y, x].TileType);
                }
            }

            writer.Write(Environment.NewLine);
        }
    }
}
