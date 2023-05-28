using MazeRunner.Content;
using MazeRunner.Wrappers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MazeRunner.Gui.Buttons.States;

public class NormalModeSelectButtonIdleState : ButtonBaseState
{
    public NormalModeSelectButtonIdleState(ButtonInfo buttonInfo) : base(buttonInfo)
    {
        var radionButton = (RadioButton)buttonInfo.Button;

        if (radionButton.IsSelected)
        {
            radionButton.IsSelected = false;
        }
    }

    public override Texture2D Texture
    {
        get
        {
            return Textures.Gui.Buttons.NormalModeSelect.Idle;
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

        if (IsCursorHoverButton(mouseState, ButtonInfo))
        {
            return new NormalModeSelectButtonHoverState(ButtonInfo);
        }

        return this;
    }
}
