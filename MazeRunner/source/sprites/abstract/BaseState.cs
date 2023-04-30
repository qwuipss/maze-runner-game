#region Usings
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace MazeRunner.Sprites.States;

public abstract class BaseState : ISpriteState
{
    public abstract Texture2D Texture { get; }

    public abstract int FramesCount { get; }

    public virtual int Width
    {
        get
        {
            return Texture.Width / FramesCount;
        }
    }

    public virtual int Height
    {
        get
        {
            return Texture.Height;
        }
    }

    public virtual int AnimationDelayMs
    {
        get
        {
            return 50;
        }
    }

    public Point CurrentAnimationPoint
    {
        get
        {
            return new Point(CurrentAnimationPointX, 0);
        }
    }

    public virtual ISpriteState ProcessState()
    {
        CurrentAnimationPointX = (CurrentAnimationPointX + Width) % (Width * FramesCount);

        return this;
    }

    protected virtual int CurrentAnimationPointX { get; set; }
}