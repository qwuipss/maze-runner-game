using MazeRunner.Content;
using MazeRunner.Drawing;
using MazeRunner.GameBase;
using MazeRunner.GameBase.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MazeRunner;

public class MazeRunnerGame : Game
{
    private readonly GraphicsDeviceManager _graphics;

    private IGameState _gameState;

    public MazeRunnerGame()
    {
        IsFixedTimeStep = false;
        IsMouseVisible = false;
        Content.RootDirectory = "Content";

        _graphics = new GraphicsDeviceManager(this);
    }

    protected override void Initialize()
    {
        base.Initialize();

        SetFullScreen();
        InitializeDrawer();

        _gameState = new GameRunningState(new GameParameters()
        {
            MazeWidth = 9,
            MazeHeight = 9,

            MazeDeadEndsRemovePercentage = 50,

            MazeBayonetTrapInsertingPercentage = 3,
            MazeDropTrapInsertingPercentage = 2,

            HeroCameraScaleFactor = 7,
            HeroCameraShadowTresholdCoeff = 2.4f,

            GuardSpawnCount = 1,
            GuardHalfHeartsDamage = 1,

            HeroHalfHeartsHealth = 6,

            GraphicsDevice = GraphicsDevice,
        });
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
}