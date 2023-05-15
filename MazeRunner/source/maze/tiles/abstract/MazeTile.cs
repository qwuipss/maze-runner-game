using MazeRunner.Extensions;
using MazeRunner.Helpers;
using MazeRunner.MazeBase.Tiles.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MazeRunner.MazeBase.Tiles;

public abstract class MazeTile
{
    private static readonly float[] FrameRotationAngles = new[] { MathHelper.PiOver2, -MathHelper.PiOver2 };

    public abstract TileType TileType { get; }

    public virtual float FrameRotationAngle { get; set; }

    public virtual Vector2 OriginFrameRotationVector { get; set; }

    public virtual float DrawingPriority
    {
        get
        {
            return 1;
        }
    }

    public virtual Texture2D Texture
    {
        get
        {
            return State.Texture;
        }
    }

    public virtual int FrameSize
    {
        get
        {
            return State.FrameSize;
        }
    }

    public virtual Rectangle CurrentAnimationFrame
    {
        get
        {
            return State.CurrentAnimationFrame;
        }
    }

    protected virtual IMazeTileState State { get; set; }

    public static Vector2 GetOriginFrameRotationVector(MazeTile tile)
    {
        var rotation = tile.FrameRotationAngle;

        if (rotation is MathHelper.PiOver2)
        {
            return new Vector2(0, tile.FrameSize);
        }

        if (rotation is -MathHelper.PiOver2)
        {
            return new Vector2(tile.FrameSize, 0);
        }

        return Vector2.Zero;
    }

    public virtual void Update(GameTime gameTime)
    {
        State = State.ProcessState(gameTime);
    }

    public virtual FloatRectangle GetHitBox(Vector2 position)
    {
        return HitBoxHelper.GetHitBox(position, FrameSize, FrameSize);
    }
}