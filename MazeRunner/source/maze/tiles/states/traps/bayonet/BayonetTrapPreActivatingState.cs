using MazeRunner.Helpers;
using MazeRunner.Sprites;
using Microsoft.Xna.Framework;

namespace MazeRunner.MazeBase.Tiles.States;

public class BayonetTrapPreActivatingState : BayonetTrapBaseState
{
    private readonly double _updateTimeDelayMs;

    protected override double UpdateTimeDelayMs => _updateTimeDelayMs;

    public BayonetTrapPreActivatingState(Hero hero, MazeTrap trap) : base(hero, trap)
    {
        var minUpdateTimeMs = 700;
        var maxUpdateTimeMs = 1000;

        _updateTimeDelayMs = RandomHelper.Next(minUpdateTimeMs, maxUpdateTimeMs);

        CurrentAnimationFramePoint = new Point(FrameSize, 0);
    }

    public override IMazeTileState ProcessState(GameTime gameTime)
    {
        ElapsedGameTimeMs += gameTime.ElapsedGameTime.TotalMilliseconds;

        if (ElapsedGameTimeMs > UpdateTimeDelayMs)
        {
            var animationPoint = CurrentAnimationFramePoint;

            if (animationPoint.X == FrameSize * 4)
            {
                return new BayonetTrapPostActivatingState(Hero, Trap);
            }

            animationPoint.X += FrameSize;

            CurrentAnimationFramePoint = animationPoint;
            ElapsedGameTimeMs -= UpdateTimeDelayMs;
        }

        return this;
    }
}