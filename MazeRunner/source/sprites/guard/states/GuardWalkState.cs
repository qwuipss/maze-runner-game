﻿using MazeRunner.Helpers;
using MazeRunner.MazeBase;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;

namespace MazeRunner.Sprites.States;

public class GuardWalkState : GuardMoveBaseState
{
    private const int WalkPathMinLength = 3;

    private const int WalkPathMaxLength = 6;

    private readonly LinkedList<Vector2> _walkPath;

    public GuardWalkState(ISpriteState previousState, Hero hero, Guard guard, Maze maze) : base(previousState, hero, guard, maze)
    {
        var walkPathLength = RandomHelper.Next(WalkPathMinLength, WalkPathMaxLength);

        _walkPath = GetRandomWalkPath(walkPathLength);
    }

    public override ISpriteState ProcessState(GameTime gameTime)
    {
        if (CollidesWithTraps(Guard, Maze, true, out var trapType))
        {
            return GetTrapCollidingState(trapType);
        }

        if (IsHeroNearby(out var _))
        {
            return new GuardChaseState(this, Hero, Guard, Maze);
        }

        var walkPosition = _walkPath.First();

        var position = GetSpriteNormalizedPosition(Guard);
        var direction = GetMovementDirection(position, walkPosition);

        if (!ProcessMovement(direction, gameTime))
        {
            return new GuardIdleState(this, Hero, Guard, Maze);
        }

        if (IsPositionReached(walkPosition, Guard))
        {
            _walkPath.RemoveFirst();
        }

        if (_walkPath.Count is 0)
        {
            return new GuardIdleState(this, Hero, Guard, Maze);
        }

        base.ProcessState(gameTime);

        return this;
    }

    private LinkedList<Vector2> GetRandomWalkPath(int pathLength)
    {
        var startCell = GetSpriteCell(Guard, Maze);

        var currentCell = startCell;

        var visitedCells = new HashSet<Cell>() { currentCell };
        var walkPath = new LinkedList<Vector2>();

        var exitCell = Maze.ExitInfo.Cell;

        var movingPosition = GetCellNormalizedPosition(currentCell, Maze);

        walkPath.AddLast(movingPosition);

        for (int i = 0; i < pathLength; i++)
        {
            var adjacentCells = GetAdjacentMovingCells(currentCell, Maze, visitedCells).ToArray();

            if (adjacentCells.Length is 0)
            {
                break;
            }

            currentCell = RandomHelper.Choice(adjacentCells);
            movingPosition = GetCellNormalizedPosition(currentCell, Maze);

            walkPath.AddLast(movingPosition);
            visitedCells.Add(currentCell);
        }

        walkPath.RemoveFirst();

        return walkPath;
    }
}