using Microsoft.Xna.Framework;

namespace MazeRunner.MazeBase.Tiles.States;

public class DropTrapActivatingState : DropTrapBaseState
{
    public DropTrapActivatingState(MazeTrap trap)
    {
        Trap = trap;
    }

    public override IMazeTileState ProcessState(GameTime gameTime)
    {
        CurrentAnimationFramePointX += FrameWidth;

        if (CurrentAnimationFramePointX == (FramesCount - 1) * FrameWidth)
        {
            return new DropTrapActivatedState(Trap);
        }

        return this;
    }
}