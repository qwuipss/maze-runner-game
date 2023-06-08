using MazeRunner.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MazeRunner.Gui.Buttons.States;

public class ResumeButtonClickedState : ButtonPushBaseState
{
    public override Texture2D Texture => Textures.Gui.Buttons.Resume.Click;

    public override int FramesCount => 5;

    public ResumeButtonClickedState(Button button) : base(button)
    {
    }

    public override IButtonState ProcessState(GameTime gameTime)
    {
        ElapsedGameTimeMs += gameTime.ElapsedGameTime.TotalMilliseconds;

        if (ElapsedGameTimeMs > UpdateTimeDelayMs)
        {
            var animationPoint = CurrentAnimationFramePoint;

            if (animationPoint.X == (FramesCount - 1) * FrameWidth)
            {
                Button.Click();

                return new ResumeButtonIdleState(Button);
            }

            var framePosX = animationPoint.X + FrameWidth;

            CurrentAnimationFramePoint = new Point(framePosX, 0);
            ElapsedGameTimeMs -= UpdateTimeDelayMs;
        }

        return this;
    }
}