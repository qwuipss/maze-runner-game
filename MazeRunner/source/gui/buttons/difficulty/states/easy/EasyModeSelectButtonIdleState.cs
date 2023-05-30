using MazeRunner.Content;
using MazeRunner.Wrappers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace MazeRunner.Gui.Buttons.States;

public class EasyModeSelectButtonIdleState : ButtonBaseState
{
    public override Texture2D Texture => Textures.Gui.Buttons.EasyModeSelect.Idle;

    public override int FramesCount => 1;

    public EasyModeSelectButtonIdleState(Button button) : base(button)
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
            return new EasyModeSelectButtonHoverState(Button);
        }

        return this;
    }
}
