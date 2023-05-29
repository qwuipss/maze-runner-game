using MazeRunner.Gui.Buttons.States;
using MazeRunner.Wrappers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MazeRunner.Gui.Buttons;

public abstract class Button
{
    public static float DrawingPriority => .15f;

    public Texture2D Texture => State.Texture;

    public Rectangle CurrentAnimationFrame => State.CurrentAnimationFrame;

    public float Width => State.FrameWidth * SelfInfo.BoxScale;

    public float Height => State.FrameHeight * SelfInfo.BoxScale;

    public Action OnClick { get; init; }

    protected IButtonState State { get; set; }

    protected ButtonInfo SelfInfo { get; set; }

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
