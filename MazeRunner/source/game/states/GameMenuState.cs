using MazeRunner.Cameras;
using MazeRunner.Components;
using MazeRunner.Content;
using MazeRunner.Drawing;
using MazeRunner.Gui.Buttons;
using MazeRunner.Helpers;
using MazeRunner.MazeBase;
using MazeRunner.MazeBase.Tiles;
using MazeRunner.Wrappers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;

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
                MazeWidth = 25,
                MazeHeight = 25,

                MazeDeadEndsRemovePercentage = 55,

                MazeBayonetTrapInsertingPercentage = 2,
                MazeDropTrapInsertingPercentage = 1,

                HeroCameraScaleFactor = 7,
                HeroCameraShadowTresholdCoeff = 2.4f,

                GuardSpawnCount = 10,

                HeroHalfHeartsHealth = 5,
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

                HeroHalfHeartsHealth = 3,
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

                HeroHalfHeartsHealth = 2,
            });
        }
    }

    public event Action<IGameState> GameStateChanged;

    private static Texture2D _cameraEffect;

    private int _viewWidth;

    private int _viewHeight;

    private Lazy<GameParameters> _difficulty;

    private GraphicsDevice _graphicsDevice;

    private ButtonInfo _startButtonInfo;

    private ButtonInfo _quitButtonInfo;

    private MazeInfo _mazeInfo;

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
            var startButton = new StartButton(() => GameStateChanged.Invoke(new GameRunningState(_difficulty.Value)));

            var startButtonBoxScale = _viewWidth / scaleDivider;

            _startButtonInfo = new ButtonInfo(startButton, startButtonBoxScale);

            startButton.Initialize(_startButtonInfo);

            var startButtonPosition = new Vector2((_viewWidth - startButton.Width) / 2, (_viewHeight - startButton.Height) / 2);

            _startButtonInfo.Position = startButtonPosition;
        }

        void InitializeQuitGameButton(int scaleDivider)
        {
            var quitButton = new QuitButton(() => Environment.Exit(0));

            var quitButtonBoxScale = _viewWidth / scaleDivider;

            _quitButtonInfo = new ButtonInfo(quitButton, quitButtonBoxScale);

            quitButton.Initialize(_quitButtonInfo);

            var quitButtonPosition = new Vector2((_viewWidth - quitButton.Width) / 2, 5 * _viewHeight / 7 - quitButton.Height / 2);

            _quitButtonInfo.Position = quitButtonPosition;
        }

        void InitializeGameDifficultySelectRadioButtons(int scaleDivider, float buttonsOffsetCoeff)
        {
            var easySelectButtonBoxScale = _viewWidth / scaleDivider;

            var normalSelectButtonBoxScale = easySelectButtonBoxScale;
            var hardSelectButtonBoxScale = normalSelectButtonBoxScale;

            #region NormalModeSelectButton
            var normalSelectButton = new NormalModeSelectRadioButton(() => _difficulty = GameModes.Normal);

            var normalSelectButtonInfo = new ButtonInfo(normalSelectButton, normalSelectButtonBoxScale);

            normalSelectButton.Initialize(normalSelectButtonInfo);

            var startMenuButton = _startButtonInfo.Button;
            var startMenuButtonPosition = _startButtonInfo.Position;

            var normalSelectButtonPosition = new Vector2(
                (_viewWidth - normalSelectButton.Width) / 2,
                startMenuButtonPosition.Y + startMenuButton.Height * buttonsOffsetCoeff);

            normalSelectButtonInfo.Position = normalSelectButtonPosition;
            #endregion

            #region EasyModeSelectButton
            var easySelectButton = new EasyModeSelectRadioButton(() => _difficulty = GameModes.Easy);

            var easySelectButtonInfo = new ButtonInfo(easySelectButton, easySelectButtonBoxScale);

            easySelectButton.Initialize(easySelectButtonInfo);

            var easySelectButtonPosition = new Vector2(
                normalSelectButtonPosition.X - easySelectButton.Width * buttonsOffsetCoeff,
                normalSelectButtonPosition.Y);

            easySelectButtonInfo.Position = easySelectButtonPosition;
            #endregion

            #region HardModeSelectButton
            var hardSelectButton = new HardModeSelectRadioButton(() => _difficulty = GameModes.Hard);

            var hardSelectButtonInfo = new ButtonInfo(hardSelectButton, normalSelectButtonBoxScale);

            hardSelectButton.Initialize(hardSelectButtonInfo);

            var hardSelectButtonPosition = new Vector2(
                normalSelectButtonPosition.X + normalSelectButton.Width * buttonsOffsetCoeff,
                normalSelectButtonPosition.Y);

            hardSelectButtonInfo.Position = hardSelectButtonPosition;
            #endregion

            _difficultySelectButtonsContainer = new RadioButtonContainer(easySelectButtonInfo, normalSelectButtonInfo, hardSelectButtonInfo);

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

        var maze = MazeGenerator.GenerateMaze((int)Math.Ceiling(_viewWidth / frameSize) + 1, (int)Math.Ceiling(_viewHeight / frameSize) + 1);

        MazeGenerator.MakeCyclic(maze, deadEndsRemovePercentage);

        MazeGenerator.InsertTraps(maze, () => new BayonetTrap(), bayonetTrapInsertingPercentage);
        MazeGenerator.InsertTraps(maze, () => new DropTrap(), dropTrapInsertingPercentage);

        MazeGenerator.InsertExit(maze);

        MazeGenerator.InsertItem(maze, new Key());

        maze.InitializeComponentsList();

        _mazeInfo = new MazeInfo(maze);
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
            _startButtonInfo, _quitButtonInfo, _mazeInfo, _staticCamera, _difficultySelectButtonsContainer,
        };
    }
}