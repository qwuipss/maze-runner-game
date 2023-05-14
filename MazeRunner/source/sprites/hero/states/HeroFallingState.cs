using MazeRunner.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MazeRunner.Sprites.States;

public class HeroFallingState : HeroBaseState
{
    public override Texture2D Texture
    {
        get
        {
            return Textures.Sprites.Hero.Fall;
        }
    }

    public override int FramesCount
    {
        get
        {
            return 4;
        }
    }

    public override int UpdateTimeDelayMs
    {
        get
        {
            return 75;
        }
    }

    public override ISpriteState ProcessState(GameTime gameTime)
    {
        ElapsedGameTimeMs += gameTime.ElapsedGameTime.TotalMilliseconds;

        if (ElapsedGameTimeMs > UpdateTimeDelayMs)
        {
            var animationPoint = CurrentAnimationFramePoint;

            if (animationPoint.X == (FramesCount - 1) * FrameSize)
            {
                return this;
            }

            var framePosX = animationPoint.X + FrameSize;

            CurrentAnimationFramePoint = new Point(framePosX, 0);

            ElapsedGameTimeMs -= UpdateTimeDelayMs;
        }

        return this;
    }
}
