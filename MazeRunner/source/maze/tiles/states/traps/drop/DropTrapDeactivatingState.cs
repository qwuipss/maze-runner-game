using Microsoft.Xna.Framework;

namespace MazeRunner.MazeBase.Tiles.States;

public class DropTrapDeactivatingState : DropTrapBaseState
{
    public DropTrapDeactivatingState(MazeTrap trap)
    {
        Trap = trap;

        CurrentAnimationFramePointX = (FramesCount - 1) * FrameWidth;
    }

    public override IMazeTileState ProcessState(GameTime gameTime)
    {
        CurrentAnimationFramePointX -= FrameWidth;

        if (CurrentAnimationFramePointX is 0)
        {
            return new DropTrapDeactivatedState(Trap);
        }

        return this;
    }
}