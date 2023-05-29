using MazeRunner.Cameras;
using MazeRunner.Components;
using MazeRunner.Drawing;
using MazeRunner.Gui.Buttons;
using MazeRunner.Helpers;
using MazeRunner.Managers;
using MazeRunner.Wrappers;
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

    private ButtonInfo _menuButtonInfo;

    private ButtonInfo _restartButtonInfo;

    private ButtonInfo _resumeButtonInfo;

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
        void InitializeRestartButton(int scaleDivider)
        {
            var restartButton = new RestartButton(() => RestartGame());

            var restartButtonBoxScale = _viewWidth / scaleDivider;

            _restartButtonInfo = new ButtonInfo(restartButton, restartButtonBoxScale);

            restartButton.Initialize(_restartButtonInfo);

            var heroInfo = _runningState.HeroInfo;

            var hero = heroInfo.Sprite;
            var heroPosition = heroInfo.Position;

            var restartButtonPosition = new Vector2((_viewWidth - restartButton.Width) / 2, (_viewHeight - restartButton.Height) / 2);

            _restartButtonInfo.Position = restartButtonPosition;
        }

        void InitializeResumeButton(int scaleDivider, float buttonOffsetCoeff)
        {
            var resumeButton = new ResumeButton(() => ResumeGame());

            var resumeButtonBoxScale = _viewWidth / scaleDivider;

            _resumeButtonInfo = new ButtonInfo(resumeButton, resumeButtonBoxScale);

            resumeButton.Initialize(_resumeButtonInfo);

            var restartButton = _restartButtonInfo.Button;
            var restartButtonPosition = _restartButtonInfo.Position;

            var resumeButtonPosition = new Vector2(restartButtonPosition.X, restartButtonPosition.Y - restartButton.Height * buttonOffsetCoeff);

            _resumeButtonInfo.Position = resumeButtonPosition;
        }

        void InitializeMenuButton(int scaleDivider, float buttonOffsetCoeff)
        {
            var menuButton = new MenuButton(() => GoToMenu());

            var menuButtonBoxScale = _viewWidth / scaleDivider;

            _menuButtonInfo = new ButtonInfo(menuButton, menuButtonBoxScale);

            menuButton.Initialize(_menuButtonInfo);

            var restartButton = _restartButtonInfo.Button;
            var restartButtonPosition = _restartButtonInfo.Position;

            var menuButtonPosition = new Vector2(restartButtonPosition.X, restartButtonPosition.Y + restartButton.Height * buttonOffsetCoeff);

            _menuButtonInfo.Position = menuButtonPosition;
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
            _resumeButtonInfo, _restartButtonInfo, _menuButtonInfo, _staticCamera,
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
