using MazeRunner.Wrappers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MazeRunner.Gui.Buttons.States;

public class ButtonIdleState : ButtonBaseState
{
    private readonly ButtonInfo _buttonInfo;

    private readonly Texture2D _texture;

    private readonly int _framesCount;

    public override Texture2D Texture
    {
        get
        {
            return _texture;
        }
    }

    public override int FramesCount
    {
        get
        {
            return _framesCount;
        }
    }

    public ButtonIdleState(ButtonInfo buttonInfo)
    {
        _buttonInfo = buttonInfo;

        _texture = _buttonInfo.IdleStateInfo.Texture;
        _framesCount = _buttonInfo.IdleStateInfo.FramesCount;
    }

    public override IButtonState ProcessState(GameTime gameTime)
    {
        var mouseState = Mouse.GetState();

        if (IsCursorHoverButton(mouseState, _buttonInfo))
        {
            return new ButtonHoverState(_buttonInfo);
        }

        return this;
    }
}
