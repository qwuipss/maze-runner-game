using MazeRunner.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MazeRunner.Gui.Buttons.States;

public class ResumeButtonHoverState : ButtonBaseState
{
    public override Texture2D Texture => Textures.Gui.Buttons.Resume.Hover;

    public override int FramesCount => 1;

    public ResumeButtonHoverState(Button button) : base(button)
    {
    }

    public override IButtonState ProcessState(GameTime gameTime)
    {
        var mouseState = Mouse.GetState();

        if (!IsCursorHoverButton(mouseState))
        {
            return new ResumeButtonIdleState(Button);
        }

        if (mouseState.LeftButton == ButtonState.Pressed)
        {
            return new ResumeButtonClickedState(Button);
        }

        return this;
    }
}
