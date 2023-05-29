using MazeRunner.Content;
using MazeRunner.Wrappers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MazeRunner.Gui.Buttons.States;

public class NormalModeSelectButtonResetState : ButtonPushBaseState
{
    public override Texture2D Texture => Textures.Gui.Buttons.NormalModeSelect.Click;

    public override int FramesCount => 5;

    public NormalModeSelectButtonResetState(ButtonInfo buttonInfo) : base(buttonInfo)
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
                return new NormalModeSelectButtonIdleState(ButtonInfo);
            }

            var framePosX = animationPoint.X - FrameWidth;

            CurrentAnimationFramePoint = new Point(framePosX, 0);
            ElapsedGameTimeMs -= UpdateTimeDelayMs;
        }

        return this;
    }
}