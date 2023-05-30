using MazeRunner.Cameras;
using MazeRunner.Components;
using MazeRunner.Drawing;
using MazeRunner.Gui.Buttons;
using MazeRunner.Helpers;
using MazeRunner.Wrappers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace MazeRunner.GameBase.States;

public class GameOverState : IGameState
{
    public event Action<IGameState> GameStateChanged;

    private static Texture2D _cameraEffect;

    private readonly GameRunningState _runningState;

    private GraphicsDevice _graphicsDevice;

    private int _viewWidth;

    private int _viewHeight;

    private Button _restartButton;

    private Button _menuButton;

    private StaticCamera _staticCamera;

    private List<MazeRunnerGameComponent> _components;

    public GameOverState(GameRunningState runningState)
    {
        _runningState = runningState;

        runningState.IsControlling = false;
    }

    public void Initialize(GraphicsDevice graphicsDevice, Game game)
    {
        _graphicsDevice = graphicsDevice;

        if (!game.IsMouseVisible)
        {
            game.IsMouseVisible = true;
        }

        var viewPort = _graphicsDevice.Viewport;

        _viewWidth = viewPort.Width;
        _viewHeight = viewPort.Height;

        InitializeCameras();
        InitializeButtons();
        InitializeComponentsList();
    }

    public void Draw(GameTime gameTime)
    {
        _runningState.Draw(gameTime);

        Drawer.BeginDraw(_staticCamera);

        foreach (var component in _components)
        {
            component.Draw(gameTime);
        }

        Drawer.EndDraw();
    }

    public void Update(GameTime gameTime)
    {
        _runningState.Update(gameTime);

        foreach (var component in _components)
        {
            component.Update(gameTime);
        }
    }

    private void InitializeButtons()
    {
        void InitializeRestartButton(int scaleDivider)
        {
            var boxScale = _viewWidth / scaleDivider;

            _restartButton = new RestartButton(() => RestartGame(), boxScale);

            _restartButton.Initialize();

            _restartButton.Position = new Vector2(_viewWidth / 3 - _restartButton.Width / 2, _viewHeight / 2);
        }

        void InitializeMenuButton(int scaleDivider)
        {
            var boxScale = _viewWidth / scaleDivider;
            
            _menuButton = new MenuButton(() => GoToMenu(), boxScale);

            _menuButton.Initialize();

            _menuButton.Position = new Vector2(2 * _viewWidth / 3 - _menuButton.Width / 2, _viewHeight / 2); ;
        }

        var buttonsScaleDivider = 360;

        InitializeRestartButton(buttonsScaleDivider);
        InitializeMenuButton(buttonsScaleDivider);
    }

    private void InitializeCameras()
    {
        void InitializeCameraEffect()
        {
            var transparency = (byte)(byte.MaxValue / 1.35);

            _cameraEffect = EffectsHelper.CreateTransparentBackground(_viewWidth, _viewHeight, transparency, _graphicsDevice);
        }

        if (_cameraEffect is null)
        {
            InitializeCameraEffect();
        }

        _runningState.HeroCamera.Effect = _cameraEffect;

        _staticCamera = new StaticCamera(_graphicsDevice);
    }

    private void InitializeComponentsList()
    {
        _components = new List<MazeRunnerGameComponent>()
        {
            _restartButton, _menuButton,
        };
    }

    private void RestartGame()
    {
        GameStateChanged.Invoke(new GameRunningState(_runningState.GameParameters));
    }

    private void GoToMenu()
    {
        GameStateChanged.Invoke(new GameMenuState());
    }
}
