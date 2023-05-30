using MazeRunner.Components;
using MazeRunner.MazeBase.Tiles;
using MazeRunner.Sprites;
using MazeRunner.Wrappers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.CompilerServices;

namespace MazeRunner.MazeBase;

public class Maze : MazeRunnerGameComponent
{
    private const float ExitOpenDistanceCoeff = 2;

    private readonly Dictionary<Cell, MazeTile> _trapsInfo;

    private readonly Dictionary<Cell, MazeTile> _itemsInfo;

    private Hero _hero;

    private float _exitOpenDistance;

    private readonly List<MazeTileInfo> _components;

    public IReadOnlyCollection<MazeTileInfo> Components => _components.AsReadOnly();

    public ImmutableDictionary<Cell, MazeTile> TrapsInfo => _trapsInfo.ToImmutableDictionary();

    public ImmutableDictionary<Cell, MazeTile> ItemsInfo => _itemsInfo.ToImmutableDictionary();

    public (Cell Cell, Exit Exit) ExitInfo { get; set; }

    public MazeTile[,] Skeleton { get; init; }

    public bool IsKeyCollected { get; set; }

    public Maze(MazeTile[,] skeleton)
    {
        Skeleton = skeleton;

        _trapsInfo = new Dictionary<Cell, MazeTile>();
        _itemsInfo = new Dictionary<Cell, MazeTile>();

        _components = new List<MazeTileInfo>();
    }

    public void PostInitialize(Hero hero)
    {
        _hero = hero;

        _exitOpenDistance = _hero.FrameSize * ExitOpenDistanceCoeff;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 GetIndependentCellPosition(MazeTile tile, Cell cell)
    {
        var framePosX = tile.FrameSize * cell.X;
        var framePosY = tile.FrameSize * cell.Y;

        return new Vector2(framePosX, framePosY);
    }

    public void InitializeComponentsList()
    {
        void InitializeSkeletonComponentsList()
        {
            for (int y = 0; y < Skeleton.GetLength(0); y++)
            {
                for (int x = 0; x < Skeleton.GetLength(1); x++)
                {
                    var tile = Skeleton[y, x];
                    var tilePosition = GetCellPosition(new Cell(x, y));

                    _components.Add(new MazeTileInfo(tile, tilePosition));
                }
            }
        }

        void InitializeTrapsComponentsList()
        {
            foreach (var (cell, trap) in _trapsInfo)
            {
                var trapPosition = GetCellPosition(cell);

                _components.Add(new MazeTileInfo(trap, trapPosition));
            }
        }

        void InitializeItemsComponentsList()
        {
            foreach (var (cell, item) in _itemsInfo)
            {
                var itemPosition = GetCellPosition(cell);

                _components.Add(new MazeTileInfo(item, itemPosition));
            }
        }

        void InitializeExitComponentsList()
        {
            var exitPosition = GetCellPosition(ExitInfo.Cell);

            _components.Add(new MazeTileInfo(ExitInfo.Exit, exitPosition));
        }

        InitializeSkeletonComponentsList();
        InitializeTrapsComponentsList();
        InitializeItemsComponentsList();
        InitializeExitComponentsList();
    }

    public override void Draw(GameTime gameTime)
    {
        foreach (var component in _components)
        {
            component.Draw(gameTime);
        }
    }

    public override void Update(GameTime gameTime)
    {
        if (NeedOpenExit())
        {
            ExitInfo.Exit.Open();
        }

        foreach (var component in _components)
        {
            component.Update(gameTime);
        }
    }

    public bool IsFloor(Cell cell)
    {
        return Skeleton[cell.Y, cell.X].TileType is TileType.Floor
           && cell != ExitInfo.Cell
           && !_trapsInfo.ContainsKey(cell)
           && !_itemsInfo.ContainsKey(cell);
    }

    public bool IsWall(Cell cell)
    {
        return Skeleton[cell.Y, cell.X].TileType is TileType.Wall;
    }

    public int GetTileCount(Func<Cell, bool> tileSelector)
    {
        var tileCount = 0;

        for (int y = 0; y < Skeleton.GetLength(0); y++)
        {
            for (int x = 0; x < Skeleton.GetLength(1); x++)
            {
                if (tileSelector.Invoke(new Cell(x, y)))
                {
                    tileCount++;
                }
            }
        }

        return tileCount;
    }

    public Vector2 GetCellPosition(Cell cell)
    {
        var tile = Skeleton[cell.Y, cell.X];

        return GetIndependentCellPosition(tile, cell);
    }

    public Cell GetCellByPosition(Vector2 position)
    {
        var cellSize = Skeleton[0, 0].FrameSize;

        var cell = new Cell((int)position.X / cellSize, (int)position.Y / cellSize);

        return cell;
    }

    public void InsertTrap(MazeTrap trap, Cell cell)
    {
        _trapsInfo.Add(cell, trap);
    }

    public void InsertExit(Exit exit, Cell cell)
    {
        ExitInfo = (cell, exit);

        Skeleton[cell.Y, cell.X] = new Floor();
    }

    public void InsertItem(MazeItem item, Cell cell)
    {
        _itemsInfo.Add(cell, item);
    }

    public void RemoveItem(Cell cell)
    {
        var cellPosition = GetCellPosition(cell);

        var itemInfo = _components.Where(mti => mti.Position == cellPosition && mti.MazeTile is MazeItem).Single();

        _itemsInfo.Remove(cell);
        _components.Remove(itemInfo);
    }

    private bool NeedOpenExit()
    {
        return IsKeyCollected
         && !ExitInfo.Exit.IsOpened
         && _hero is not null
         && Vector2.Distance(_hero.Position, GetCellPosition(ExitInfo.Cell)) < _exitOpenDistance;
    }
}