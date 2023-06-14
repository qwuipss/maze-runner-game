using MazeRunner.Helpers;
using MazeRunner.Sprites;
using Microsoft.Xna.Framework;

namespace MazeRunner.MazeBase.Tiles.States;

public class DropTrapDeactivatedState : DropTrapBaseState
{
    private const int MinUpdateTimeMs = 1000;

    private const int MaxUpdateTimeMs = 15000;

    private readonly double _updateTimeDelayMs;

    protected override double UpdateTimeDelayMs => _updateTimeDelayMs;

    public DropTrapDeactivatedState(Hero hero, MazeTrap trap) : base(hero, trap)
    {
        _updateTimeDelayMs = RandomHelper.Next(MinUpdateTimeMs, MaxUpdateTimeMs);
    }

    public override IMazeTileState ProcessState(GameTime gameTime)
    {
        ElapsedGameTimeMs += gameTime.ElapsedGameTime.TotalMilliseconds;

        if (ElapsedGameTimeMs > UpdateTimeDelayMs)
        {
            return new DropTrapActivatingState(Hero, Trap);
        }

        return this;
    }
}