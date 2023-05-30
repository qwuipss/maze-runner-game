using MazeRunner.Content;
using MazeRunner.Wrappers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MazeRunner.Gui.Buttons.States;

public class EasyModeSelectButtonSelectedState : ButtonBaseState
{
    public override Texture2D Texture => Textures.Gui.Buttons.EasyModeSelect.Click;

    public override int FramesCount => 5;

    public EasyModeSelectButtonSelectedState(Button button) : base(button)
    {
        if (button is RadioButton radioButton)
        {
            radioButton.Select();
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
