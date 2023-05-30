using MazeRunner.Helpers;
using MazeRunner.MazeBase;
using MazeRunner.Wrappers;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;

namespace MazeRunner.Sprites.States;

public class GuardWalkState : GuardMoveBaseState
{
    private const int WalkPathMinLength = 3;

    private const int WalkPathMaxLength = 6;

    private readonly SpriteInfo _heroInfo;

    private readonly SpriteInfo _guardInfo;

    private readonly Maze _maze;

    private readonly LinkedList<Vector2> _walkPath;

    public GuardWalkState(ISpriteState previousState, SpriteInfo heroInfo, SpriteInfo guardInfo, Maze maze) : base(previousState)
    {
        _heroInfo = heroInfo;
        _guardInfo = guardInfo;

        _maze = maze;

        var walkPathLength = RandomHelper.Next(WalkPathMinLength, WalkPathMaxLength);

        _walkPath = GetRandomWalkPath(walkPathLength);
    }

    public override ISpriteState ProcessState(GameTime gameTime)
    {
        if (CollidesWithTraps(_guardInfo, _maze, true, out var trapType))
        {
            return GetTrapCollidingState(trapType);
        }

        if (IsHeroNearby(_heroInfo, _guardInfo, _maze, out var _))
        {
            return new GuardChaseState(this, _heroInfo, _guardInfo, _maze);
        }

        var walkPosition = _walkPath.First();

        var position = GetSpriteNormalizedPosition(_guardInfo);
        var direction = GetMovementDirection(position, walkPosition);

        if (!ProcessMovement(_guardInfo, direction, _maze, gameTime))
        {
            return new GuardIdleState(this, _heroInfo, _guardInfo, _maze);
        }

        if (IsPositionReached(walkPosition, _guardInfo))
        {
            _walkPath.RemoveFirst();
        }

        if (_walkPath.Count is 0)
        {
            return new GuardIdleState(this, _heroInfo, _guardInfo, _maze);
        }

        base.ProcessState(gameTime);

        return this;
    }

    private LinkedList<Vector2> GetRandomWalkPath(int pathLength)
    {
        var startCell = GetSpriteCell(_guardInfo, _maze);

        var currentCell = startCell;

        var visitedCells = new HashSet<Cell>() { currentCell };
        var walkPath = new LinkedList<Vector2>();

        var exitCell = _maze.ExitInfo.Cell;

        var movingPosition = GetCellNormalizedPosition(currentCell, _maze);

        walkPath.AddLast(movingPosition);

        for (int i = 0; i < pathLength; i++)
        {
            var adjacentCells = GetAdjacentMovingCells(currentCell, exitCell, _maze, visitedCells).ToArray();

            if (adjacentCells.Length is 0)
            {
                break;
            }

            currentCell = RandomHelper.Choice(adjacentCells);
            movingPosition = GetCellNormalizedPosition(currentCell, _maze);

            walkPath.AddLast(movingPosition);
            visitedCells.Add(currentCell);
        }

        walkPath.RemoveFirst();

        return walkPath;
    }
}