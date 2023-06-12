using MazeRunner.Managers;
using MazeRunner.Sprites;
using Microsoft.Xna.Framework;

namespace MazeRunner.MazeBase.Tiles.States;

public class DropTrapDeactivatingState : DropTrapBaseState
{
    public DropTrapDeactivatingState(Hero hero, MazeTrap trap) : base(hero, trap)
    {
        var framePosX = (FramesCount - 1) * FrameSize;

        CurrentAnimationFramePoint = new Point(framePosX, 0);

        SoundManager.Traps.Drop.PlayDeactivateSound(GetDistanceToHero());
    }

    public override IMazeTileState ProcessState(GameTime gameTime)
    {
        ElapsedGameTimeMs += gameTime.ElapsedGameTime.TotalMilliseconds;

        if (ElapsedGameTimeMs > UpdateTimeDelayMs)
        {
            var animationPoint = CurrentAnimationFramePoint;

            if (animationPoint.X == FrameSize)
            {
                return new DropTrapDeactivatedState(Hero, Trap);
            }

            animationPoint.X -= FrameSize;

            CurrentAnimationFramePoint = animationPoint;

            ElapsedGameTimeMs -= UpdateTimeDelayMs;
        }

        return this;
    }
}