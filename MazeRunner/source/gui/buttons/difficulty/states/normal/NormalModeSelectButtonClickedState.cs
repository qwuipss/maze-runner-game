using MazeRunner.Content;
using MazeRunner.Wrappers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MazeRunner.Gui.Buttons.States;

public class NormalModeSelectButtonClickedState : ButtonPushBaseState
{
    public override Texture2D Texture => Textures.Gui.Buttons.NormalModeSelect.Click;

    public override int FramesCount => 5;

    public NormalModeSelectButtonClickedState(ButtonInfo buttonInfo) : base(buttonInfo)
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
                ButtonInfo.Button.OnClick.Invoke();

                return new NormalModeSelectButtonSelectedState(ButtonInfo);
            }

            var framePosX = animationPoint.X + FrameWidth;

            CurrentAnimationFramePoint = new Point(framePosX, 0);
            ElapsedGameTimeMs -= UpdateTimeDelayMs;
        }

        return this;
    }
}
