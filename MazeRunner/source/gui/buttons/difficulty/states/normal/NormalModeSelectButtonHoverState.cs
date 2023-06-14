using MazeRunner.Content;
using MazeRunner.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MazeRunner.Gui.Buttons.States;

public class NormalModeSelectButtonHoverState : ButtonBaseState
{
    public override Texture2D Texture => Textures.Gui.Buttons.NormalModeSelect.Hover;

    public override int FramesCount => 1;

    public NormalModeSelectButtonHoverState(Button button) : base(button)
    {
    }

    public override IButtonState ProcessState(GameTime gameTime)
    {
        var mouseState = Mouse.GetState();

        if (!IsCursorHoverButton(mouseState))
        {
            return new NormalModeSelectButtonIdleState(Button);
        }

        if (mouseState.LeftButton is ButtonState.Pressed)
        {
            if (Button.CanBeClicked.Invoke())
            {
                return new NormalModeSelectButtonClickedState(Button);
            }
        }

        return this;
    }
}
