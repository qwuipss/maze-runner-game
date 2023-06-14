using MazeRunner.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MazeRunner.Gui.Buttons.States;

public class StartButtonHoverState : ButtonBaseState
{
    public override Texture2D Texture => Textures.Gui.Buttons.Start.Hover;

    public override int FramesCount => 1;

    public StartButtonHoverState(Button button) : base(button)
    {
    }

    public override IButtonState ProcessState(GameTime gameTime)
    {
        var mouseState = Mouse.GetState();

        if (!IsCursorHoverButton(mouseState))
        {
            return new StartButtonIdleState(Button);
        }

        if (mouseState.LeftButton is ButtonState.Pressed)
        {
            if (Button.CanBeClicked.Invoke())
            {
                return new StartButtonClickedState(Button);
            }
        }

        return this;
    }
}
