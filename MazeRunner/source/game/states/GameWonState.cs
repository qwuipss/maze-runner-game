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

    private Texture2D _cameraEffect;

    private GameWonWriter _gameWonWriter;

    private Button _menuButton;

    private List<MazeRunnerGameComponent> _components;

    public override void Initialize(GraphicsDevice graphicsDevice, Game game)
    {
        base.Initialize(graphicsDevice, game);

        TurnOnMouseVisible(game);

        InitializeCamera();
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
    }

    private void InitializeTextWriters()
    {
        _gameWonWriter = new GameWonWriter(ViewWidth, ViewHeight);
    }

    private void InitializeCamera()
    {
        void InitializeCameraEffect()
        {
            _cameraEffect = EffectsHelper.CreateTransparentBackground(ViewWidth, ViewHeight, 255, GraphicsDevice);
        }

        if (_cameraEffect is null)
        {
            InitializeCameraEffect();
        }

        _staticCamera = new StaticCamera(ViewWidth, ViewHeight)
        {
            Effect = _cameraEffect,
            DrawingPriority = .5f,
        };
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
        _components = new List<MazeRunnerGameComponent>
        {
            _staticCamera, _menuButton, _gameWonWriter,
        };
    }

    private void GoToMenu()
    {
        GameStateChanged.Invoke(new GameMenuState());
    }
}
