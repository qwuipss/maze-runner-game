using MazeRunner.Helpers;
using MazeRunner.Sprites;
using Microsoft.Xna.Framework;

namespace MazeRunner.MazeBase.Tiles.States;

public class BayonetTrapDeactivatedState : BayonetTrapBaseState
{
    private readonly double _updateTimeDelayMs;

    protected override double UpdateTimeDelayMs => _updateTimeDelayMs;

    public BayonetTrapDeactivatedState(Hero hero, MazeTrap trap) : base(hero, trap)
    {
        var minUpdateTimeMs = 1000;
        var maxUpdateTimeMs = 10000;

        _updateTimeDelayMs = RandomHelper.Next(minUpdateTimeMs, maxUpdateTimeMs);
    }

    public override IMazeTileState ProcessState(GameTime gameTime)
    {
        ElapsedGameTimeMs += gameTime.ElapsedGameTime.TotalMilliseconds;

        if (ElapsedGameTimeMs > UpdateTimeDelayMs)
        {
            return new BayonetTrapPreActivatingState(Hero, Trap);
        }

        return this;
    }
}