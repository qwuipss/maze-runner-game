using MazeRunner.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MazeRunner.Sprites.States;

public class HeroDamageTakingState : HeroBaseState
{
    public override double UpdateTimeDelayMs => 75;

    public override Texture2D Texture => Textures.Sprites.Hero.DamageTaking;

    public override int FramesCount => 5;

    private readonly ISpriteState _previousState;

    public HeroDamageTakingState(ISpriteState previousState) : base(previousState)
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
                return _previousState;
            }

            var framePosX = animationPoint.X + FrameSize;

            CurrentAnimationFramePoint = new Point(framePosX, 0);
            ElapsedGameTimeMs -= UpdateTimeDelayMs;
        }

        return this;
    }
}
