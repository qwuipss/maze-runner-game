#region Usings
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace MazeRunner.Sprites.States;

public abstract class BaseState : ISpriteState
{
    public abstract Texture2D Texture { get; }

    public abstract int FramesCount { get; }

    public virtual int TextureWidth
    {
        get
        {
            return Texture.Width / FramesCount;
        }
    }

    public virtual int TextureHeight
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
        CurrentAnimationPointX = (CurrentAnimationPointX + TextureWidth) % (TextureWidth * FramesCount);

        return this;
    }

    protected virtual int CurrentAnimationPointX { get; set; }
}