﻿using MazeRunner.Content;
using MazeRunner.Extensions;
using MazeRunner.GameBase;
using MazeRunner.Managers;
using MazeRunner.MazeBase;
using MazeRunner.MazeBase.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace MazeRunner.Sprites.States;

public abstract class GuardMoveBaseState : GuardBaseState
{
    protected GuardMoveBaseState(ISpriteState previousState, Hero hero, Guard guard, Maze maze) : base(previousState, hero, guard, maze)
    {
    }

    public override Texture2D Texture => Textures.Sprites.Guard.Run;

    public override int FramesCount => 4;

    public override double UpdateTimeDelayMs => 150;

    public static bool PathToHeroExist(Hero hero, Guard guard, Maze maze, out IEnumerable<Vector2> path)
    {
        path = FindPathToHero(maze, hero, guard);

        if (!path.Any())
        {
            return false;
        }

        return true;
    }

    protected static Vector2 GetMovementDirection(Guard guard, IEnumerable<Vector2> path)
    {
        var direction = Vector2.Zero;
        var guardPosition = GetSpriteNormalizedPosition(guard);

        foreach (var movingPosition in path)
        {
            if (!IsPositionReached(movingPosition, guard))
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

    protected static IEnumerable<Vector2> FindPathToHero(Maze maze, Hero hero, Guard guard)
    {
        var heroCell = GetSpriteCell(hero, maze);
        var guardCell = GetSpriteCell(guard, maze);

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
                if (currentNode.LinkNumber > Optimization.GuardHeroMaxPathLength)
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

    protected static bool IsPositionReached(Vector2 position, Sprite sprite)
    {
        var hitBox = sprite.GetHitBox(sprite.Position);
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

    private static bool CanMove(Guard guard, Vector2 movement, Maze maze)
    {
        var possiblePosition = guard.Position + movement;

        return !CollisionManager.CollidesWithTraps(guard, possiblePosition, maze, true, out var _);
    }

    protected bool ProcessMovement(Guard guard, Vector2 direction, Maze maze, GameTime gameTime)
    {
        var movement = guard.GetMovement(direction, gameTime);

        if (!CanMove(guard, movement, maze))
        {
            return false;
        }

        ProcessFrameEffect(movement);

        guard.Position += movement;

        return true;
    }
}
