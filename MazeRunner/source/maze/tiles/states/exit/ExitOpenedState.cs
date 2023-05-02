#region Usings
#endregion

namespace MazeRunner.MazeBase.Tiles.States;

public class ExitOpenedState : BayonetTrapBaseState
{
    public ExitOpenedState()
    {
        CurrentAnimationFramePointX = (FramesCount - 1) * FrameWidth;
    }

    public override IMazeTileState ProcessState()
    {
        return this;
    }
}