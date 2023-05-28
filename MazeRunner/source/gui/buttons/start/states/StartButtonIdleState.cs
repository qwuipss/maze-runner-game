using MazeRunner.Content;
using MazeRunner.Wrappers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MazeRunner.Gui.Buttons.States;

public class StartButtonIdleState : ButtonBaseState
{
    public StartButtonIdleState(ButtonInfo buttonInfo) : base(buttonInfo)
    {
    }

    public override Texture2D Texture
    {
        get
        {
            return Textures.Gui.Buttons.Start.Idle;
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
            return new StartButtonHoverState(ButtonInfo);
        }

        return this;
    }
}
