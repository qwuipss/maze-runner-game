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
    public static event Action GameRestartedNotify;

    public static event Action GameMenuReturnedNotify;

    public override event Action<IGameState> ControlGiveUpNotify;

    private readonly GameRunningState _runningState;

    private readonly float _cameraEffectTransparency;

    private Button _restartButton;

    private Button _menuButton;

    private StaticCamera _staticCamera;

    private HashSet<MazeRunnerGameComponent> _components;

    private bool _canButtonsBeClicked;

    public GameOverState(GameRunningState runningState, float cameraEffectTransparency)
    {
        _canButtonsBeClicked = true;

        Shadower = new EffectsHelper.Shadower(false);

        _runningState = runningState;

        _cameraEffectTransparency = cameraEffectTransparency;

        runningState.IsControlling = false;
    }

    public override void Initialize(GraphicsDevice graphicsDevice, Game game)
    {
        base.Initialize(graphicsDevice, game);

        InitializeCameras();
        InitializeButtons();
        InitializeComponents();
    }

    public override void Draw(GameTime gameTime)
    {
        _runningState.Draw(gameTime);

        Drawer.BeginDraw(_staticCamera);

        Drawer.DrawCursor(BaseCursor);

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

        ProcessShadowerState(_components);
    }

    private void InitializeButtons()
    {
        void InitializeRestartButton(float scaleDivider)
        {
            var boxScale = ViewWidth / scaleDivider;

            _restartButton = new RestartButton(boxScale, () => _canButtonsBeClicked);

            _restartButton.Initialize();

            _restartButton.Position = new Vector2(ViewWidth / 3 - _restartButton.Width / 2, ViewHeight / 2);

            _restartButton.ButtonPressedNotify += RestartGame;
        }

        void InitializeMenuButton(float scaleDivider)
        {
            var boxScale = ViewWidth / scaleDivider;

            _menuButton = new MenuButton(boxScale, () => _canButtonsBeClicked);

            _menuButton.Initialize();

            _menuButton.Position = new Vector2(2 * ViewWidth / 3 - _menuButton.Width / 2, ViewHeight / 2);

            _menuButton.ButtonPressedNotify += GoToMenu;
        }

        var buttonsScaleDivider = 360;

        InitializeRestartButton(buttonsScaleDivider);
        InitializeMenuButton(buttonsScaleDivider);
    }

    private void InitializeCameras()
    {
        var heroCamera = _runningState.HeroCamera;

        heroCamera.Effect = EffectsHelper.Shadower.BlackBackground;
        heroCamera.EffectTransparency = _cameraEffectTransparency;

        _staticCamera = new StaticCamera(ViewWidth, ViewHeight);
    }

    private void InitializeComponents()
    {
        _components = new HashSet<MazeRunnerGameComponent>
        {
            _restartButton, _menuButton,
        };
    }

    private void RestartGame()
    {
        _canButtonsBeClicked = false;
        NeedShadowerActivate = true;

        Shadower.TresholdReached += () => ControlGiveUpNotify.Invoke(new GameRunningState(_runningState.GameParameters));

        GameRestartedNotify.Invoke();
    }

    private void GoToMenu()
    {
        GameRunningState.StopPlayingMusic();

        _canButtonsBeClicked = false;
        NeedShadowerActivate = true;

        GameMenuReturnedNotify.Invoke();
        Shadower.TresholdReached += () => ControlGiveUpNotify.Invoke(new GameMenuState());
    }
}
