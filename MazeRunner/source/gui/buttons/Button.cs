using MazeRunner.Components;
using MazeRunner.Drawing;
using MazeRunner.Gui.Buttons.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MazeRunner.Gui.Buttons;

public abstract class Button : MazeRunnerGameComponent
{
    public static float DrawingPriority => .15f;

    public Texture2D Texture => State.Texture;

    public Rectangle CurrentAnimationFrame => State.CurrentAnimationFrame;

    public float Width => State.FrameWidth * BoxScale;

    public float Height => State.FrameHeight * BoxScale;

    public Action OnClick { get; init; }

    public float BoxScale { get; init; }

    public Vector2 Position { get; set; }

    protected IButtonState State { get; set; }

    public Button(Action onClick, float boxScale)
    {
        OnClick = onClick;
        BoxScale = boxScale;
    }

    public abstract void Initialize();

    public override void Update(GameTime gameTime)
    {
        State = State.ProcessState(gameTime);
    }

    public override void Draw(GameTime gameTime)
    {
        Drawer.DrawButton(this);
    }
}
