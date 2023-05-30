using MazeRunner.Components;
using MazeRunner.Drawing;
using MazeRunner.Helpers;
using MazeRunner.MazeBase.Tiles.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Drawing;
using RectagleXna = Microsoft.Xna.Framework.Rectangle;

namespace MazeRunner.MazeBase.Tiles;

public abstract class MazeTile : MazeRunnerGameComponent
{
    public abstract TileType TileType { get; }

    public float FrameRotationAngle { get; set; }

    public Vector2 OriginFrameRotationVector { get; set; }

    public virtual float DrawingPriority
    {
        get
        {
            return 1;
        }
    }

    public Texture2D Texture
    {
        get
        {
            return State.Texture;
        }
    }

    public int FrameSize
    {
        get
        {
            return State.FrameSize;
        }
    }

    public RectagleXna CurrentAnimationFrame
    {
        get
        {
            return State.CurrentAnimationFrame;
        }
    }

    protected virtual IMazeTileState State { get; set; }

    public Vector2 Position { get; set; }

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

    public virtual RectangleF GetHitBox(Vector2 position)
    {
        return HitBoxHelper.GetHitBox(position, FrameSize, FrameSize);
    }

    public override void Update(GameTime gameTime)
    {
        State = State.ProcessState(gameTime);
    }

    public override void Draw(GameTime gameTime)
    {
        Drawer.DrawMazeTile(this);
    }
}