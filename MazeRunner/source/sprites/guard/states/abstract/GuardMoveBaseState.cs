using MazeRunner.Content;
using MazeRunner.Extensions;
using MazeRunner.Managers;
using MazeRunner.MazeBase;
using MazeRunner.MazeBase.Tiles;
using MazeRunner.Wrappers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace MazeRunner.Sprites.States;

public abstract class GuardMoveBaseState : GuardBaseState
{
    protected GuardMoveBaseState(ISpriteState previousState) : base(previousState)
    {
    }

    public override Texture2D Texture
    {
        get
        {
            return Textures.Sprites.Guard.Run;
        }
    }

    public override int FramesCount
    {
        get
        {
            return 4;
        }
    }

    public override double UpdateTimeDelayMs
    {
        get
        {
            return 150;
        }
    }

    public static bool PathToHeroExist(SpriteInfo heroInfo, SpriteInfo guardInfo, MazeInfo mazeInfo, out IEnumerable<Vector2> path)
    {
        path = FindPathToHero(mazeInfo.Maze, heroInfo, guardInfo);

        if (!path.Any())
        {
            return false;
        }

        return true;
    }

    protected static Vector2 GetMovementDirection(SpriteInfo guardInfo, IEnumerable<Vector2> path)
    {
        var direction = Vector2.Zero;
        var guardPosition = GetSpriteNormalizedPosition(guardInfo);

        foreach (var movingPosition in path)
        {
            if (!IsPositionReached(movingPosition, guardInfo))
            {
                direction = GetMovementDirection(guardPosition, movingPosition);

                break;
            }
        }

        return direction;
    }

    protected static Vector2 GetMovementDirection(Vector2 from, Vector2 to)
    {
        var delta = to - from;

        if (delta != Vector2.Zero)
        {
            return Vector2.Normalize(delta);
        }

        return delta;
    }

    protected static IEnumerable<Vector2> FindPathToHero(Maze maze, SpriteInfo heroInfo, SpriteInfo guardInfo)
    {
        var heroCell = GetSpriteCell(heroInfo, maze);
        var guardCell = GetSpriteCell(guardInfo, maze);

        var visitedCells = new HashSet<Cell>() { guardCell };

        var startNode = new LinkNode<Cell>(guardCell, null);
        var searchingQueue = new Queue<LinkNode<Cell>>();

        searchingQueue.Enqueue(startNode);

        var exitCell = maze.ExitInfo.Cell;

        while (searchingQueue.Count is not 0)
        {
            var currentNode = searchingQueue.Dequeue();
            var currentCell = currentNode.Value;

            if (currentCell == heroCell)
            {
                if (currentNode.LinkNumber > OptimizationConstants.GuardHeroMaxPathLength)
                {
                    return Array.Empty<Vector2>();
                }

                return GetPath(currentNode.Reverse(), maze);
            }

            var adjacentCells = GetAdjacentMovingCells(currentCell, exitCell, maze, visitedCells);

            foreach (var cell in adjacentCells)
            {
                searchingQueue.Enqueue(new LinkNode<Cell>(cell, currentNode));
                visitedCells.Add(cell);
            }
        }

        return Array.Empty<Vector2>();
    }

    protected static IEnumerable<Vector2> GetPath(IEnumerable<Cell> cellsPath, Maze maze)
    {
        using var enumerator = cellsPath.GetEnumerator();

        while (enumerator.MoveNext())
        {
            var firstCell = enumerator.Current;

            if (!enumerator.MoveNext())
            {
                yield return GetCellNormalizedPosition(firstCell, maze);
            }

            var firstCellPosition = GetCellNormalizedPosition(firstCell, maze);

            var secondCell = enumerator.Current;
            var secondCellPosition = GetCellNormalizedPosition(secondCell, maze);

            var delta = (secondCellPosition - firstCellPosition) / 2f;

            yield return new Vector2(firstCellPosition.X + delta.X, firstCellPosition.Y + delta.Y);
            yield return secondCellPosition;
        }
    }

    protected static bool IsPositionReached(Vector2 position, SpriteInfo spriteInfo)
    {
        var hitBox = spriteInfo.Sprite.GetHitBox(spriteInfo.Position);
        var positionMaterialBox = new RectangleF(position.X, position.Y, float.Epsilon, float.Epsilon);

        return hitBox.IntersectsWith(positionMaterialBox);
    }

    protected static Vector2 GetCellNormalizedPosition(Cell cell, Maze maze)
    {
        var halfFrameSize = maze.Skeleton[cell.Y, cell.X].FrameSize / 2;
        var position = maze.GetCellPosition(cell);

        return new Vector2(position.X + halfFrameSize, position.Y + halfFrameSize);
    }

    protected static IEnumerable<Cell> GetAdjacentMovingCells(Cell cell, Cell exitCell, Maze maze, HashSet<Cell> visitedCells)
    {
        return MazeGenerator.GetAdjacentCells(cell, maze, 1)
            .Where(cell => maze.Skeleton[cell.Y, cell.X].TileType is not TileType.Wall && cell != exitCell && !visitedCells.Contains(cell));
    }

    private static bool CanMove(SpriteInfo guardInfo, Vector2 movement, Maze maze)
    {
        var possiblePosition = guardInfo.Position + movement;

        return !CollisionManager.CollidesWithTraps(guardInfo.Sprite, possiblePosition, maze, true, out var _);
    }

    protected bool ProcessMovement(SpriteInfo guardInfo, Vector2 direction, Maze maze, GameTime gameTime)
    {
        var movement = guardInfo.Sprite.GetMovement(direction, gameTime);

        if (!CanMove(guardInfo, movement, maze))
        {
            return false;
        }

        ProcessFrameEffect(movement);

        guardInfo.Position += movement;

        return true;
    }
}
