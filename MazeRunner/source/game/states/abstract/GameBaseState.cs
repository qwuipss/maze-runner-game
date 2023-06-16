using MazeRunner.Components;
using MazeRunner.Gui.Cursors;
using MazeRunner.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace MazeRunner.GameBase.States;

public abstract class GameBaseState : IGameState
{
    public abstract event Action<IGameState> ControlGiveUpNotify;

    protected int ViewWidth { get; private set; }

    protected int ViewHeight { get; private set; }

    protected EffectsHelper.Shadower Shadower { get; set; }

    protected bool NeedShadowerActivate { get; set; }

    protected bool NeedShadowerDeactivate { get; set; }

    protected GraphicsDevice GraphicsDevice { get; private set; }

    protected BaseCursor BaseCursor { get; private set; }

    public abstract void Draw(GameTime gameTime);

    public abstract void Update(GameTime gameTime);

    protected static void TurnOffMouseVisible(Game game)
    {
        if (game.IsMouseVisible)
        {
            game.IsMouseVisible = false;
        }
    }

    public virtual void Initialize(GraphicsDevice graphicsDevice, Game game)
    {
        GraphicsDevice = graphicsDevice;

        InitializeViewDimensions();

        BaseCursor = new BaseCursor(ViewWidth * .45f, ViewWidth);
    }

    private void InitializeViewDimensions()
    {
        var viewPort = GraphicsDevice.Viewport;

        ViewWidth = viewPort.Width;
        ViewHeight = viewPort.Height;
    }

    protected void ProcessShadowerState(HashSet<MazeRunnerGameComponent> components)
    {
        if (NeedShadowerActivate)
        {
            NeedShadowerActivate = false;

            components.Add(Shadower);
        }
        else if (NeedShadowerDeactivate)
        {
            NeedShadowerDeactivate = false;

            components.Remove(Shadower);
        }
    }
}
