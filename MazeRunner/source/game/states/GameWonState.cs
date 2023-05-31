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

namespace MazeRunner.GameBase.States;

public class GameWonState : GameBaseState
{
    public override event Action<IGameState> GameStateChanged;

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
        InitializeComponentsList();
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

        _menuButton = new MenuButton(() => GoToMenu(), boxScale);

        _menuButton.Initialize();

        _menuButton.Position = new Vector2((ViewWidth - _menuButton.Width) / 2, (ViewHeight - _menuButton.Height) / 2);
    }

    private void InitializeComponentsList()
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
            GameStateChanged.Invoke(new GameMenuState());
        };
    }

    private void InitializeShadower()
    {
        Shadower = new EffectsHelper.Shadower(true);

        Shadower.TresholdReached += () => NeedShadowerDeactivate = true;
    }
}
