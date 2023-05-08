using MazeRunner.Helpers;
using Microsoft.Xna.Framework;

namespace MazeRunner.MazeBase.Tiles.States;

public class BayonetTrapActivatedState : BayonetTrapBaseState
{
    public BayonetTrapActivatedState(MazeTrap trap)
    {
        Trap = trap;

        CurrentAnimationFramePointX = (FramesCount - 1) * FrameWidth;
    }

    public override IMazeTileState ProcessState(GameTime gameTime)
    {
        return this;
    }
}