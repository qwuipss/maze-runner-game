using MazeRunner.Content;
using MazeRunner.Wrappers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MazeRunner.Gui.Buttons.States;

public class NormalModeSelectButtonHoverState : ButtonBaseState
{
    public NormalModeSelectButtonHoverState(ButtonInfo buttonInfo) : base(buttonInfo)
    {
    }

    public override Texture2D Texture
    {
        get
        {
            return Textures.Gui.Buttons.NormalModeSelect.Hover;
        }
    }

    public override int FramesCount
    {
        get
        {
            return 1;
        }
    }

    public override IButtonState ProcessState(GameTime gameTime)
    {
        var mouseState = Mouse.GetState();

        if (!IsCursorHoverButton(mouseState, ButtonInfo))
        {
            return new NormalModeSelectButtonIdleState(ButtonInfo);
        }

        if (mouseState.LeftButton == ButtonState.Pressed)
        {
            return new NormalModeSelectButtonClickedState(ButtonInfo);
        }

        return this;
    }
}
