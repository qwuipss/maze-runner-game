using MazeRunner.Gui.Buttons.States;
using MazeRunner.Wrappers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MazeRunner.Gui.Buttons;

public class Button
{
    public Action OnClick { get; init; }

    private ButtonInfo _selfInfo;

    private IButtonState _state;

    public static float DrawingPriority
    {
        get
        {
            return .15f;
        }
    }

    public Texture2D Texture
    {
        get
        {
            return _state.Texture;
        }
    }

    public Rectangle CurrentAnimationFrame
    {
        get
        {
            return _state.CurrentAnimationFrame;
        }
    }

    public float Width
    {
        get
        {
            return _state.FrameWidth * _selfInfo.BoxScale;
        }
    }

    public float Height
    {
        get
        {
            return _state.FrameHeight * _selfInfo.BoxScale;
        }
    }

    public Button(Action onClick)
    {
        OnClick = onClick;
    }

    public void Initialize(ButtonInfo buttonInfo)
    {
        _selfInfo = buttonInfo;

        _state = new ButtonIdleState(_selfInfo);
    }

    public void Update(GameTime gameTime)
    {
        _state = _state.ProcessState(gameTime);
    }
}
