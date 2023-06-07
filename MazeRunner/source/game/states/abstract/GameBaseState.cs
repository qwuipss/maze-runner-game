using MazeRunner.Components;
using MazeRunner.Helpers;
using MazeRunner.MazeBase;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using static System.Net.Mime.MediaTypeNames;

namespace MazeRunner.GameBase.States;

public abstract class GameBaseState : IGameState
{
    private const int UpdateAreaWidthRadius = 7;

    private const int UpdateAreaHeightRadius = 5;

    public abstract event Action<IGameState> ControlGiveUpNotify;

    protected GraphicsDevice GraphicsDevice { get; set; }

    protected int ViewWidth { get; set; }

    protected int ViewHeight { get; set; }

    protected EffectsHelper.Shadower Shadower { get; set; }

    protected bool NeedShadowerActivate { get; set; }

    protected bool NeedShadowerDeactivate { get; set; }

    public abstract void Draw(GameTime gameTime);

    public abstract void Update(GameTime gameTime);

    protected static void TurnOffMouseVisible(Game game)
    {
        if (game.IsMouseVisible)
        {
            game.IsMouseVisible = false;
        }
    }

    protected static void TurnOnMouseVisible(Game game)
    {
        if (!game.IsMouseVisible)
        {
            game.IsMouseVisible = true;
        }
    }

    public virtual void Initialize(GraphicsDevice graphicsDevice, Game game)
    {
        GraphicsDevice = graphicsDevice;

        InitializeViewDimensions();
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
