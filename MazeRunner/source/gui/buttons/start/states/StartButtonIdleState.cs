using MazeRunner.Content;
using MazeRunner.Wrappers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MazeRunner.Gui.Buttons.States;

public class StartButtonIdleState : ButtonBaseState
{
    public override Texture2D Texture => Textures.Gui.Buttons.Start.Idle;

    public override int FramesCount => 1;

    public StartButtonIdleState(ButtonInfo buttonInfo) : base(buttonInfo)
    {
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
