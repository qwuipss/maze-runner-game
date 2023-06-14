using MazeRunner.Content;
using MazeRunner.Drawing;
using MazeRunner.GameBase.States;
using MazeRunner.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace MazeRunner.GameBase;

public class MazeRunnerGame : Game
{
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
        InitializeShadower();

        Activated += (_, _) => SoundEffect.MasterVolume = 1;
        Deactivated += (_, _) => SoundEffect.MasterVolume = 0;

        HandleGiveUpControl(new GameMenuState());
    }

    protected override void LoadContent()
    {
        Textures.Load(this);
        Fonts.Load(this);
        Sounds.Load(this);
    }

    protected override void Update(GameTime gameTime)
    {
        if (IsActive)
        {
            _gameState.Update(gameTime);
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        if (IsActive)
        {
            GraphicsDevice.Clear(Color.Black);

            _gameState.Draw(gameTime);
        }

        base.Draw(gameTime);
    }

    private void SetFullScreen()
    {
        _graphics.IsFullScreen = true;

        _graphics.PreferredBackBufferWidth = GraphicsDevice.Adapter.CurrentDisplayMode.Width;
        _graphics.PreferredBackBufferHeight = GraphicsDevice.Adapter.CurrentDisplayMode.Height;

        _graphics.ApplyChanges();
    }

    private void HandleGiveUpControl(IGameState gameState)
    {
        _gameState = gameState;

        _gameState.Initialize(GraphicsDevice, this);

        _gameState.ControlGiveUpNotify += HandleGiveUpControl;
    }
}