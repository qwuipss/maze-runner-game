using MazeRunner.Cameras;
using MazeRunner.Components;
using MazeRunner.Drawing;
using MazeRunner.Drawing.Writers;
using MazeRunner.Gui.Buttons;
using MazeRunner.Helpers;
using MazeRunner.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace MazeRunner.GameBase.States;

public class GameWonState : GameBaseState
{
    public override event Action<IGameState> ControlGiveUpNotify;

    private StaticCamera _staticCamera;

    private GameWonWriter _gameWonWriter;

    private Button _menuButton;

    private HashSet<MazeRunnerGameComponent> _components;

    public override void Initialize(GraphicsDevice graphicsDevice, Game game)
    {
        base.Initialize(graphicsDevice, game);

        TurnOnMouseVisible(game);

        InitializeCamera();
        InitializeShadower();
        InitializeButtons();
        InitializeTextWriters();
        InitializeComponents();
    }

    public override void Draw(GameTime gameTime)
    {
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
    }

    private void InitializeTextWriters()
    {
        _gameWonWriter = new GameWonWriter(ViewWidth, ViewHeight);
    }

    private void InitializeCamera()
    {
        _staticCamera = new StaticCamera(ViewWidth, ViewHeight);
    }

    private void InitializeButtons()
    {
        var scaleDivider = 300;

        var boxScale = ViewWidth / scaleDivider;

        _menuButton = new MenuButton(boxScale);

        _menuButton.Initialize();

        _menuButton.Position = new Vector2((ViewWidth - _menuButton.Width) / 2, (ViewHeight - _menuButton.Height) / 2);

        _menuButton.ButtonPressed += SoundManager.PlayButtonPressedSound;
        _menuButton.ButtonPressed += GoToMenu;
    }

    private void InitializeComponents()
    {
        _components = new HashSet<MazeRunnerGameComponent>
        {
            _menuButton, _gameWonWriter, Shadower,
        };
    }

    private void GoToMenu()
    {
        Shadower = new EffectsHelper.Shadower(false);

        NeedShadowerActivate = true;

        Shadower.TresholdReached += () =>
        {
            ControlGiveUpNotify.Invoke(new GameMenuState());
        };
    }

    private void InitializeShadower()
    {
        Shadower = new EffectsHelper.Shadower(true);

        Shadower.TresholdReached += () => NeedShadowerDeactivate = true;
    }
}
