using MazeRunner.MazeBase;
using Microsoft.Xna.Framework;

namespace MazeRunner.Sprites.States;

public class GuardDeadState : GuardDeathBaseState
{
    public GuardDeadState(ISpriteState previousState, Hero hero, Guard guard, Maze maze) : base(previousState, hero, guard, maze)
    {
        var framePosX = (FramesCount - 1) * FrameSize;

        CurrentAnimationFramePoint = new Point(framePosX, 0);
    }

    public override double UpdateTimeDelayMs
    {
        get
        {
            return double.MaxValue;
        }
    }

    public override ISpriteState ProcessState(GameTime gameTime)
    {
        return this;
    }
}
