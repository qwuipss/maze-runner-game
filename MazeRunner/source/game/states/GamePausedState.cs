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

public class GamePausedState : GameBaseState
{
    private const float PauseGameVolumePercentageChange = -50;

    private const float ResumeGameVolumePercentageChange = 100;

    private readonly GameRunningState _runningState;

    public static event Action GamePausedNotify;

    public override event Action<IGameState> ControlGiveUpNotify;

    private StaticCamera _staticCamera;

    private Button _menuButton;

    private Button _restartButton;

    private Button _resumeButton;

    private HashSet<MazeRunnerGameComponent> _components;

    private bool _handleSecondaryButtons;

    private bool _canButtonsBeClicked;

    public GamePausedState(GameRunningState runningState)
    {
        _canButtonsBeClicked = true;

        _runningState = runningState;

        _handleSecondaryButtons = true;

        GameRunningState.GameRunningMusic.ChangeVolume(PauseGameVolumePercentageChange);

        GamePausedNotify.Invoke();
    }

    public override void Initialize(GraphicsDevice graphicsDevice, Game game)
    {
        base.Initialize(graphicsDevice, game);

        InitializeButtons();
        InitializeShadower();
        InitializeStaticCamera();
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
        foreach (var component in _components)
        {
            component.Update(gameTime);
        }

        ProcessShadowerState(_components);

        if (_handleSecondaryButtons)
        {
            HandleSecondaryButtons(gameTime);
        }
    }

    private void InitializeButtons()
    {
        void InitializeRestartButton(float scaleDivider)
        {
            var boxScale = ViewWidth / scaleDivider;

            _restartButton = new RestartButton(boxScale, () => _canButtonsBeClicked);

            _restartButton.Initialize();

            _restartButton.Position = new Vector2((ViewWidth - _restartButton.Width) / 2, (ViewHeight - _restartButton.Height) / 2);

            _restartButton.ButtonPressedNotify += RestartGame;
            _restartButton.ButtonPressedNotify += () => _handleSecondaryButtons = false;
        }

        void InitializeResumeButton(float scaleDivider, float buttonOffsetCoeff)
        {
            var boxScale = ViewWidth / scaleDivider;

            _resumeButton = new ResumeButton(boxScale, () => _canButtonsBeClicked);

            _resumeButton.Initialize();

            var restartButtonPosition = _restartButton.Position;

            _resumeButton.Position = new Vector2(restartButtonPosition.X, restartButtonPosition.Y - _restartButton.Height * buttonOffsetCoeff);

            _resumeButton.ButtonPressedNotify += ResumeGame;
            _resumeButton.ButtonPressedNotify += () => _handleSecondaryButtons = false;
        }

        void InitializeMenuButton(float scaleDivider, float buttonOffsetCoeff)
        {
            var boxScale = ViewWidth / scaleDivider;

            _menuButton = new MenuButton(boxScale, () => _canButtonsBeClicked);

            _menuButton.Initialize();

            var restartButtonPosition = _restartButton.Position;

            _menuButton.Position = new Vector2(restartButtonPosition.X, restartButtonPosition.Y + _restartButton.Height * buttonOffsetCoeff);

            _menuButton.ButtonPressedNotify += GoToMenu;
            _menuButton.ButtonPressedNotify += () => _handleSecondaryButtons = false;
        }

        var buttonsScaleDivider = 400;
        var buttonsOffsetCoeff = 1.5f;

        InitializeRestartButton(buttonsScaleDivider);
        InitializeResumeButton(buttonsScaleDivider, buttonsOffsetCoeff);
        InitializeMenuButton(buttonsScaleDivider, buttonsOffsetCoeff);
    }

    private void InitializeShadower()
    {
        Shadower = new EffectsHelper.Shadower(false);
    }

    private void InitializeStaticCamera()
    {
        _staticCamera = new StaticCamera(ViewWidth, ViewHeight)
        {
            Effect = EffectsHelper.Shadower.BlackBackground,
            EffectTransparency = 1 / 4.25f,
        };
    }

    private void InitializeComponents()
    {
        _components = new HashSet<MazeRunnerGameComponent>
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
        _canButtonsBeClicked = false;

        GameRunningState.GameRunningMusic.ChangeVolume(ResumeGameVolumePercentageChange);

        ControlGiveUpNotify.Invoke(_runningState);
    }

    private void RestartGame()
    {
        _canButtonsBeClicked = false;
        NeedShadowerActivate = true;

        Shadower.TresholdReached += () => ControlGiveUpNotify.Invoke(new GameRunningState(_runningState.GameParameters));
    }

    private void GoToMenu()
    {
        GameRunningState.StopPlayingMusic();

        _canButtonsBeClicked = false;
        NeedShadowerActivate = true;

        Shadower.TresholdReached += () => ControlGiveUpNotify.Invoke(new GameMenuState());
    }
}
