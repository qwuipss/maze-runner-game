using MazeRunner.Content;
using MazeRunner.MazeBase;
using MazeRunner.MazeBase.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MazeRunner.Sprites.States;

public abstract class GuardMoveBaseState : GuardBaseState
{
    protected const float MoveNormalizationAdditive = 1.25f;

    protected GuardMoveBaseState(ISpriteState previousState) : base(previousState)
    {
    }

    public override Texture2D Texture
    {
        get
        {
            return Textures.Sprites.Guard.Run;
        }
    }

    public override int FramesCount
    {
        get
        {
            return 4;
        }
    }

    public override double UpdateTimeDelayMs
    {
        get
        {
            return 150;
        }
    }

    protected static bool IsWalkPositionReached(Vector2 walkPosition, Vector2 position)
    {
        return MathF.Abs(walkPosition.X - position.X) < MoveNormalizationAdditive
            && MathF.Abs(walkPosition.Y - position.Y) < MoveNormalizationAdditive;
    }

    protected static IEnumerable<Cell> GetAdjacentMovingCells(Cell cell, Cell exitCell, Maze maze, HashSet<Cell> visitedCells)
    {
        return MazeGenerator.GetAdjacentCells(cell, maze, 1)
            .Where(cell => maze.Skeleton[cell.Y, cell.X].TileType is not TileType.Wall && exitCell != cell && !visitedCells.Contains(cell));
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

    protected static Vector2 GetNormalizedPosition(Vector2 position)
    {
        return new Vector2(position.X + MoveNormalizationAdditive, position.Y + MoveNormalizationAdditive);
    }
}
