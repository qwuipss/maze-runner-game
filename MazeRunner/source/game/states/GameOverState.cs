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

    private ButtonInfo _restartButtonInfo;

    private ButtonInfo _menuButtonInfo;

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
            var restartButton = new RestartButton(() => RestartGame());

            var restartButtonBoxScale = _viewWidth / scaleDivider;

            _restartButtonInfo = new ButtonInfo(restartButton, restartButtonBoxScale);

            restartButton.Initialize(_restartButtonInfo);

            var restartButtonPosition = new Vector2(_viewWidth / 3 - restartButton.Width / 2, _viewHeight / 2);

            _restartButtonInfo.Position = restartButtonPosition;
        }

        void InitializeMenuButton(int scaleDivider)
        {
            var menuButton = new MenuButton(() => GoToMenu());

            var menuButtonBoxScale = _viewWidth / scaleDivider;

            _menuButtonInfo = new ButtonInfo(menuButton, menuButtonBoxScale);

            menuButton.Initialize(_menuButtonInfo);

            var menuButtonPosition = new Vector2(2 * _viewWidth / 3 - menuButton.Width / 2, _viewHeight / 2);

            _menuButtonInfo.Position = menuButtonPosition;
        }

        var buttonsScaleDivider = 360;

        InitializeRestartButton(buttonsScaleDivider);
        InitializeMenuButton(buttonsScaleDivider);
    }

    private void InitializeCameras()
    {
        if (_cameraEffect is null)
        {
            var transparency = (byte)(byte.MaxValue / 1.35);

            _cameraEffect = EffectsHelper.CreateTransparentBackground(_viewWidth, _viewHeight, transparency, _graphicsDevice);
        }

        _runningState.HeroCamera.Effect = _cameraEffect;

        _staticCamera = new StaticCamera(_graphicsDevice);
    }

    private void InitializeComponentsList()
    {
        _components = new List<MazeRunnerGameComponent>()
        {
            _restartButtonInfo, _menuButtonInfo,
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
