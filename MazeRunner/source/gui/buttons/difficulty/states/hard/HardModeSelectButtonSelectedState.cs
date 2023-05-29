using MazeRunner.Content;
using MazeRunner.Wrappers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MazeRunner.Gui.Buttons.States;

public class HardModeSelectButtonSelectedState : ButtonBaseState
{
    public override Texture2D Texture => Textures.Gui.Buttons.HardModeSelect.Click;

    public override int FramesCount => 5;

    public HardModeSelectButtonSelectedState(ButtonInfo buttonInfo) : base(buttonInfo)
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
