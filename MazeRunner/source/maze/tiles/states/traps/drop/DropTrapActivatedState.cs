using MazeRunner.Helpers;
using Microsoft.Xna.Framework;

namespace MazeRunner.MazeBase.Tiles.States;

public class DropTrapActivatedState : DropTrapBaseState
{
    private readonly int _updateTimeDelayMs;

    protected override int UpdateTimeDelayMs
    {
        get
        {
            return _updateTimeDelayMs;
        }
    }

    public DropTrapActivatedState()
    {
        var minUpdateTimeMs = 1000;
        var maxUpdateTimeMs = 1300;

        _updateTimeDelayMs = RandomHelper.Next(minUpdateTimeMs, maxUpdateTimeMs);

        var framePosX = (FramesCount - 1) * FrameSize;

        CurrentAnimationFramePoint = new Point(framePosX, 0);
    }

    public override IMazeTileState ProcessState(GameTime gameTime)
    {
        ElapsedGameTimeMs += gameTime.ElapsedGameTime.TotalMilliseconds;

        if (ElapsedGameTimeMs > UpdateTimeDelayMs)
        {
            return new DropTrapDeactivatingState();
        }

        return this;
    }
}