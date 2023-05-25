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

    private readonly MazeInfo _mazeInfo;

    private readonly LinkedList<Vector2> _walkPath;

    public GuardWalkState(ISpriteState previousState, SpriteInfo heroInfo, SpriteInfo guardInfo, MazeInfo mazeInfo) : base(previousState)
    {
        _heroInfo = heroInfo;
        _guardInfo = guardInfo;

        _mazeInfo = mazeInfo;

        var walkPathLength = RandomHelper.Next(WalkPathMinLength, WalkPathMaxLength);

        _walkPath = GetRandomWalkPath(walkPathLength);
    }

    public override ISpriteState ProcessState(GameTime gameTime)
    {
        if (CollidesWithTraps(_guardInfo, _mazeInfo, true, out var trapType))
        {
            return GetTrapCollidingState(trapType);
        }

        if (IsHeroNearby(_heroInfo, _guardInfo, _mazeInfo, out var _))
        {
            return new GuardChaseState(this, _heroInfo, _guardInfo, _mazeInfo);
        }

        var walkPosition = _walkPath.First();

        var position = GetSpriteNormalizedPosition(_guardInfo);
        var direction = GetMovementDirection(position, walkPosition);

        if (!ProcessMovement(_guardInfo, direction, _mazeInfo.Maze, gameTime))
        {
            return new GuardIdleState(this, _heroInfo, _guardInfo, _mazeInfo);
        }

        if (IsPositionReached(walkPosition, _guardInfo))
        {
            _walkPath.RemoveFirst();
        }

        if (_walkPath.Count is 0)
        {
            return new GuardIdleState(this, _heroInfo, _guardInfo, _mazeInfo);
        }

        base.ProcessState(gameTime);

        return this;
    }

    private LinkedList<Vector2> GetRandomWalkPath(int pathLength)
    {
        var maze = _mazeInfo.Maze;

        var startCell = GetSpriteCell(_guardInfo, maze);

        var currentCell = startCell;

        var visitedCells = new HashSet<Cell>() { currentCell };
        var walkPath = new LinkedList<Vector2>();

        var exitCell = maze.ExitInfo.Cell;

        var movingPosition = GetCellNormalizedPosition(currentCell, maze);

        walkPath.AddLast(movingPosition);

        for (int i = 0; i < pathLength; i++)
        {
            var adjacentCells = GetAdjacentMovingCells(currentCell, exitCell, maze, visitedCells).ToArray();

            if (adjacentCells.Length is 0)
            {
                break;
            }

            currentCell = RandomHelper.Choice(adjacentCells);
            movingPosition = GetCellNormalizedPosition(currentCell, maze);

            walkPath.AddLast(movingPosition);
            visitedCells.Add(currentCell);
        }

        walkPath.RemoveFirst();

        return walkPath;
    }
}