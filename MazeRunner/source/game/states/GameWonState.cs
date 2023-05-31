using MazeRunner.Cameras;
using MazeRunner.Components;
using MazeRunner.Drawing;
using MazeRunner.Drawing.Writers;
using MazeRunner.Gui.Buttons;
using MazeRunner.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeRunner.GameBase.States;

public class GameWonState : IGameState
{
    public event Action<IGameState> GameStateChanged;

    private GraphicsDevice _graphicsDevice;

    private StaticCamera _staticCamera;

    private Texture2D _cameraEffect;

    private GameWonWriter _gameWonWriter;

    private int _viewWidth;

    private int _viewHeight;

    private Button _menuButton;

    private List<MazeRunnerGameComponent> _components;

    public void Initialize(GraphicsDevice graphicsDevice, Game game)
    {
        if (!game.IsMouseVisible)
        {
            game.IsMouseVisible = true;
        }

        _graphicsDevice = graphicsDevice;

        var viewPort = _graphicsDevice.Viewport;

        _viewWidth = viewPort.Width;
        _viewHeight = viewPort.Height;

        InitializeCamera();
        InitializeButtons();
        InitializeTextWriters();
        InitializeComponentsList();
    }

    public void Draw(GameTime gameTime)
    {
        Drawer.BeginDraw(_staticCamera);

        foreach (var component in _components)
        {
            component.Draw(gameTime);
        }

        Drawer.EndDraw();
    }

    public void Update(GameTime gameTime)
    {
        foreach (var component in _components)
        {
            component.Update(gameTime);
        }
    }

    private void InitializeTextWriters()
    {
        _gameWonWriter = new GameWonWriter(_viewWidth, _viewHeight);
    }

    private void InitializeCamera()
    {
        void InitializeCameraEffect()
        {
            _cameraEffect = EffectsHelper.CreateTransparentBackground(_viewWidth, _viewHeight, byte.MaxValue, _graphicsDevice);
        }

        if (_cameraEffect is null)
        {
            InitializeCameraEffect();
        }

        _staticCamera = new StaticCamera(_graphicsDevice)
        {
            Effect = _cameraEffect,
            DrawingPriority = .5f,
        };
    }

    private void InitializeButtons()
    {
        var scaleDivider = 300;

        var boxScale = _viewWidth / scaleDivider;

        _menuButton = new MenuButton(() => GoToMenu(), boxScale);

        _menuButton.Initialize();

        _menuButton.Position = new Vector2((_viewWidth - _menuButton.Width) / 2, (_viewHeight - _menuButton.Height) / 2);
    }

    private void InitializeComponentsList()
    {
        _components = new List<MazeRunnerGameComponent>
        {
            _staticCamera, _menuButton, _gameWonWriter,
        };
    }

    private void GoToMenu()
    {
        GameStateChanged.Invoke(new GameMenuState());
    }
}
