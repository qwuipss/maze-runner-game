using MazeRunner.Helpers;
using MazeRunner.Managers;
using MazeRunner.Sprites;
using Microsoft.Xna.Framework;

namespace MazeRunner.MazeBase.Tiles.States;

public class BayonetTrapPreActivatingState : BayonetTrapBaseState
{
    private const int MinUpdateTimeMs = 700;

    private const int MaxUpdateTimeMs = 1000;

    private readonly double _updateTimeDelayMs;

    protected override double UpdateTimeDelayMs => _updateTimeDelayMs;

    public BayonetTrapPreActivatingState(Hero hero, MazeTrap trap) : base(hero, trap)
    {
        _updateTimeDelayMs = RandomHelper.Next(MinUpdateTimeMs, MaxUpdateTimeMs);

        CurrentAnimationFramePoint = new Point(FrameSize, 0);
    }

    public override IMazeTileState ProcessState(GameTime gameTime)
    {
        ElapsedGameTimeMs += gameTime.ElapsedGameTime.TotalMilliseconds;

        if (ElapsedGameTimeMs > UpdateTimeDelayMs)
        {
            var animationPoint = CurrentAnimationFramePoint;

            if (animationPoint.X == FrameSize * 2)
            {
                SoundManager.Traps.Bayonet.PlayPreActivateSound(GetDistanceToHero());
            }

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