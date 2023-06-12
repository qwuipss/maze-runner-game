using MazeRunner.Managers;
using MazeRunner.Sprites;
using Microsoft.Xna.Framework;

namespace MazeRunner.MazeBase.Tiles.States;

public class BayonetTrapPostActivatingState : BayonetTrapBaseState
{
    public BayonetTrapPostActivatingState(Hero hero, MazeTrap trap) : base(hero, trap)
    {
        CurrentAnimationFramePoint = new Point(FrameSize * 3, 0);

        SoundManager.Traps.Bayonet.PlayActivateSound(GetDistanceToHero());
    }

    public override IMazeTileState ProcessState(GameTime gameTime)
    {
        ElapsedGameTimeMs += gameTime.ElapsedGameTime.TotalMilliseconds;

        if (ElapsedGameTimeMs > UpdateTimeDelayMs)
        {
            var animationPoint = CurrentAnimationFramePoint;

            if (animationPoint.X == (FramesCount - 1) * FrameSize)
            {
                return new BayonetTrapActivatedState(Hero, Trap);
            }

            animationPoint.X += FrameSize;

            CurrentAnimationFramePoint = animationPoint;
            ElapsedGameTimeMs -= UpdateTimeDelayMs;
        }

        return this;
    }
}
