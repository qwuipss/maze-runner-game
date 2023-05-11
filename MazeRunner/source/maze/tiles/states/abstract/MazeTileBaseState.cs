using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MazeRunner.MazeBase.Tiles.States;

public abstract class MazeTileBaseState : IMazeTileState
{
    public abstract Texture2D Texture { get; }

    public abstract int FramesCount { get; }

    protected abstract int UpdateTimeDelayMs { get; }

    public virtual int FrameSize
    {
        get
        {
            return Texture.Width / FramesCount;
        }
    }

    public virtual int FrameHeight
    {
        get
        {
            return Texture.Height;
        }
    }

    public virtual Point CurrentAnimationFramePoint
    {
        get
        {
            return new Point(CurrentAnimationFramePointX, 0);
        }
    }

    protected virtual IMazeTileState State { get; set; }

    protected virtual int CurrentAnimationFramePointX { get; set; }

    protected virtual double ElapsedGameTimeMs { get; set; }

    public abstract IMazeTileState ProcessState(GameTime gameTime);
}