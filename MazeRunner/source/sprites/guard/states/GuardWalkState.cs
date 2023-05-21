using MazeRunner.Helpers;
using MazeRunner.MazeBase;
using MazeRunner.MazeBase.Tiles;
using MazeRunner.Wrappers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;

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

    private static Vector2 GetMovementDirection(Vector2 from, Vector2 to)
    {
        var directionX = to.X - from.X;
        var unitDirectionX = 0;

        if (directionX > float.Epsilon)
        {
            unitDirectionX = 1;
        }
        else if (directionX < -float.Epsilon)
        {
            unitDirectionX = -1;
        }

        var directionY = to.Y - from.Y;
        var unitDirectionY = 0;

        if (directionY > float.Epsilon)
        {
            unitDirectionY = 1;
        }
        else if (directionY < -float.Epsilon)
        {
            unitDirectionY = -1;
        }

        return new Vector2(unitDirectionX, unitDirectionY);
    }

    public override ISpriteState ProcessState(GameTime gameTime)
    {
        base.ProcessState(gameTime);

        var walkPosition = _walkPath.First();

        var guardPosition = _guardInfo.Position;
        var direction = GetMovementDirection(guardPosition, walkPosition);

        var guard = _guardInfo.Sprite;
        var movement = guard.GetMovement(direction, gameTime);

        var newPosition = guardPosition + movement;

        _guardInfo.Position = newPosition;

        if (MathF.Abs(walkPosition.X - newPosition.X) < 1 && MathF.Abs(walkPosition.Y - newPosition.Y) < 1)
        {
            _walkPath.RemoveFirst();
        }

        if (_walkPath.Count is 0)
        {
            return new GuardIdleState(this, _heroInfo, _guardInfo, _mazeInfo);
        }

        return this;
    }

    private LinkedList<Vector2> GetRandomWalkPath(int pathLength)
    {
        Vector2 GetCellWalkPosition(Cell cell)
        {
            var cellPosition = _mazeInfo.Maze.GetCellPosition(cell);

            return cellPosition;
        }

        var startCell = _mazeInfo.Maze.GetCellByPosition(_guardInfo.Position);

        var maze = _mazeInfo.Maze;
        var skeleton = maze.Skeleton;

        var currentCell = startCell;

        var visitedCells = new HashSet<Cell>() { currentCell };

        var path = new LinkedList<Vector2>();
        var walkPosition = GetCellWalkPosition(currentCell);

        path.AddLast(walkPosition);

        for (int i = 0; i < pathLength; i++)
        {
            var adjacentCells = MazeGenerator.GetAdjacentCells(currentCell, maze, 1)
                .Where(cell => skeleton[cell.Y, cell.X].TileType is not TileType.Wall && maze.ExitInfo.Cell != cell && !visitedCells.Contains(cell)).ToArray();

            if (adjacentCells.Length is 0)
            {
                break;
            }

            currentCell = RandomHelper.Choice(adjacentCells);
            walkPosition = GetCellWalkPosition(currentCell);

            path.AddLast(walkPosition);
            visitedCells.Add(currentCell);
        }

        return path;
    }
}