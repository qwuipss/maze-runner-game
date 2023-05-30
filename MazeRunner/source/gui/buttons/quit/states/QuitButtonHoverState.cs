using MazeRunner.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MazeRunner.Gui.Buttons.States;

public class QuitButtonHoverState : ButtonBaseState
{
    public override Texture2D Texture => Textures.Gui.Buttons.Quit.Hover;

    public override int FramesCount => 1;

    public QuitButtonHoverState(Button button) : base(button)
    {
    }

    public override IButtonState ProcessState(GameTime gameTime)
    {
        var mouseState = Mouse.GetState();

        if (!IsCursorHoverButton(mouseState))
        {
            return new QuitButtonIdleState(Button);
        }

        if (mouseState.LeftButton == ButtonState.Pressed)
        {
            return new QuitButtonClickedState(Button);
        }

        return this;
    }
}
