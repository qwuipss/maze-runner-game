using MazeRunner.Gui.Buttons.States;
using MazeRunner.Wrappers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MazeRunner.Gui.Buttons;

public class Button
{
    public Action OnClick { get; init; }

    private IButtonState _state;

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

    public static float DrawingPriority
    {
        get
        {
            return .15f;
        }
    }

    public Button(Action onClick)
    {
        OnClick = onClick;
    }

    public void Initialize(ButtonInfo buttonInfo)
    {
        _state = new ButtonIdleState(buttonInfo);
    }

    public void Update(GameTime gameTime)
    {
        _state = _state.ProcessState(gameTime);
    }
}
