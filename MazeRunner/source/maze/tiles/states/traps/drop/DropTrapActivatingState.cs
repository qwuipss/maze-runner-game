using MazeRunner.Managers;
using MazeRunner.Sprites;
using Microsoft.Xna.Framework;

namespace MazeRunner.MazeBase.Tiles.States;

public class DropTrapActivatingState : DropTrapBaseState
{
    public DropTrapActivatingState(Hero hero, MazeTrap trap) : base(hero, trap)
    {
        SoundManager.Traps.Drop.PlayActivateSound(GetDistanceToHero());
    }

    public override IMazeTileState ProcessState(GameTime gameTime)
    {
        ElapsedGameTimeMs += gameTime.ElapsedGameTime.TotalMilliseconds;

        if (ElapsedGameTimeMs > UpdateTimeDelayMs)
        {
            var animationPoint = CurrentAnimationFramePoint;

            if (animationPoint.X == (FramesCount - 1) * FrameSize)
            {
                return new DropTrapActivatedState(Hero, Trap);
            }

            animationPoint.X += FrameSize;

            CurrentAnimationFramePoint = animationPoint;
            ElapsedGameTimeMs -= UpdateTimeDelayMs;
        }

        return this;
    }
}