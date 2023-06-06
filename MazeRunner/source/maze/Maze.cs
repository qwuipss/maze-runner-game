using MazeRunner.Components;
using MazeRunner.GameBase;
using MazeRunner.GameBase.States;
using MazeRunner.MazeBase.Tiles;
using MazeRunner.Sprites;
using MazeRunner.Sprites.States;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.CompilerServices;
using static MazeRunner.Content.Textures;

namespace MazeRunner.MazeBase;

public class Maze : MazeRunnerGameComponent
{
    private const float ExitOpenDistanceCoeff = 2;

    private readonly Dictionary<Cell, MazeTile> _trapsInfo;

    private readonly Dictionary<Cell, MazeTile> _itemsInfo;

    private readonly Dictionary<Cell, MazeTile> _marksInfo;

    private Hero _hero;

    private float _exitOpenDistance;

    private readonly List<MazeTile> _mazeTiles;

    public IReadOnlyCollection<MazeTile> Components => _mazeTiles.AsReadOnly();

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
        _marksInfo = new Dictionary<Cell, MazeTile>();

        _mazeTiles = new List<MazeTile>();
    }

    public void Initialize(Hero hero)
    {
        _hero = hero;

        _exitOpenDistance = _hero.FrameSize * ExitOpenDistanceCoeff;

        Position = _hero.Position;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 GetCellPosition(Cell cell)
    {
        var frameSize = GameConstants.AssetsFrameSize;

        var framePosX = frameSize * cell.X;
        var framePosY = frameSize * cell.Y;

        return new Vector2(framePosX, framePosY);
    }

    public static Cell GetCellByPosition(Vector2 position)
    {
        var cellSize = GameConstants.AssetsFrameSize;

        var cell = new Cell((int)position.X / cellSize, (int)position.Y / cellSize);

        return cell;
    }

    public void InitializeComponentsList()
    {
        void InitializeSkeletonComponentsList()
        {
            for (int y = 0; y < Skeleton.GetLength(0); y++)
            {
                for (int x = 0; x < Skeleton.GetLength(1); x++)
                {
                    var mazeTile = Skeleton[y, x];

                    mazeTile.Position = GetCellPosition(new Cell(x, y));

                    _mazeTiles.Add(mazeTile);
                }
            }
        }

        void InitializeTrapsComponentsList()
        {
            foreach (var (cell, trap) in _trapsInfo)
            {
                trap.Position = GetCellPosition(cell);

                _mazeTiles.Add(trap);
            }
        }

        void InitializeItemsComponentsList()
        {
            foreach (var (cell, item) in _itemsInfo)
            {
                item.Position = GetCellPosition(cell);

                _mazeTiles.Add(item);
            }
        }

        void InitializeExitComponentsList()
        {
            ExitInfo.Exit.Position = GetCellPosition(ExitInfo.Cell);

            _mazeTiles.Add(ExitInfo.Exit);
        }

        InitializeSkeletonComponentsList();
        InitializeTrapsComponentsList();
        InitializeItemsComponentsList();
        InitializeExitComponentsList();
    }

    public override void Draw(GameTime gameTime)
    {
        foreach (var mazeTile in _mazeTiles)
        {
            mazeTile.Draw(gameTime);
        }
    }

    public override void Update(GameTime gameTime)
    {
        if (_hero is null)
        {
            return;
        }

        if (NeedOpenExit())
        {
            ExitInfo.Exit.Open();
        }

        var heroCell = SpriteBaseState.GetSpriteCell(_hero);
        var updatableArea = GameBaseState.GetUpdatableArea(heroCell, Skeleton);

        for (int y = updatableArea.Top; y < updatableArea.Bottom; y++)
        {
            for (int x = updatableArea.Left; x < updatableArea.Right; x++)
            {
                Skeleton[y, x].Update(gameTime);
            }
        }

        //foreach (var mazeTile in _mazeTiles)
        //{
        //    if (_hero is null)
        //    {
        //        mazeTile.Update(gameTime);

        //        return;
        //    }

        //    if (IsInArea(updatableArea, mazeTile))
        //    {
        //        mazeTile.Update(gameTime);
        //    }
        //}

        Position = _hero.Position;
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

    public void InsertMark(MazeMark mark, Cell cell)
    {
        _marksInfo.Add(cell, mark);

        _mazeTiles.Add(mark);
    }

    public bool CanInsertMark(Cell cell)
    {
        return !_marksInfo.ContainsKey(cell) && !_trapsInfo.ContainsKey(cell);
    }

    public void RemoveItem(Cell cell)
    {
        var cellPosition = GetCellPosition(cell);

        var itemTile = _mazeTiles.Where(mazeTile => mazeTile.Position == cellPosition && mazeTile is MazeItem).Single();

        _itemsInfo.Remove(cell);
        _mazeTiles.Remove(itemTile);
    }

    private bool NeedOpenExit()
    {
        return IsKeyCollected
         && !ExitInfo.Exit.IsOpened
         && _hero is not null
         && Vector2.Distance(_hero.Position, GetCellPosition(ExitInfo.Cell)) < _exitOpenDistance;
    }
}