using MazeRunner.Cameras;
using MazeRunner.Components;
using MazeRunner.Content;
using MazeRunner.Drawing;
using MazeRunner.Gui.Buttons;
using MazeRunner.Gui.Buttons.States;
using MazeRunner.MazeBase;
using MazeRunner.MazeBase.Tiles;
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

    private int _windowWidth;

    private int _windowHeight;

    private GraphicsDevice _graphicsDevice;

    private ButtonInfo _startButtonInfo;

    private MazeInfo _mazeInfo;

    private MenuCamera _menuCamera;

    private List<MazeRunnerGameComponent> _components;

    public void Initialize(GraphicsDevice graphicsDevice)
    {
        _graphicsDevice = graphicsDevice;

        _windowWidth = _graphicsDevice.Adapter.CurrentDisplayMode.Width;
        _windowHeight = _graphicsDevice.Adapter.CurrentDisplayMode.Height;

        InitializeMenuButtons();
        InitializeMenuCamera();
        InitializeBackgroundMaze();
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

    public void Update(GameTime gameTime)
    {
        foreach (var component in _components)
        {
            component.Update(gameTime);
        }
    }

    private void InitializeMenuButtons()
    {
        void InitializeGameStartButton()
        {
            var startButton = new Button(() => GameStateChanged.Invoke(new GameRunningState(GameModes.Easy)));

            var startButtonBoxScale = _windowWidth / 360;

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

            var startButtonPosition = new Vector2((_windowWidth - startButton.Width) / 2, (_windowHeight - startButton.Height) / 2);
            _startButtonInfo.Position = startButtonPosition;
        }

        InitializeGameStartButton();
    }

    private void InitializeBackgroundMaze()
    {
        var bayonetTrapInsertingPercentage = 2;
        var dropTrapInsertingPercentage = 2;

        var deadEndsRemovePercentage = 75;

        var frameSize = (double)Textures.MazeTiles.Floor_1.Width;

        var maze = MazeGenerator.GenerateMaze((int)Math.Ceiling(_windowWidth / frameSize) + 1, (int)Math.Ceiling(_windowHeight / frameSize) + 1);

        MazeGenerator.MakeCyclic(maze, deadEndsRemovePercentage);

        MazeGenerator.InsertTraps(maze, () => new BayonetTrap(), bayonetTrapInsertingPercentage);
        MazeGenerator.InsertTraps(maze, () => new DropTrap(), dropTrapInsertingPercentage);

        MazeGenerator.InsertExit(maze);

        MazeGenerator.InsertItem(maze, new Key());

        maze.InitializeComponentsList();

        _mazeInfo = new MazeInfo(maze);
    }

    private void InitializeMenuCamera()
    {
        _menuCamera = new MenuCamera(_graphicsDevice);
    }

    private void InitializeComponentsList()
    {
        _components = new List<MazeRunnerGameComponent>()
        {
            _startButtonInfo, _mazeInfo, _menuCamera,
        };
    }
}