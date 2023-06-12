using MazeRunner.Managers;
using MazeRunner.Sprites;
using Microsoft.Xna.Framework;

namespace MazeRunner.MazeBase.Tiles.States;

public class BayonetTrapDeactivatingState : BayonetTrapBaseState
{
    public BayonetTrapDeactivatingState(Hero hero, MazeTrap trap) : base(hero, trap)
    {
        var framePosX = (FramesCount - 1) * FrameSize;

        CurrentAnimationFramePoint = new Point(framePosX, 0);

        SoundManager.Traps.Bayonet.PlayDeactivateSound(GetDistanceToHero());
    }

    public override IMazeTileState ProcessState(GameTime gameTime)
    {
        ElapsedGameTimeMs += gameTime.ElapsedGameTime.TotalMilliseconds;

        var animationPoint = CurrentAnimationFramePoint;

        if (ElapsedGameTimeMs > UpdateTimeDelayMs)
        {
            if (animationPoint.X is 0)
            {
                return new BayonetTrapDeactivatedState(Hero, Trap);
            }

            animationPoint.X -= FrameSize;

            CurrentAnimationFramePoint = animationPoint;
            ElapsedGameTimeMs -= UpdateTimeDelayMs;
        }

        return this;
    }
}