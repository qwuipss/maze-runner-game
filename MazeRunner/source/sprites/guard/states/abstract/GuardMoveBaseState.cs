using MazeRunner.Content;
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
    public override Texture2D Texture => Textures.Sprites.Guard.Run;

    public override int FramesCount => 4;

    public override double UpdateTimeDelayMs => 150;

    protected GuardMoveBaseState(ISpriteState previousState, Hero hero, Guard guard, Maze maze) : base(previousState, hero, guard, maze)
    {
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

    protected static IEnumerable<Vector2> FindPathToHero(Hero hero, Guard guard, Maze maze)
    {
        var heroCell = GetSpriteCell(hero, maze);
        var guardCell = GetSpriteCell(guard, maze);

        var visitedCells = new HashSet<Cell>() { guardCell };

        var startNode = new LinkNode<Cell>(guardCell, null);
        var searchingQueue = new Queue<LinkNode<Cell>>();

        searchingQueue.Enqueue(startNode);

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

            var adjacentCells = GetAdjacentMovingCells(currentCell, maze, visitedCells);

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

    protected static IEnumerable<Cell> GetAdjacentMovingCells(Cell cell, Maze maze, HashSet<Cell> visitedCells)
    {
        var exitCell = maze.ExitInfo.Cell;

        return MazeGenerator.GetAdjacentCells(cell, maze, 1)
            .Where(cell => maze.Skeleton[cell.Y, cell.X].TileType is not TileType.Wall && cell != exitCell && !visitedCells.Contains(cell));
    }

    public static bool PathToHeroExist(Hero hero, Guard guard, Maze maze, out IEnumerable<Vector2> path)
    {
        path = FindPathToHero(hero, guard, maze);

        if (!path.Any())
        {
            return false;
        }

        return true;
    }

    protected Vector2 GetMovementDirection(IEnumerable<Vector2> path)
    {
        var direction = Vector2.Zero;
        var guardPosition = GetSpriteNormalizedPosition(Guard);

        foreach (var movingPosition in path)
        {
            if (!IsPositionReached(movingPosition, Guard))
            {
                direction = GetMovementDirection(guardPosition, movingPosition);

                break;
            }
        }

        return direction;
    }

    private bool CanMove(Vector2 movement)
    {
        var possiblePosition = Guard.Position + movement;

        return !CollisionManager.CollidesWithTraps(Guard, possiblePosition, Maze, true, out var _);
    }

    protected bool ProcessMovement(Vector2 direction, GameTime gameTime)
    {
        var movement = Guard.GetMovement(direction, gameTime);

        if (!CanMove(movement))
        {
            return false;
        }

        ProcessFrameEffect(movement);

        Guard.Position += movement;

        return true;
    }
}
