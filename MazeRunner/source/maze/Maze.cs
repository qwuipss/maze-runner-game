using MazeRunner.Components;
using MazeRunner.Drawing;
using MazeRunner.Extensions;
using MazeRunner.MazeBase.Tiles;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;

namespace MazeRunner.MazeBase;

public class Maze : MazeRunnerGameComponent
{
    private readonly MazeTile[,] _skeleton;

    private readonly Dictionary<Cell, MazeTrap> _traps;

    private readonly Dictionary<Cell, MazeItem> _items;

    public (Cell Coords, Exit Exit) ExitInfo { get; set; }

    public ImmutableDoubleDimArray<MazeTile> Skeleton
    {
        get
        {
            return _skeleton.ToImmutableDoubleDimArray();
        }
    }

    public ImmutableDictionary<Cell, MazeTrap> Traps
    {
        get
        {
            return _traps.ToImmutableDictionary();
        }
    }

    public ImmutableDictionary<Cell, MazeItem> Items
    {
        get
        {
            return _items.ToImmutableDictionary();
        }
    }

    public Maze(MazeTile[,] skeleton)
    {
        _skeleton = skeleton;

        _traps = new Dictionary<Cell, MazeTrap>();
        _items = new Dictionary<Cell, MazeItem>();
    }

    public override void Draw(GameTime gameTime)
    {
        Drawer.DrawMaze(this, gameTime);
    }

    public override void Update(MazeRunnerGame game, GameTime gameTime)
    {
    }

    public Vector2 GetCellPosition(Cell cell)
    {
        int posX = 0, posY = 0;

        for (int y = 0; y < cell.Y; y++)
        {
            posY += _skeleton[y, cell.X].FrameHeight;
        }

        for (int x = 0; x < cell.X; x++)
        {
            posX += _skeleton[cell.Y, x].FrameWidth;
        }

        return new Vector2(posX, posY);
    }

    public void InsertTrap(MazeTrap trap, Cell cell)
    {
        _traps.Add(cell, trap);
    }

    public void InsertItem(MazeItem item, Cell cell)
    {
        _items.Add(cell, item);
    }

    public void RemoveItem(Cell cell)
    {
        _items.Remove(cell);
    }

    public void InsertExit(Exit exit, Cell coords)
    {
        ExitInfo = (coords, exit);

        _skeleton[coords.Y, coords.X] = new Floor();
    }

    public bool IsFloor(Cell cell)
    {
        return Skeleton[cell.Y, cell.X].TileType is TileType.Floor
           && !_traps.ContainsKey(cell)
           && !_items.ContainsKey(cell)
           && cell != ExitInfo.Coords;
    }

    public bool IsWall(Cell cell)
    {
        return Skeleton[cell.Y, cell.X].TileType is TileType.Wall;
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
