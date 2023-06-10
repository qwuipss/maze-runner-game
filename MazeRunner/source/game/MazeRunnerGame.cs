using MazeRunner.Content;
using MazeRunner.Drawing;
using MazeRunner.GameBase.States;
using MazeRunner.Helpers;
using MazeRunner.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MazeRunner.GameBase;

public class MazeRunnerGame : Game
{
    private const int MinMusicPlayingPercentage = 30;

    private const int MaxMusicPlayingPercentage = 100;

    private readonly GraphicsDeviceManager _graphics;

    private IGameState _gameState;

    public MazeRunnerGame()
    {
        IsFixedTimeStep = true;
        IsMouseVisible = true;

        Content.RootDirectory = "Content";

        _graphics = new GraphicsDeviceManager(this);
    }

    protected override void Initialize()
    {
        void InitializeShadower()
        {
            var viewport = GraphicsDevice.Viewport;

            var width = viewport.Width;
            var height = viewport.Height;

            EffectsHelper.Shadower.InitializeBlackBackground(width, height, GraphicsDevice);
        }

        void InitializeDrawer()
        {
            Drawer.Initialize(this);
        }

        base.Initialize();

        //SetFullScreen();
        InitializeDrawer();
        InitializeMusic();
        InitializeShadower();

        _gameState = new GameMenuState();

        _gameState.Initialize(GraphicsDevice, this);

        _gameState.ControlGiveUpNotify += ControlGiveUpHandler;
    }

    protected override void LoadContent()
    {
        Textures.Load(this);
        Fonts.Load(this);
        Sounds.Load(this);
    }

    protected override void Update(GameTime gameTime)
    {
        _gameState.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        _gameState.Draw(gameTime);

        base.Draw(gameTime);
    }

    private static void InitializeMusic()
    {
        GameMenuState.MenuEnteredNotify += async () => await SoundManager.PlayGameMenuMusicAsync(GetRandomMusicPlayingPercentage());
        GameMenuState.MenuLeavedNotify += async () => await SoundManager.StopPlayingGameMenuMusicAsync();

        GameRunningState.GameStartedNotify += async () => await SoundManager.PlayGameRunningMusicAsync(GetRandomMusicPlayingPercentage());
        GameRunningState.GameOveredNotify += async () => await SoundManager.StopPlayingGameRunningMusicAsync();
    }

    private static int GetRandomMusicPlayingPercentage()
    {
        return RandomHelper.Next(MinMusicPlayingPercentage, MaxMusicPlayingPercentage);
    }

    private void SetFullScreen()
    {
        _graphics.IsFullScreen = true;

        _graphics.PreferredBackBufferWidth = GraphicsDevice.Adapter.CurrentDisplayMode.Width;
        _graphics.PreferredBackBufferHeight = GraphicsDevice.Adapter.CurrentDisplayMode.Height;

        _graphics.ApplyChanges();
    }


    private void ControlGiveUpHandler(IGameState gameState)
    {
        _gameState = gameState;

        _gameState.Initialize(GraphicsDevice, this);

        _gameState.ControlGiveUpNotify += ControlGiveUpHandler;
    }
}