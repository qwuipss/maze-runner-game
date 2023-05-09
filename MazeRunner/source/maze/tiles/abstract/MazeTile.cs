using MazeRunner.MazeBase.Tiles.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MazeRunner.MazeBase.Tiles;

public abstract class MazeTile
{
    public abstract TileType TileType { get; }

    public virtual float FrameRotationAngle { get; init; }

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

    public virtual int FrameWidth
    {
        get
        {
            return State.FrameWidth;
        }
    }

    public virtual int FrameHeight
    {
        get
        {
            return State.FrameHeight;
        }
    }

    public virtual Point CurrentAnimationFramePoint
    {
        get
        {
            return State.CurrentAnimationFramePoint;
        }
    }

    protected virtual IMazeTileState State { get; set; }

    public virtual void Update(GameTime gameTime)
    {
        State = State.ProcessState(gameTime);
    }
}