using MazeRunner.Components;
using MazeRunner.Drawing;
using MazeRunner.Gui.Buttons.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MazeRunner.Gui.Buttons;

public abstract class Button : MazeRunnerGameComponent
{
    public abstract event Action ButtonPressedNotify;

    public static float DrawingPriority => .15f;

    public Texture2D Texture => State.Texture;

    public Rectangle CurrentAnimationFrame => State.CurrentAnimationFrame;

    public float Width => State.FrameWidth * BoxScale;

    public float Height => State.FrameHeight * BoxScale;

    public float BoxScale { get; init; }

    public Func<bool> CanBeClicked { get; init; }

    protected IButtonState State { get; set; }

    public Button(float boxScale, Func<bool> canBeClicked)
    {
        BoxScale = boxScale;
        CanBeClicked = canBeClicked;
    }

    public abstract void Initialize();

    public abstract void Click(bool notifyAboutPush = true);

    public override void Update(GameTime gameTime)
    {
        State = State.ProcessState(gameTime);
    }

    public override void Draw(GameTime gameTime)
    {
        Drawer.DrawButton(this);
    }
}
