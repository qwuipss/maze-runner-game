using MazeRunner.Helpers;
using Microsoft.Xna.Framework;

namespace MazeRunner.MazeBase.Tiles.States;

public class DropTrapDeactivatedState : DropTrapBaseState
{
    private readonly int _updateTimeDelayMs;

    protected override int UpdateTimeDelayMs
    {
        get
        {
            return _updateTimeDelayMs;
        }
    }

    public DropTrapDeactivatedState()
    {
        var minUpdateTimeMs = 1000;
        var maxUpdateTimeMs = 15000;

        _updateTimeDelayMs = RandomHelper.Next(minUpdateTimeMs, maxUpdateTimeMs);
    }

    public override IMazeTileState ProcessState(GameTime gameTime)
    {
        ElapsedGameTimeMs += gameTime.ElapsedGameTime.TotalMilliseconds;

        if (ElapsedGameTimeMs > UpdateTimeDelayMs)
        {
            return new DropTrapActivatingState();
        }

        return this;
    }
}