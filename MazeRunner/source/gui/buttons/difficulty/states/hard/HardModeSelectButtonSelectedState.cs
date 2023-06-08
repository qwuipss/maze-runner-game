using MazeRunner.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MazeRunner.Gui.Buttons.States;

public class HardModeSelectButtonSelectedState : ButtonBaseState
{
    public override Texture2D Texture => Textures.Gui.Buttons.HardModeSelect.Click;

    public override int FramesCount => 5;

    public HardModeSelectButtonSelectedState(Button button) : base(button)
    {
        if (button is RadioButton radioButton)
        {
            radioButton.Click();
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
