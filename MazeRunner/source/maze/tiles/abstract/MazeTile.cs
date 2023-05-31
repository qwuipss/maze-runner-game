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

    public virtual float DrawingPriority => 1;

    public Texture2D Texture => State.Texture;

    public int FrameSize => State.FrameSize;

    public RectagleXna CurrentAnimationFrame => State.CurrentAnimationFrame;

    public Vector2 Position { get; set; }

    public float FrameRotationAngle { get; set; }

    public Vector2 OriginFrameRotationVector { get; set; }

    protected IMazeTileState State { get; set; }

    public static Vector2 GetOriginFrameRotationVector(MazeTile mazeTile)
    {
        var rotation = mazeTile.FrameRotationAngle;

        if (rotation is MathHelper.PiOver2)
        {
            return new Vector2(0, mazeTile.FrameSize);
        }

        if (rotation is -MathHelper.PiOver2)
        {
            return new Vector2(mazeTile.FrameSize, 0);
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