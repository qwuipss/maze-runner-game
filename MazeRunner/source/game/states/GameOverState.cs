using MazeRunner.Cameras;
using MazeRunner.Components;
using MazeRunner.Drawing;
using MazeRunner.Gui.Buttons;
using MazeRunner.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace MazeRunner.GameBase.States;

public class GameOverState : GameBaseState
{
    public override event Action<IGameState> GameStateChanged;

    private static Texture2D _cameraEffect;

    private readonly GameRunningState _runningState;

    private Button _restartButton;

    private Button _menuButton;

    private StaticCamera _staticCamera;

    private List<MazeRunnerGameComponent> _components;

    public GameOverState(GameRunningState runningState)
    {
        _runningState = runningState;

        runningState.IsControlling = false;
    }

    public override void Initialize(GraphicsDevice graphicsDevice, Game game)
    {
        base.Initialize(graphicsDevice, game);

        TurnOnMouseVisible(game);

        InitializeCameras();
        InitializeButtons();
        InitializeComponentsList();
    }

    public override void Draw(GameTime gameTime)
    {
        _runningState.Draw(gameTime);

        Drawer.BeginDraw(_staticCamera);

        foreach (var component in _components)
        {
            component.Draw(gameTime);
        }

        Drawer.EndDraw();
    }

    public override void Update(GameTime gameTime)
    {
        _runningState.Update(gameTime);

        foreach (var component in _components)
        {
            component.Update(gameTime);
        }
    }

    private void InitializeButtons()
    {
        void InitializeRestartButton(float scaleDivider)
        {
            var boxScale = ViewWidth / scaleDivider;

            _restartButton = new RestartButton(() => RestartGame(), boxScale);

            _restartButton.Initialize();

            _restartButton.Position = new Vector2(ViewWidth / 3 - _restartButton.Width / 2, ViewHeight / 2);
        }

        void InitializeMenuButton(float scaleDivider)
        {
            var boxScale = ViewWidth / scaleDivider;

            _menuButton = new MenuButton(() => GoToMenu(), boxScale);

            _menuButton.Initialize();

            _menuButton.Position = new Vector2(2 * ViewWidth / 3 - _menuButton.Width / 2, ViewHeight / 2); ;
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

            _cameraEffect = EffectsHelper.CreateTransparentBackground(ViewWidth, ViewHeight, transparency, GraphicsDevice);
        }

        if (_cameraEffect is null)
        {
            InitializeCameraEffect();
        }

        _runningState.HeroCamera.Effect = _cameraEffect;

        _staticCamera = new StaticCamera(ViewWidth, ViewHeight);
    }

    private void InitializeComponentsList()
    {
        _components = new List<MazeRunnerGameComponent>
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
