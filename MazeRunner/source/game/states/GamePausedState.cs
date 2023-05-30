using MazeRunner.Cameras;
using MazeRunner.Components;
using MazeRunner.Drawing;
using MazeRunner.Gui.Buttons;
using MazeRunner.Helpers;
using MazeRunner.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace MazeRunner.GameBase.States;

public class GamePausedState : IGameState
{
    public event Action<IGameState> GameStateChanged;

    private static Texture2D _cameraEffect;

    private readonly GameRunningState _runningState;

    private GraphicsDevice _graphicsDevice;

    private StaticCamera _staticCamera;

    private int _viewWidth;

    private int _viewHeight;

    private Button _menuButton;

    private Button _restartButton;

    private Button _resumeButton;

    private List<MazeRunnerGameComponent> _components;

    public GamePausedState(GameRunningState runningState)
    {
        _runningState = runningState;
    }

    public void Initialize(GraphicsDevice graphicsDevice, Game game)
    {
        _graphicsDevice = graphicsDevice;

        if (!game.IsMouseVisible)
        {
            game.IsMouseVisible = true;
        }

        var viewPort = graphicsDevice.Viewport;

        _viewWidth = viewPort.Width;
        _viewHeight = viewPort.Height;

        InitializeButtons();
        InitializeStaticCamera();
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
        foreach (var component in _components)
        {
            component.Update(gameTime);
        }

        HandleSecondaryButtons(gameTime);
    }

    private void InitializeButtons()
    {
        void InitializeRestartButton(float scaleDivider)
        {
            var boxScale = _viewWidth / scaleDivider;

            _restartButton = new RestartButton(() => RestartGame(), boxScale);

            _restartButton.Initialize();

            _restartButton.Position = new Vector2((_viewWidth - _restartButton.Width) / 2, (_viewHeight - _restartButton.Height) / 2);
        }

        void InitializeResumeButton(float scaleDivider, float buttonOffsetCoeff)
        {
            var boxScale = _viewWidth / scaleDivider;

            _resumeButton = new ResumeButton(() => ResumeGame(), boxScale);

            _resumeButton.Initialize();

            var restartButtonPosition = _restartButton.Position;

            _resumeButton.Position = new Vector2(restartButtonPosition.X, restartButtonPosition.Y - _restartButton.Height * buttonOffsetCoeff);
        }

        void InitializeMenuButton(float scaleDivider, float buttonOffsetCoeff)
        {
            var boxScale = _viewWidth / scaleDivider;

            _menuButton = new MenuButton(() => GoToMenu(), boxScale);

            _menuButton.Initialize();

            var restartButtonPosition = _restartButton.Position;

            _menuButton.Position = new Vector2(restartButtonPosition.X, restartButtonPosition.Y + _restartButton.Height * buttonOffsetCoeff);
        }

        var buttonsScaleDivider = 400;
        var buttonsOffsetCoeff = 1.5f;

        InitializeRestartButton(buttonsScaleDivider);
        InitializeResumeButton(buttonsScaleDivider, buttonsOffsetCoeff);
        InitializeMenuButton(buttonsScaleDivider, buttonsOffsetCoeff);
    }

    private void InitializeStaticCamera()
    {
        void InitializeCameraEffect()
        {
            var transparency = (byte)(byte.MaxValue / 4.25);

            _cameraEffect = EffectsHelper.CreateTransparentBackground(_viewWidth, _viewHeight, transparency, _graphicsDevice);
        }

        if (_cameraEffect is null)
        {
            InitializeCameraEffect();
        }

        _staticCamera = new StaticCamera(_graphicsDevice)
        {
            Effect = _cameraEffect,
        };
    }

    private void InitializeComponentsList()
    {
        _components = new List<MazeRunnerGameComponent>()
        {
            _resumeButton, _restartButton, _menuButton, _staticCamera,
        };
    }

    private void HandleSecondaryButtons(GameTime gameTime)
    {
        if (KeyboardManager.IsGamePauseSwitched(gameTime))
        {
            ResumeGame();
        }
    }

    private void ResumeGame()
    {
        GameStateChanged.Invoke(_runningState);
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
