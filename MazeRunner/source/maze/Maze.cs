using MazeRunner.Components;
using MazeRunner.GameBase;
using MazeRunner.GameBase.States;
using MazeRunner.Helpers;
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
    private const int MazeTileUpdateAreaWidthRadius = GameRunningState.UpdateAreaWidthRadius;

    private const int MazeTileUpdateAreaHeightRadius = GameRunningState.UpdateAreaHeightRadius;

    private const int MazeTileCollideAreaRadius = 2;

    private const float ExitOpenDistanceCoeff = 2;

    private readonly Dictionary<Cell, MazeTile> _hoverTilesInfo;

    private Hero _hero;

    private float _exitOpenDistance;

    private readonly HashSet<MazeTile> _mazeTiles;

    public ImmutableHashSet<MazeTile> Components => _mazeTiles.ToImmutableHashSet();

    public ImmutableDictionary<Cell, MazeTile> HoverTilesInfo => _hoverTilesInfo.ToImmutableDictionary();

    public (Cell Cell, Exit Exit) ExitInfo { get; set; }

    public MazeTile[,] Skeleton { get; init; }

    public bool IsKeyCollected { get; set; }

    public Maze(MazeTile[,] skeleton)
    {
        Skeleton = skeleton;

        _mazeTiles = new HashSet<MazeTile>();
        _hoverTilesInfo = new Dictionary<Cell, MazeTile>();
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

    public override void Draw(GameTime gameTime)
    {
        foreach (var mazeTile in _mazeTiles)
        {
            mazeTile.Draw(gameTime);
        }
    }

    public override void Update(GameTime gameTime)
    {
        void UpdateAll()
        {
            foreach (var mazeTile in _mazeTiles)
            {
                mazeTile.Update(gameTime);
            }
        }

        if (_hero is null)
        {
            UpdateAll();

            return;
        }

        if (NeedOpenExit())
        {
            ExitInfo.Exit.Open();
        }

        var heroCell = SpriteBaseState.GetSpriteCell(_hero);
        var updatableArea = HitBoxHelper.GetArea(heroCell, MazeTileUpdateAreaWidthRadius, MazeTileUpdateAreaHeightRadius, Skeleton);

        for (int y = updatableArea.Top; y <= updatableArea.Bottom; y++)
        {
            for (int x = updatableArea.Left; x <= updatableArea.Right; x++)
            {
                var cell = new Cell(x, y);

                Skeleton[y, x].Update(gameTime);

                if (_hoverTilesInfo.TryGetValue(cell, out var tile))
                {
                    tile.Update(gameTime);
                }
            }
        }

        Position = _hero.Position;
    }

    public void Initialize(Hero hero)
    {
        _hero = hero;

        _exitOpenDistance = _hero.FrameSize * ExitOpenDistanceCoeff;

        Position = _hero.Position;
    }

    public void InitializeComponentsList()
    {
        void InitializeSkeleton()
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

        void InitializeHoverTiles()
        {
            foreach (var (cell, tile) in _hoverTilesInfo)
            {
                tile.Position = GetCellPosition(cell);

                _mazeTiles.Add(tile);
            }
        }

        void InitializeExit()
        {
            ExitInfo.Exit.Position = GetCellPosition(ExitInfo.Cell);

            _mazeTiles.Add(ExitInfo.Exit);
        }

        InitializeSkeleton();
        InitializeHoverTiles();
        InitializeExit();
    }

    public Rectangle GetCollideArea()
    {
        var heroCell = SpriteBaseState.GetSpriteCell(_hero);

        return HitBoxHelper.GetArea(heroCell, MazeTileCollideAreaRadius, MazeTileCollideAreaRadius, Skeleton);
    }

    public bool IsFloor(Cell cell)
    {
        return Skeleton[cell.Y, cell.X].TileType is TileType.Floor and not TileType.Exit
           && !_hoverTilesInfo.ContainsKey(cell);
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
        _hoverTilesInfo.Add(cell, trap);
    }

    public void InsertExit(Exit exit, Cell cell)
    {
        ExitInfo = (cell, exit);

        Skeleton[cell.Y, cell.X] = new Floor();
    }

    public void InsertItem(MazeItem item, Cell cell)
    {
        _hoverTilesInfo.Add(cell, item);
    }

    public void InsertMark(MazeMark mark, Cell cell)
    {
        _hoverTilesInfo.Add(cell, mark);

        _mazeTiles.Add(mark);
    }

    public bool CanInsertMark(Cell cell)
    {
        return !_hoverTilesInfo.ContainsKey(cell);
    }

    public void RemoveItem(Cell cell)
    {
        var cellPosition = GetCellPosition(cell);

        var itemTile = _mazeTiles.Where(mazeTile => mazeTile.Position == cellPosition && mazeTile is MazeItem).Single();

        _hoverTilesInfo.Remove(cell);
        _mazeTiles.Remove(itemTile);
    }

    private bool NeedOpenExit()
    {
        return IsKeyCollected
         && !ExitInfo.Exit.IsOpened
         && Vector2.Distance(_hero.Position, GetCellPosition(ExitInfo.Cell)) < _exitOpenDistance;
    }
}