using MazeRunner.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MazeRunner.Gui.Buttons.States;

public class EasyModeSelectButtonResetState : ButtonPushBaseState
{
    public override Texture2D Texture => Textures.Gui.Buttons.EasyModeSelect.Click;

    public override int FramesCount => 5;

    public EasyModeSelectButtonResetState(Button button) : base(button)
    {
        var framePosX = (FramesCount - 1) * FrameWidth;

        CurrentAnimationFramePoint = new Point(framePosX, 0);
    }

    public override IButtonState ProcessState(GameTime gameTime)
    {
        ElapsedGameTimeMs += gameTime.ElapsedGameTime.TotalMilliseconds;

        if (ElapsedGameTimeMs > UpdateTimeDelayMs)
        {
            var animationPoint = CurrentAnimationFramePoint;

            if (animationPoint.X is 0)
            {
                return new EasyModeSelectButtonIdleState(Button);
            }

            var framePosX = animationPoint.X - FrameWidth;

            CurrentAnimationFramePoint = new Point(framePosX, 0);
            ElapsedGameTimeMs -= UpdateTimeDelayMs;
        }

        return this;
    }
}