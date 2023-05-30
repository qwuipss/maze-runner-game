using MazeRunner.Content;
using MazeRunner.Wrappers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MazeRunner.Gui.Buttons.States;

public class RestartButtonHoverState : ButtonBaseState
{
    public override Texture2D Texture => Textures.Gui.Buttons.Restart.Hover;

    public override int FramesCount => 1;

    public RestartButtonHoverState(Button button) : base(button)
    {
    }

    public override IButtonState ProcessState(GameTime gameTime)
    {
        var mouseState = Mouse.GetState();

        if (!IsCursorHoverButton(mouseState))
        {
            return new RestartButtonIdleState(Button);
        }

        if (mouseState.LeftButton == ButtonState.Pressed)
        {
            return new RestartButtonClickedState(Button);
        }

        return this;
    }
}
