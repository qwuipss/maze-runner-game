using Microsoft.Xna.Framework;

namespace MazeRunner.MazeBase.Tiles.States;

public class BayonetTrapActivatingState : BayonetTrapBaseState
{
    public BayonetTrapActivatingState(MazeTrap trap)
    {
        Trap = trap;
    }

    public override IMazeTileState ProcessState(GameTime gameTime)
    {
        CurrentAnimationFramePointX += FrameWidth;

        if (CurrentAnimationFramePointX == (FramesCount - 1) * FrameWidth)
        {
            return new BayonetTrapActivatedState(Trap);
        }

        return this;
    }
}