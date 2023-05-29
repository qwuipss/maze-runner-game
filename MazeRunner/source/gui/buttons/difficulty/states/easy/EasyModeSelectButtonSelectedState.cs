using MazeRunner.Content;
using MazeRunner.Wrappers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MazeRunner.Gui.Buttons.States;

public class EasyModeSelectButtonSelectedState : ButtonBaseState
{
    public override Texture2D Texture => Textures.Gui.Buttons.EasyModeSelect.Click;

    public override int FramesCount => 5;

    public EasyModeSelectButtonSelectedState(ButtonInfo buttonInfo) : base(buttonInfo)
    {
        var radioButton = (RadioButton)buttonInfo.Button;
        radioButton.Select();

        var framePosX = (FramesCount - 1) * FrameWidth;

        CurrentAnimationFramePoint = new Point(framePosX, 0);
    }

    public override IButtonState ProcessState(GameTime gameTime)
    {
        return this;
    }
}
