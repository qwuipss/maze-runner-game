using MazeRunner.Extensions;
using MazeRunner.MazeBase;
using MazeRunner.Wrappers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MazeRunner.Sprites.States;

public class GuardChaseState : GuardMoveBaseState
{
    private readonly SpriteInfo _heroInfo;

    private readonly SpriteInfo _guardInfo;
    private readonly MazeInfo _mazeInfo;

    public GuardChaseState(ISpriteState previousState, SpriteInfo heroInfo, SpriteInfo guardInfo, MazeInfo mazeInfo) : base(previousState)
    {
        _heroInfo = heroInfo;

        _guardInfo = guardInfo;
        _mazeInfo = mazeInfo;
    }

    public override ISpriteState ProcessState(GameTime gameTime)
    {
        base.ProcessState(gameTime);

        if (!IsHeroNearby(_heroInfo, _guardInfo))
        {
            return new GuardIdleState(this, _heroInfo, _guardInfo, _mazeInfo);
        }

        var pathToHero = FindPathToHero(_mazeInfo.Maze, _heroInfo, _guardInfo);

        var direction = Vector2.Zero;
        var guardPosition = GetSpriteNormalizedPosition(_guardInfo);

        foreach (var movingPosition in pathToHero)
        {
            if (!IsPositionReached(movingPosition, _guardInfo))
            {
                direction = GetMovementDirection(guardPosition, movingPosition);
                break;
            }
        }

        var movement = _guardInfo.Sprite.GetMovement(direction, gameTime);

        ProcessFrameEffect(movement);

        _guardInfo.Position += movement;

        return this;
    }

    private static IEnumerable<Vector2> FindPathToHero(Maze maze, SpriteInfo heroInfo, SpriteInfo guardInfo)
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
                return GetMovingPositionsPath(currentNode.Reverse(), maze);
            }

            var adjacentCells = GetAdjacentMovingCells(currentCell, exitCell, maze, visitedCells);

            foreach (var cell in adjacentCells)
            {
                searchingQueue.Enqueue(new LinkNode<Cell>(cell, currentNode));
                visitedCells.Add(cell);
            }
        }

        throw new Exception();
    }

    private static IEnumerable<Vector2> GetMovingPositionsPath(IEnumerable<Cell> cellsPath, Maze maze)
    {
        var enumerator = cellsPath.GetEnumerator();

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
}
