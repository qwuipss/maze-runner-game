using MazeRunner.Content;
using MazeRunner.Drawing;
using MazeRunner.GameBase.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MazeRunner.GameBase;

public class MazeRunnerGame : Game
{
    private readonly GraphicsDeviceManager _graphics;

    private IGameState _gameState;

    public MazeRunnerGame()
    {
        IsFixedTimeStep = false;
        IsMouseVisible = true;
        Content.RootDirectory = "Content";

        _graphics = new GraphicsDeviceManager(this);
    }

    protected override void Initialize()
    {
        base.Initialize();

        SetFullScreen();
        InitializeDrawer();

        _gameState = new GameMenuState();

        _gameState.Initialize(GraphicsDevice);

        _gameState.GameStateChanged += GameStateChangedHandler;
    }

    protected override void LoadContent()
    {
        Textures.Load(this);
        Fonts.Load(this);
    }

    protected override void Update(GameTime gameTime)
    {
        _gameState.ProcessState(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        _gameState.Draw(gameTime);

        base.Draw(gameTime);
    }

    private void SetFullScreen()
    {
        _graphics.IsFullScreen = true;

        _graphics.PreferredBackBufferWidth = GraphicsDevice.Adapter.CurrentDisplayMode.Width;
        _graphics.PreferredBackBufferHeight = GraphicsDevice.Adapter.CurrentDisplayMode.Height;

        _graphics.ApplyChanges();
    }

    private void InitializeDrawer()
    {
        Drawer.Initialize(this);
    }

    private void GameStateChangedHandler(IGameState gameState)
    {
        _gameState = gameState;

        _gameState.Initialize(GraphicsDevice);
    }
}