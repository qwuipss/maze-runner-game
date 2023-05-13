using MazeRunner.Extensions;
using MazeRunner.MazeBase.Tiles;
using MazeRunner.Wrappers;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;

namespace MazeRunner.MazeBase;

public class Maze
{
    private readonly MazeTile[,] _skeleton;

    private readonly Dictionary<Cell, MazeTrap> _trapsInfo;

    private readonly Dictionary<Cell, MazeItem> _itemsInfo;

    private readonly List<MazeTileInfo> _components;

    public (Cell Cell, Exit Exit) ExitInfo { get; set; }

    public IReadOnlyCollection<MazeTileInfo> Components
    {
        get
        {
            return _components.AsReadOnly();
        }
    }

    public ImmutableDoubleDimArray<MazeTile> Skeleton
    {
        get
        {
            return _skeleton.ToImmutableDoubleDimArray();
        }
    }

    public ImmutableDictionary<Cell, MazeTrap> TrapsInfo
    {
        get
        {
            return _trapsInfo.ToImmutableDictionary();
        }
    }

    public ImmutableDictionary<Cell, MazeItem> ItemsInfo
    {
        get
        {
            return _itemsInfo.ToImmutableDictionary();
        }
    }

    public Maze(MazeTile[,] skeleton)
    {
        _skeleton = skeleton;

        _trapsInfo = new Dictionary<Cell, MazeTrap>();
        _itemsInfo = new Dictionary<Cell, MazeItem>();

        _components = new List<MazeTileInfo>();
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
        InitializeSkeletonComponentsList();
        InitializeTrapsComponentsList();
        InitializeItemsComponentsList();
        InitializeExitComponentsList();
    }

    public void Draw(GameTime gameTime)
    {
        foreach (var component in _components)
        {
            component.Draw(gameTime);
        }
    }

    public void Update(MazeRunnerGame game, GameTime gameTime)
    {
        foreach (var component in _components)
        {
            component.Update(game, gameTime);
        }
    }

    #region Utilities
    public bool IsFloor(Cell cell)
    {
        return _skeleton[cell.Y, cell.X].TileType is TileType.Floor
           && cell != ExitInfo.Cell
           && !_trapsInfo.ContainsKey(cell)
           && !_itemsInfo.ContainsKey(cell);
    }

    public bool IsWall(Cell cell)
    {
        return _skeleton[cell.Y, cell.X].TileType is TileType.Wall;
    }

    public int GetFloorsCount()
    {
        var floorsCount = 0;

        for (int y = 0; y < _skeleton.GetLength(0); y++)
        {
            for (int x = 0; x < _skeleton.GetLength(1); x++)
            {
                if (IsFloor(new Cell(x, y)))
                {
                    floorsCount++;
                }
            }
        }

        return floorsCount;
    }

    public Vector2 GetCellPosition(Cell cell)
    {
        var tile = _skeleton[cell.Y, cell.X];

        return GetIndependentCellPosition(tile, cell);
    }
    #endregion

    #region Inserters
    public void InsertTrap(MazeTrap trap, Cell cell)
    {
        _trapsInfo.Add(cell, trap);
    }

    public void InsertExit(Exit exit, Cell cell)
    {
        ExitInfo = (cell, exit);

        _skeleton[cell.Y, cell.X] = new Floor();
    }

    public void InsertItem(MazeItem item, Cell cell)
    {
        _itemsInfo.Add(cell, item);
    }

    public void RemoveItem(Cell cell)
    {
        var cellPosition = GetCellPosition(cell);
        var itemInfo = new MazeTileInfo(_itemsInfo[cell], cellPosition);

        _itemsInfo.Remove(cell);

        _components.Remove(itemInfo);
    }
    #endregion

    #region ComponentsListInitializers
    private void InitializeSkeletonComponentsList()
    {
        for (int y = 0; y < _skeleton.GetLength(0); y++)
        {
            for (int x = 0; x < _skeleton.GetLength(1); x++)
            {
                var tile = _skeleton[y, x];
                var tilePosition = GetCellPosition(new Cell(x, y));

                _components.Add(new MazeTileInfo(tile, tilePosition));
            }
        }
    }

    private void InitializeTrapsComponentsList()
    {
        foreach (var (cell, trap) in _trapsInfo)
        {
            var trapPosition = GetCellPosition(cell);

            _components.Add(new MazeTileInfo(trap, trapPosition));
        }
    }

    private void InitializeItemsComponentsList()
    {
        foreach (var (cell, item) in _itemsInfo)
        {
            var itemPosition = GetCellPosition(cell);

            _components.Add(new MazeTileInfo(item, itemPosition));
        }
    }

    private void InitializeExitComponentsList()
    {
        var exitPosition = GetCellPosition(ExitInfo.Cell);

        _components.Add(new MazeTileInfo(ExitInfo.Exit, exitPosition));
    }
    #endregion
}