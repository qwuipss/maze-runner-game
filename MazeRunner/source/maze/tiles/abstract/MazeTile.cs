using MazeRunner.MazeBase.Tiles.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MazeRunner.MazeBase.Tiles;

public abstract class MazeTile
{
    public abstract TileType TileType { get; }

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

    public virtual IMazeTileState State { get; set; }

    protected virtual double ElapsedGameTimeMs { get; set; }

    public virtual Point GetCurrentAnimationFramePoint(GameTime gameTime)
    {
        ElapsedGameTimeMs += gameTime.ElapsedGameTime.TotalMilliseconds;

        if (ElapsedGameTimeMs >= State.FrameAnimationDelayMs)
        {
            State = ProcessState();
            ElapsedGameTimeMs -= State.FrameAnimationDelayMs;
        }

        return State.CurrentAnimationFramePoint;
    }

    public virtual IMazeTileState ProcessState()
    {
        return State.ProcessState();
    }
}