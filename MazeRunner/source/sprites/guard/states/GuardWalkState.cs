using MazeRunner.Helpers;
using MazeRunner.Managers;
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

    private const float WalkingNormalizationAdditive = 1;

    private readonly SpriteInfo _heroInfo;

    private readonly SpriteInfo _guardInfo;
    private readonly MazeInfo _mazeInfo;

    private readonly LinkedList<Vector2> _walkPath;
    private readonly LinkedList<Vector2> _debugPath;

    public GuardWalkState(ISpriteState previousState, SpriteInfo heroInfo, SpriteInfo guardInfo, MazeInfo mazeInfo) : base(previousState)
    {
        _heroInfo = heroInfo;

        _guardInfo = guardInfo;
        _mazeInfo = mazeInfo;

        var walkPathLength = RandomHelper.Next(WalkPathMinLength, WalkPathMaxLength);

        _walkPath = GetRandomWalkPath(walkPathLength);
        _debugPath = new LinkedList<Vector2>(_walkPath);
    }

    private static Vector2 GetMovementDirection(Vector2 from, Vector2 to)
    {
        var delta = to - from;

        if (delta != Vector2.Zero)
        {
            return Vector2.Normalize(delta);
        }

        return delta;
    }

    public override ISpriteState ProcessState(GameTime gameTime)
    {
        static bool IsWalkPositionReached(Vector2 walkPosition, Vector2 position)
        {
            return MathF.Abs(walkPosition.X - position.X) < WalkingNormalizationAdditive 
                && MathF.Abs(walkPosition.Y - position.Y) < WalkingNormalizationAdditive;
        }

        base.ProcessState(gameTime); 
        
        if (IsHeroNearby(_heroInfo, _guardInfo))
        {
            //return new GuardChaseState(this, _heroInfo, _guardInfo, _mazeInfo);
        }

        var walkPosition = _walkPath.First();

        var guardPosition = _guardInfo.Position;
        var direction = GetMovementDirection(guardPosition, walkPosition);

        var guard = _guardInfo.Sprite;
        var movement = guard.GetMovement(direction, gameTime);

        ProcessFrameEffect(movement);

        var newPosition = guardPosition + movement;

        if (CollisionManager.CollidesWithWalls(guard, guardPosition, movement, _mazeInfo.Maze)) //
        {
            throw new Exception();
        }

        _guardInfo.Position = newPosition;

        if (IsWalkPositionReached(walkPosition, newPosition))
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
        static Cell[] GetAdjacentWalkingCells(Cell cell, Cell exitCell, Maze maze, HashSet<Cell> visitedCells)
        {
            return MazeGenerator.GetAdjacentCells(cell, maze, 1)
                .Where(cell => maze.Skeleton[cell.Y, cell.X].TileType is not TileType.Wall && exitCell != cell && !visitedCells.Contains(cell))
                .ToArray();
        };

        var guardPosition = _guardInfo.Position;

        var startPosition = new Vector2(guardPosition.X + WalkingNormalizationAdditive, guardPosition.Y + WalkingNormalizationAdditive);
        var startCell = _mazeInfo.Maze.GetCellByPosition(startPosition);

        var currentCell = startCell;

        var visitedCells = new HashSet<Cell>() { currentCell };

        var path = new LinkedList<Vector2>(); 
        
        var maze = _mazeInfo.Maze;
        var skeleton = maze.Skeleton;

        var exitCell = maze.ExitInfo.Cell;

        var walkingPosition = maze.GetCellPosition(currentCell);

        path.AddLast(walkingPosition);

        for (int i = 0; i < pathLength; i++)
        {
            var adjacentCells = GetAdjacentWalkingCells(currentCell, exitCell, maze, visitedCells);

            if (adjacentCells.Length is 0)
            {
                break;
            }

            currentCell = RandomHelper.Choice(adjacentCells);
            walkingPosition = maze.GetCellPosition(currentCell);

            path.AddLast(walkingPosition);
            visitedCells.Add(currentCell);
        }

        return path;
    }
}