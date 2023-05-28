using MazeRunner.Content;
using MazeRunner.Wrappers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MazeRunner.Gui.Buttons.States;

public class EasyModeSelectButtonIdleState : ButtonBaseState
{
    public EasyModeSelectButtonIdleState(ButtonInfo buttonInfo) : base(buttonInfo)
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
            return Textures.Gui.Buttons.EasyModeSelect.Idle;
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
            return new EasyModeSelectButtonHoverState(ButtonInfo);
        }

        return this;
    }
}
