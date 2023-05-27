using MazeRunner.Cameras;
using MazeRunner.Components;
using MazeRunner.Content;
using MazeRunner.Drawing;
using MazeRunner.Gui.Buttons;
using MazeRunner.Gui.Buttons.States;
using MazeRunner.Wrappers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace MazeRunner.GameBase.States;

public class GameMenuState : IGameState
{
    private static class GameModes
    {
        private static readonly Lazy<GameParameters> _easy;

        public static GameParameters Easy
        {
            get
            {
                return _easy.Value;
            }
        }

        public static GameParameters Medium;
        public static GameParameters Hard;

        static GameModes()
        {
            _easy = new Lazy<GameParameters>(() => new GameParameters()
            {
                MazeWidth = 9,
                MazeHeight = 9,

                MazeDeadEndsRemovePercentage = 50,

                MazeBayonetTrapInsertingPercentage = 3,
                MazeDropTrapInsertingPercentage = 2,

                HeroCameraScaleFactor = 7,
                HeroCameraShadowTresholdCoeff = 2.4f,

                GuardSpawnCount = 1,
                GuardHalfHeartsDamage = 1,

                HeroHalfHeartsHealth = 6,
            });
        }
    }

    public event Action<IGameState> GameStateChanged;

    private GraphicsDevice _graphicsDevice;

    private ButtonInfo _startButtonInfo;

    private MenuCamera _menuCamera;

    private List<MazeRunnerGameComponent> _components;

    public void Initialize(GraphicsDevice graphicsDevice)
    {
        _graphicsDevice = graphicsDevice;

        InitializeMenuButtons();
        InitializeMenuCamera();
        InitializeComponentsList();
    }

    public void Draw(GameTime gameTime)
    {
        Drawer.BeginDraw(_menuCamera);

        foreach (var component in _components)
        {
            component.Draw(gameTime);
        }

        Drawer.EndDraw();
    }

    public void ProcessState(GameTime gameTime)
    {
        foreach (var component in _components)
        {
            component.Update(gameTime);
        }
    }

    private void InitializeMenuButtons()
    {
        void InitializeGameStartButton(int windowWidth, int windowHeight)
        {
            var startButton = new Button(() => GameStateChanged.Invoke(new GameRunningState(GameModes.Easy)));

            var startButtonBoxScale = 5;

            _startButtonInfo = new ButtonInfo(startButton, startButtonBoxScale,
                new ButtonStateInfo
                {
                    Texture = Textures.Gui.Buttons.Start.Idle,
                    FramesCount = 1,
                },
                new ButtonStateInfo
                {
                    Texture = Textures.Gui.Buttons.Start.Hover,
                    FramesCount = 1,
                },
                new ButtonStateInfo
                {
                    Texture = Textures.Gui.Buttons.Start.Click,
                    FramesCount = 5,
                });

            startButton.Initialize(_startButtonInfo);

            var startButtonPosition = new Vector2((windowWidth - startButton.Width) / 2, (windowHeight - startButton.Height) / 2);
            _startButtonInfo.Position = startButtonPosition;
        }

        var windowWidth = _graphicsDevice.Adapter.CurrentDisplayMode.Width;
        var windowHeight = _graphicsDevice.Adapter.CurrentDisplayMode.Height;

        InitializeGameStartButton(windowWidth, windowHeight);
    }

    private void InitializeMenuCamera()
    {
        _menuCamera = new MenuCamera(_graphicsDevice);
    }

    private void InitializeComponentsList()
    {
        _components = new List<MazeRunnerGameComponent>()
        {
            _startButtonInfo
        };
    }
}
