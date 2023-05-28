using MazeRunner.Content;
using MazeRunner.Wrappers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MazeRunner.Gui.Buttons.States;

internal class EasyModeSelectButtonSelectedState : ButtonBaseState
{
    public EasyModeSelectButtonSelectedState(ButtonInfo buttonInfo) : base(buttonInfo)
    {
        var radioButton = (RadioButton)buttonInfo.Button;
        radioButton.Select();

        var framePosX = (FramesCount - 1) * FrameWidth;

        CurrentAnimationFramePoint = new Point(framePosX, 0);
    }

    public override Texture2D Texture
    {
        get
        {
            return Textures.Gui.Buttons.EasyModeSelect.Click;
        }
    }

    public override int FramesCount
    {
        get
        {
            return 5;
        }
    }

    public override IButtonState ProcessState(GameTime gameTime)
    {
        return this;
    }
}
