using MazeRunner.Components;
using MazeRunner.Extensions;
using MazeRunner.MazeBase.Tiles;
using MazeRunner.Wrappers;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace MazeRunner.MazeBase;

public class Maze : MazeRunnerGameComponent
{
    private readonly MazeTile[,] _skeleton;

    private readonly Dictionary<Cell, MazeTrap> _traps;

    private readonly Dictionary<Cell, MazeItem> _items;

    public (Cell Coords, Exit Exit) ExitInfo { get; set; }

    public List<MazeTileInfo> Components { get; init; }

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

        Components = new List<MazeTileInfo>();
    }

    public void InitializeComponentsList()
    {
        InitializeSkeletonComponentsList();
        InitializeTrapsComponentsList();
        InitializeItemsComponentsList();
        InitializeExitComponentsList();
    }

    public override void Draw(GameTime gameTime)
    {
        foreach (var component in Components)
        {
            component.Draw(gameTime);
        }
    }

    public override void Update(MazeRunnerGame game, GameTime gameTime)
    {
        foreach (var component in Components)
        {
            component.Update(game, gameTime);
        }
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
        var cellPosition = GetCellPosition(cell);
        var itemInfo = new MazeTileInfo(_items[cell], cellPosition);

        _items.Remove(cell);

        Components.Remove(itemInfo);
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

    private void InitializeSkeletonComponentsList()
    {
        for (int y = 0; y < _skeleton.GetLength(0); y++)
        {
            for (int x = 0; x < _skeleton.GetLength(1); x++)
            {
                var tile = _skeleton[y, x];
                var tilePosition = GetCellPosition(new Cell(x, y));

                Components.Add(new MazeTileInfo(tile, tilePosition));
            }
        }
    }

    private void InitializeTrapsComponentsList()
    {
        foreach (var (coords, trap) in _traps)
        {
            var trapPosition = GetCellPosition(coords);

            Components.Add(new MazeTileInfo(trap, trapPosition));
        }
    }

    private void InitializeItemsComponentsList()
    {
        foreach (var (coords, item) in _items)
        {
            var itemPosition = GetCellPosition(coords);

            Components.Add(new MazeTileInfo(item, itemPosition));
        }
    }

    private void InitializeExitComponentsList()
    {
        var exitPosition = GetCellPosition(ExitInfo.Coords);

        Components.Add(new MazeTileInfo(ExitInfo.Exit, exitPosition));
    }
}