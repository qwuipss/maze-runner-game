using MazeRunner.Managers;
using MazeRunner.MazeBase;
using MazeRunner.Wrappers;
using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace MazeRunner.Sprites.States;

public class GuardChaseState : GuardMoveBaseState
{
    private class LinkNode<T> : IEnumerable<T>
    {
        public T Value { get; init; }

#nullable enable
        public LinkNode<T>? Previous { get; set; }

        public LinkNode()
        {
        }

        public LinkNode(T item, LinkNode<T>? previous)
        {
            Value = item;
            Previous = previous;
        }
#nullable disable

        public IEnumerator<T> GetEnumerator()
        {
            var currentNode = this;

            while (currentNode is not null)
            {
                yield return currentNode.Value;

                currentNode = currentNode.Previous;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

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

        var maze = _mazeInfo.Maze;

        var cellsPath = FindPathToHero(_mazeInfo.Maze, _heroInfo, _guardInfo).ToList(); //
        var chasePath = cellsPath.Select(cell => GetMovingPosition(cell, maze)).ToList(); //

        var direction = Vector2.Zero;
        var guardPosition = GetSpriteNormalizedPosition(_guardInfo);

        foreach (var movingPosition in chasePath)
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

    private static IEnumerable<Cell> FindPathToHero(Maze maze, SpriteInfo heroInfo, SpriteInfo guardInfo)
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
                foreach (var cell in currentNode.Reverse().Skip(1))
                {
                    if ((cell.X - guardCell.X) * (cell.Y - guardCell.Y) is 0)
                    {
                        yield return cell;
                    }
                    else
                    {
                        yield break;
                    }
                }
            }

            var adjacentCells = GetAdjacentMovingCells(currentCell, exitCell, maze, visitedCells);

            foreach (var cell in adjacentCells)
            {
                searchingQueue.Enqueue(new LinkNode<Cell>(cell, currentNode));
                visitedCells.Add(cell);
            }
        }
    }
}
