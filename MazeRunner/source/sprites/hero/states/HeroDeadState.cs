using Microsoft.Xna.Framework;

namespace MazeRunner.Sprites.States;

public class HeroDeadState : HeroDeathBaseState
{
    public HeroDeadState()
    {
        var framePosX = (FramesCount - 1) * FrameSize;

        CurrentAnimationFramePoint = new Point(framePosX, 0);
    }

    public override int UpdateTimeDelayMs
    {
        get
        {
            return int.MaxValue;
        }
    }

    public override ISpriteState ProcessState(GameTime gameTime)
    {
        return this;
    }
}
