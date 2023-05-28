using MazeRunner.Content;
using MazeRunner.Wrappers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MazeRunner.Gui.Buttons.States;

public class HardModeSelectButtonHoverState : ButtonBaseState
{
    public HardModeSelectButtonHoverState(ButtonInfo buttonInfo) : base(buttonInfo)
    {
    }

    public override Texture2D Texture
    {
        get
        {
            return Textures.Gui.Buttons.HardModeSelect.Hover;
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
            return new HardModeSelectButtonIdleState(ButtonInfo);
        }

        if (mouseState.LeftButton == ButtonState.Pressed)
        {
            return new HardModeSelectButtonClickedState(ButtonInfo);
        }

        return this;
    }
}
