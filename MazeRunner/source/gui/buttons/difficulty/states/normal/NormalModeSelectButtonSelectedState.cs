using MazeRunner.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MazeRunner.Gui.Buttons.States;

public class NormalModeSelectButtonSelectedState : ButtonBaseState
{
    public override Texture2D Texture => Textures.Gui.Buttons.NormalModeSelect.Click;

    public override int FramesCount => 5;

    public NormalModeSelectButtonSelectedState(Button button, bool notifyAboutPush = true) : base(button)
    {
        if (button is RadioButton radioButton)
        {
            if (notifyAboutPush)
            {
                radioButton.Click();
            }
        }
        else
        {
            throw new InvalidCastException();
        }

        var framePosX = (FramesCount - 1) * FrameWidth;

        CurrentAnimationFramePoint = new Point(framePosX, 0);
    }

    public override IButtonState ProcessState(GameTime gameTime)
    {
        return this;
    }
}
