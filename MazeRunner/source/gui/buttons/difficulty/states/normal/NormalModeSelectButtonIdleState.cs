using MazeRunner.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace MazeRunner.Gui.Buttons.States;

public class NormalModeSelectButtonIdleState : ButtonBaseState
{
    public override Texture2D Texture => Textures.Gui.Buttons.NormalModeSelect.Idle;

    public override int FramesCount => 1;

    public NormalModeSelectButtonIdleState(Button button) : base(button)
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
            return new NormalModeSelectButtonHoverState(Button);
        }

        return this;
    }
}
