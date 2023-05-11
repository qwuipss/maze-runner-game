using MazeRunner.Helpers;
using Microsoft.Xna.Framework;

namespace MazeRunner.MazeBase.Tiles.States;

public class BayonetTrapPreActivatingState : BayonetTrapBaseState
{
    private readonly int _updateTimeDelayMs;

    protected override int UpdateTimeDelayMs
    {
        get
        {
            return _updateTimeDelayMs;
        }
    }

    public BayonetTrapPreActivatingState()
    {
        var minUpdateTimeMs = 700;
        var maxUpdateTimeMs = 1000;

        _updateTimeDelayMs = RandomHelper.Next(minUpdateTimeMs, maxUpdateTimeMs);

        CurrentAnimationFramePointX = FrameSize;
    }

    public override IMazeTileState ProcessState(GameTime gameTime)
    {
        var elapsedTime = gameTime.ElapsedGameTime.TotalMilliseconds;

        ElapsedGameTimeMs += elapsedTime;

        if (ElapsedGameTimeMs > UpdateTimeDelayMs)
        {
            CurrentAnimationFramePointX += FrameSize;

            if (CurrentAnimationFramePointX == FrameSize * 2)
            {
                return new BayonetTrapPostActivatingState();
            }

            ElapsedGameTimeMs -= elapsedTime;
        }

        return this;
    }
}