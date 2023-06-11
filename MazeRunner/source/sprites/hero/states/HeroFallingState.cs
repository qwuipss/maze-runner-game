using MazeRunner.MazeBase;
using Microsoft.Xna.Framework;

namespace MazeRunner.Sprites.States;

public class HeroFallingState : HeroFallBaseState
{
    private readonly ISpriteState _previousState;

    public override double UpdateTimeDelayMs => 75;

    public HeroFallingState(ISpriteState previousState, Hero hero, Maze maze) : base(previousState, hero, maze)
    {
        _previousState = previousState;
    }

    public override ISpriteState ProcessState(GameTime gameTime)
    {
        ElapsedGameTimeMs += gameTime.ElapsedGameTime.TotalMilliseconds;

        if (ElapsedGameTimeMs > UpdateTimeDelayMs)
        {
            var animationPoint = CurrentAnimationFramePoint;

            if (animationPoint.X == (FramesCount - 1) * FrameSize)
            {
                return new HeroFellState(this, Hero, Maze, _previousState is HeroRunState);
            }

            var framePosX = animationPoint.X + FrameSize;

            CurrentAnimationFramePoint = new Point(framePosX, 0);
            ElapsedGameTimeMs -= UpdateTimeDelayMs;
        }

        return this;
    }
}
