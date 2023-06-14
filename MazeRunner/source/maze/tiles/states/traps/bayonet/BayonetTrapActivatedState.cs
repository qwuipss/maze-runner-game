using MazeRunner.Helpers;
using MazeRunner.Sprites;
using Microsoft.Xna.Framework;

namespace MazeRunner.MazeBase.Tiles.States;

public class BayonetTrapActivatedState : BayonetTrapBaseState
{
    private const int MinUpdateTimeMs = 1000;

    private const int MaxUpdateTimeMs = 3000;

    private readonly double _updateTimeDelayMs;

    protected override double UpdateTimeDelayMs => _updateTimeDelayMs;

    public BayonetTrapActivatedState(Hero hero, MazeTrap trap) : base(hero, trap)
    {
        _updateTimeDelayMs = RandomHelper.Next(MinUpdateTimeMs, MaxUpdateTimeMs);

        var framePosX = (FramesCount - 1) * FrameSize;

        CurrentAnimationFramePoint = new Point(framePosX, 0);
    }

    public override IMazeTileState ProcessState(GameTime gameTime)
    {
        ElapsedGameTimeMs += gameTime.ElapsedGameTime.TotalMilliseconds;

        if (ElapsedGameTimeMs > UpdateTimeDelayMs)
        {
            return new BayonetTrapDeactivatingState(Hero, Trap);
        }

        return this;
    }
}