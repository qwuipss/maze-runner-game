﻿using MazeRunner.Cameras;
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
    public override event Action<IGameState> ControlGiveUpNotify;

    private readonly GameRunningState _runningState;

    private StaticCamera _staticCamera;

    private Button _menuButton;

    private Button _restartButton;

    private Button _resumeButton;

    private HashSet<MazeRunnerGameComponent> _components;

    public GamePausedState(GameRunningState runningState)
    {
        _runningState = runningState;
    }

    public override void Initialize(GraphicsDevice graphicsDevice, Game game)
    {
        base.Initialize(graphicsDevice, game);

        TurnOnMouseVisible(game);

        InitializeButtons();
        InitializeShadower();
        InitializeStaticCamera();
        InitializeComponents();
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
        foreach (var component in _components)
        {
            component.Update(gameTime);
        }

        ProcessShadowerState(_components);
        HandleSecondaryButtons(gameTime);
    }

    private void InitializeButtons()
    {
        void InitializeRestartButton(float scaleDivider)
        {
            var boxScale = ViewWidth / scaleDivider;

            _restartButton = new RestartButton(boxScale);

            _restartButton.Initialize();

            _restartButton.Position = new Vector2((ViewWidth - _restartButton.Width) / 2, (ViewHeight - _restartButton.Height) / 2);

            _restartButton.ButtonPressed += SoundManager.PlayButtonPressedSound;
            _restartButton.ButtonPressed += RestartGame;
        }

        void InitializeResumeButton(float scaleDivider, float buttonOffsetCoeff)
        {
            var boxScale = ViewWidth / scaleDivider;

            _resumeButton = new ResumeButton(boxScale);

            _resumeButton.Initialize();

            var restartButtonPosition = _restartButton.Position;

            _resumeButton.Position = new Vector2(restartButtonPosition.X, restartButtonPosition.Y - _restartButton.Height * buttonOffsetCoeff);

            _resumeButton.ButtonPressed += SoundManager.PlayButtonPressedSound;
            _resumeButton.ButtonPressed += ResumeGame;
        }

        void InitializeMenuButton(float scaleDivider, float buttonOffsetCoeff)
        {
            var boxScale = ViewWidth / scaleDivider;

            _menuButton = new MenuButton(boxScale);

            _menuButton.Initialize();

            var restartButtonPosition = _restartButton.Position;

            _menuButton.Position = new Vector2(restartButtonPosition.X, restartButtonPosition.Y + _restartButton.Height * buttonOffsetCoeff);

            _menuButton.ButtonPressed += SoundManager.PlayButtonPressedSound;
            _menuButton.ButtonPressed += GoToMenu;
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
        ControlGiveUpNotify.Invoke(_runningState);
    }

    private void RestartGame()
    {
        Shadower.TresholdReached += () => ControlGiveUpNotify.Invoke(new GameRunningState(_runningState.GameParameters));

        NeedShadowerActivate = true;
    }

    private void GoToMenu()
    {
        Shadower.TresholdReached += () => ControlGiveUpNotify.Invoke(new GameMenuState());

        NeedShadowerActivate = true;
    }
}
