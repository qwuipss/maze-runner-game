using MazeRunner.Content;
using MazeRunner.Wrappers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MazeRunner.Gui.Buttons.States;

public class MenuButtonIdleState : ButtonBaseState
{
    public override Texture2D Texture => Textures.Gui.Buttons.Menu.Idle;

    public override int FramesCount => 1;

    public MenuButtonIdleState(Button button) : base(button)
    {
    }

    public override IButtonState ProcessState(GameTime gameTime)
    {
        var mouseState = Mouse.GetState();

        if (IsCursorHoverButton(mouseState))
        {
            return new MenuButtonHoverState(Button);
        }

        return this;
    }
}