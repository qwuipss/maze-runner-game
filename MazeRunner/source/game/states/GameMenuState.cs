using MazeRunner.Cameras;
using MazeRunner.Components;
using MazeRunner.Content;
using MazeRunner.Drawing;
using MazeRunner.Gui.Buttons;
using MazeRunner.Helpers;
using MazeRunner.MazeBase;
using MazeRunner.MazeBase.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace MazeRunner.GameBase.States;

public class GameMenuState : IGameState
{
    private static class GameModes
    {
        public static readonly Lazy<GameParameters> Easy;
        public static readonly Lazy<GameParameters> Normal;
        public static readonly Lazy<GameParameters> Hard;

        static GameModes()
        {
            Easy = new Lazy<GameParameters>(() => new GameParameters()
            {
                MazeWidth = 5,//25
                MazeHeight = 5,

                MazeDeadEndsRemovePercentage = 55,

                MazeBayonetTrapInsertingPercentage = 2,
                MazeDropTrapInsertingPercentage = 1,

                HeroCameraScaleFactor = 7,
                HeroCameraShadowTresholdCoeff = 2.4f,

                GuardSpawnCount = 0,//10

                HeroHealth = 5,
            });

            Normal = new Lazy<GameParameters>(() => new GameParameters()
            {
                MazeWidth = 35,
                MazeHeight = 35,

                MazeDeadEndsRemovePercentage = 60,

                MazeBayonetTrapInsertingPercentage = 3,
                MazeDropTrapInsertingPercentage = 2,

                HeroCameraScaleFactor = 7,
                HeroCameraShadowTresholdCoeff = 2.4f,

                GuardSpawnCount = 15,

                HeroHealth = 3,
            });

            Hard = new Lazy<GameParameters>(() => new GameParameters()
            {
                MazeWidth = 45,
                MazeHeight = 45,

                MazeDeadEndsRemovePercentage = 70,

                MazeBayonetTrapInsertingPercentage = 4,
                MazeDropTrapInsertingPercentage = 2,

                HeroCameraScaleFactor = 7,
                HeroCameraShadowTresholdCoeff = 2.4f,

                GuardSpawnCount = 25,

                HeroHealth = 2,
            });
        }
    }

    public event Action<IGameState> GameStateChanged;

    private static Texture2D _cameraEffect;

    private int _viewWidth;

    private int _viewHeight;

    private Lazy<GameParameters> _difficulty;

    private GraphicsDevice _graphicsDevice;

    private Button _startButton;

    private Button _quitButton;

    private Maze _maze;

    private StaticCamera _staticCamera;

    private RadioButtonContainer _difficultySelectButtonsContainer;

    private List<MazeRunnerGameComponent> _components;

    public void Initialize(GraphicsDevice graphicsDevice, Game game)
    {
        _graphicsDevice = graphicsDevice;

        if (!game.IsMouseVisible)
        {
            game.IsMouseVisible = true;
        }

        _difficulty = GameModes.Normal;

        var viewPort = _graphicsDevice.Viewport;

        _viewWidth = viewPort.Width;
        _viewHeight = viewPort.Height;

        InitializeButtons();
        InitializeStaticCamera();
        InitializeBackgroundMaze();
        InitializeComponentsList();
    }

    public void Draw(GameTime gameTime)
    {
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
    }

    private void InitializeButtons()
    {
        void InitializeGameStartButton(int scaleDivider)
        {
            var boxScale = _viewWidth / scaleDivider;

            _startButton = new StartButton(() => GameStateChanged.Invoke(new GameRunningState(_difficulty.Value)), boxScale);

            _startButton.Initialize();

            _startButton.Position = new Vector2((_viewWidth - _startButton.Width) / 2, (_viewHeight - _startButton.Height) / 2);
        }

        void InitializeQuitGameButton(int scaleDivider)
        {
            var boxScale = _viewWidth / scaleDivider;

            _quitButton = new QuitButton(() => Environment.Exit(0), boxScale);

            _quitButton.Initialize();

            _quitButton.Position = new Vector2((_viewWidth - _quitButton.Width) / 2, 5 * _viewHeight / 7 - _quitButton.Height / 2);
        }

        void InitializeGameDifficultySelectRadioButtons(int scaleDivider, float buttonsOffsetCoeff)
        {
            var boxScale = _viewWidth / scaleDivider;

            var normalSelectButton = new NormalModeSelectRadioButton(() => _difficulty = GameModes.Normal, boxScale);

            normalSelectButton.Initialize();

            var normalSelectButtonPosition = new Vector2(
                (_viewWidth - normalSelectButton.Width) / 2,
                _startButton.Position.Y + _startButton.Height * buttonsOffsetCoeff);

            normalSelectButton.Position = normalSelectButtonPosition;

            var easySelectButton = new EasyModeSelectRadioButton(() => _difficulty = GameModes.Easy, boxScale);

            easySelectButton.Initialize();

            easySelectButton.Position = new Vector2(
                normalSelectButtonPosition.X - easySelectButton.Width * buttonsOffsetCoeff,
                normalSelectButtonPosition.Y);

            var hardSelectButton = new HardModeSelectRadioButton(() => _difficulty = GameModes.Hard, boxScale);

            hardSelectButton.Initialize();

            hardSelectButton.Position = new Vector2(
                normalSelectButtonPosition.X + normalSelectButton.Width * buttonsOffsetCoeff,
                normalSelectButtonPosition.Y);

            _difficultySelectButtonsContainer = new RadioButtonContainer(easySelectButton, normalSelectButton, hardSelectButton);

            normalSelectButton.Push();
        }

        var startButtonScaleDivider = 360;

        var quitButtonScaleDivider = 460;

        var difficultyButtonsOffsetCoeff = 1.25f;
        var dificultyButtonsScaleDivider = 560;

        InitializeGameStartButton(startButtonScaleDivider);
        InitializeQuitGameButton(quitButtonScaleDivider);
        InitializeGameDifficultySelectRadioButtons(dificultyButtonsScaleDivider, difficultyButtonsOffsetCoeff);
    }

    private void InitializeBackgroundMaze()
    {
        var bayonetTrapInsertingPercentage = 2;
        var dropTrapInsertingPercentage = 2;

        var deadEndsRemovePercentage = 75;

        var frameSize = (double)Textures.MazeTiles.Floor_1.Width;

        _maze = MazeGenerator.GenerateMaze((int)Math.Ceiling(_viewWidth / frameSize) + 1, (int)Math.Ceiling(_viewHeight / frameSize) + 1);

        MazeGenerator.MakeCyclic(_maze, deadEndsRemovePercentage);

        MazeGenerator.InsertTraps(_maze, () => new BayonetTrap(), bayonetTrapInsertingPercentage);
        MazeGenerator.InsertTraps(_maze, () => new DropTrap(), dropTrapInsertingPercentage);

        MazeGenerator.InsertExit(_maze);

        MazeGenerator.InsertItem(_maze, new Key());

        _maze.InitializeComponentsList();
    }

    private void InitializeStaticCamera()
    {
        void InitializeCameraEffect()
        {
            var shadowTreshold = _viewHeight / 2.1f;

            _cameraEffect = EffectsHelper.CreateGradientCircleEffect(_viewWidth, _viewHeight, shadowTreshold, _graphicsDevice);
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
            _startButton, _quitButton, _maze, _staticCamera, _difficultySelectButtonsContainer,
        };
    }
}