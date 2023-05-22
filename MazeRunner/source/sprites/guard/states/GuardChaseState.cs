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

        var pathHeadNode = FindPathToHero(_mazeInfo.Maze, _heroInfo, _guardInfo).Reverse().Skip(1);
        
        var movingPositions = new List<Vector2>();

        foreach (var cell in pathHeadNode)
        {
            var movingPosition = maze.GetCellPosition(cell);

            movingPositions.Add(movingPosition);
        }

        var position = _guardInfo.Position;
        var direction = Vector2.Zero;

        foreach (var targetPosition in movingPositions)
        {
            if (!IsWalkPositionReached(targetPosition, position))
            {
                direction = GetMovementDirection(position, targetPosition);
                break;
            }
        }

        var movement = _guardInfo.Sprite.GetMovement(direction, gameTime);

        ProcessFrameEffect(movement);

        var newPosition = _guardInfo.Position + movement;

        _guardInfo.Position = newPosition;

        return this;
    }

    private static LinkNode<Cell> FindPathToHero(Maze maze, SpriteInfo heroInfo, SpriteInfo guardInfo)
    {
        var heroStartPosition = GetNormalizedPosition(heroInfo.Position);
        var heroCell = maze.GetCellByPosition(heroStartPosition);

        var guardStartPosition = GetNormalizedPosition(guardInfo.Position);
        var guardCell = maze.GetCellByPosition(guardStartPosition);

        var visitedCells = new HashSet<Cell>() { guardCell };

        var headNode = new LinkNode<Cell>(guardCell, null);
        var searchingQueue = new Queue<LinkNode<Cell>>();

        searchingQueue.Enqueue(headNode);

        var exitCell = maze.ExitInfo.Cell;

        while (searchingQueue.Count is not 0)
        {
            var currentCellNode = searchingQueue.Dequeue();
            var currentCell = currentCellNode.Value;

            if (currentCell == heroCell)
            {
                return currentCellNode;
            }

            var adjacentCells = GetAdjacentMovingCells(currentCell, exitCell, maze, visitedCells);

            foreach (var cell in adjacentCells)
            {
                if (!visitedCells.Contains(cell))
                {
                    searchingQueue.Enqueue(new LinkNode<Cell>(cell, currentCellNode));
                    visitedCells.Add(cell);
                }
            }
        }

        return new LinkNode<Cell>();
    }
}
