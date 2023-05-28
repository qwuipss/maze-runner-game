using MazeRunner.Gui.Buttons.States;
using MazeRunner.Wrappers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MazeRunner.Gui.Buttons;

public abstract class Button
{
    public Action OnClick { get; init; }

    protected IButtonState State { get; set; }

    protected ButtonInfo SelfInfo { get; set; }

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
            return State.Texture;
        }
    }

    public Rectangle CurrentAnimationFrame
    {
        get
        {
            return State.CurrentAnimationFrame;
        }
    }

    public float Width
    {
        get
        {
            return State.FrameWidth * SelfInfo.BoxScale;
        }
    }

    public float Height
    {
        get
        {
            return State.FrameHeight * SelfInfo.BoxScale;
        }
    }

    public Button(Action onClick)
    {
        OnClick = onClick;
    }

    public virtual void Initialize(ButtonInfo buttonInfo)
    {
        SelfInfo = buttonInfo;
    }

    public virtual void Update(GameTime gameTime)
    {
        State = State.ProcessState(gameTime);
    }
}
