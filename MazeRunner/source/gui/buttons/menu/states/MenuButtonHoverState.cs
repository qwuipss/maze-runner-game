using MazeRunner.Content;
using MazeRunner.Wrappers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MazeRunner.Gui.Buttons.States;

public class MenuButtonHoverState : ButtonBaseState
{
    public override Texture2D Texture => Textures.Gui.Buttons.Menu.Hover;

    public override int FramesCount => 1;

    public MenuButtonHoverState(ButtonInfo buttonInfo) : base(buttonInfo)
    {
    }

    public override IButtonState ProcessState(GameTime gameTime)
    {
        var mouseState = Mouse.GetState();

        if (!IsCursorHoverButton(mouseState, ButtonInfo))
        {
            return new MenuButtonIdleState(ButtonInfo);
        }

        if (mouseState.LeftButton == ButtonState.Pressed)
        {
            return new MenuButtonClickedState(ButtonInfo);
        }

        return this;
    }
}