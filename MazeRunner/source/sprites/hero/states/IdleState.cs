#region Usings
using MazeRunner.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace MazeRunner.Sprites.States;

public class IdleState : ISpriteState
{
    private int _currentAnimationPointX;

    public Texture2D Texture
    {
        get
        {
            return Textures.Sprites.Heroes.HeroIdle;
        }
    }

    public int FramesCount
    {
        get
        {
            return 6;
        }
    }

    public int Width
    {
        get
        {
            return Texture.Width / FramesCount;
        }
    }

    public int Height
    {
        get
        {
            return Texture.Height;
        }
    }

    public int AnimationDelayMs
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
            return new Point(_currentAnimationPointX, 0);
        }
    }

    public ISpriteState ProcessState()
    {
        _currentAnimationPointX = (_currentAnimationPointX + Width) % (Width * FramesCount);

        return this;
    }
}
