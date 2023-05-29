using Microsoft.Xna.Framework;

namespace MazeRunner.Sprites.States;

public class GuardFalledState : GuardFallBaseState
{
    public override double UpdateTimeDelayMs => double.MaxValue;

    public GuardFalledState(ISpriteState previousState) : base(previousState)
    {
        var framePosX = (FramesCount - 1) * FrameSize;

        CurrentAnimationFramePoint = new Point(framePosX, 0);
    }

    public override ISpriteState ProcessState(GameTime gameTime)
    {
        return this;
    }
}
