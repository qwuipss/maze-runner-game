using MazeRunner.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace MazeRunner.Gui.Buttons.States;

public class HardModeSelectButtonIdleState : ButtonBaseState
{
    public override Texture2D Texture => Textures.Gui.Buttons.HardModeSelect.Idle;

    public override int FramesCount => 1;

    public HardModeSelectButtonIdleState(Button button) : base(button)
    {
        if (button is RadioButton radioButton)
        {
            if (radioButton.IsSelected)
            {
                radioButton.IsSelected = false;
            }
        }
        else
        {
            throw new InvalidCastException();
        }
    }

    public override IButtonState ProcessState(GameTime gameTime)
    {
        var mouseState = Mouse.GetState();

        if (IsCursorHoverButton(mouseState))
        {
            return new HardModeSelectButtonHoverState(Button);
        }

        return this;
    }
}
