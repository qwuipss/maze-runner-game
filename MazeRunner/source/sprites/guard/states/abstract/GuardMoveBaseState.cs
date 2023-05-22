using MazeRunner.Content;
using MazeRunner.MazeBase;
using MazeRunner.MazeBase.Tiles;
using MazeRunner.Wrappers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
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

    protected static Cell GetSpriteCell(SpriteInfo spriteInfo, Maze maze)
    {
        var position = GetSpriteNormalizedPosition(spriteInfo);
        var cell = maze.GetCellByPosition(position);

        return cell;
    }

    protected static Vector2 GetSpriteNormalizedPosition(SpriteInfo spriteInfo)
    {
        var hitBox = spriteInfo.Sprite.GetHitBox(spriteInfo.Position);
        var position = new Vector2(hitBox.X + hitBox.Width / 2, hitBox.Y + hitBox.Height / 2);

        return position;
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

    protected static Vector2 GetMovingPosition(Cell cell, Maze maze)
    {
        var halfFrameSize = maze.Skeleton[cell.Y, cell.X].FrameSize / 2;
        var position = maze.GetCellPosition(cell);

        return new Vector2(position.X + halfFrameSize, position.Y + halfFrameSize);
    }

    protected static bool IsPositionReached(Vector2 position, SpriteInfo spriteInfo)
    {
        var hitBox = spriteInfo.Sprite.GetHitBox(spriteInfo.Position);
        var positionMaterialBox = new RectangleF(position.X, position.Y, float.Epsilon, float.Epsilon);

        return hitBox.IntersectsWith(positionMaterialBox);
    }

    protected static IEnumerable<Cell> GetAdjacentMovingCells(Cell cell, Cell exitCell, Maze maze, HashSet<Cell> visitedCells)
    {
        return MazeGenerator.GetAdjacentCells(cell, maze, 1)
            .Where(cell => maze.Skeleton[cell.Y, cell.X].TileType is not TileType.Wall && cell != exitCell && !visitedCells.Contains(cell));
    }
}
