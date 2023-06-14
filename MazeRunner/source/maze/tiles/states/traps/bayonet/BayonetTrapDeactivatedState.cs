using MazeRunner.Helpers;
using MazeRunner.Sprites;
using Microsoft.Xna.Framework;

namespace MazeRunner.MazeBase.Tiles.States;

public class BayonetTrapDeactivatedState : BayonetTrapBaseState
{
    private const int MinUpdateTimeMs = 1000;

    private const int MaxUpdateTimeMs = 10000;

    private readonly double _updateTimeDelayMs;

    protected override double UpdateTimeDelayMs => _updateTimeDelayMs;

    public BayonetTrapDeactivatedState(Hero hero, MazeTrap trap) : base(hero, trap)
    {
        _updateTimeDelayMs = RandomHelper.Next(MinUpdateTimeMs, MaxUpdateTimeMs);
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