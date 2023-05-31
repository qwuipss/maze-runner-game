using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MazeRunner.GameBase.States;

public abstract class GameBaseState : IGameState
{
    public abstract event Action<IGameState> GameStateChanged;

    protected GraphicsDevice GraphicsDevice { get; set; }

    protected int ViewWidth { get; set; }

    protected int ViewHeight { get; set; }

    public virtual void Initialize(GraphicsDevice graphicsDevice, Game game)
    {
        GraphicsDevice = graphicsDevice;

        InitializeViewDimensions();
    }

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

    private void InitializeViewDimensions()
    {
        var viewPort = GraphicsDevice.Viewport;

        ViewWidth = viewPort.Width;
        ViewHeight = viewPort.Height;
    }
}
