using MazeRunner.Cameras;
using MazeRunner.Components;
using MazeRunner.Drawing;
using MazeRunner.Gui.Buttons;
using MazeRunner.Helpers;
using MazeRunner.Managers;
using MazeRunner.MazeBase;
using MazeRunner.MazeBase.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace MazeRunner.GameBase.States;

public class GameMenuState : GameBaseState
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
                MazeWidth = 9,
                MazeHeight = 9,

                MazeDeadEndsRemovePercentage = 55,

                MazeBayonetTrapInsertingPercentage = 5, //2
                MazeDropTrapInsertingPercentage = 1.25f,

                GuardSpawnCount = 2,

                ChalksInsertingPercentage = 1,
                FoodInsertingPercentage = .75f,

                HeroHealth = 500, //5
                ChalkUses = 10,
            });

            Normal = new Lazy<GameParameters>(() => new GameParameters()
            {
                MazeWidth = 35,
                MazeHeight = 35,

                MazeDeadEndsRemovePercentage = 60,

                MazeBayonetTrapInsertingPercentage = 2.25f,
                MazeDropTrapInsertingPercentage = 1.75f,

                GuardSpawnCount = 15,

                ChalksInsertingPercentage = 1.25f,
                FoodInsertingPercentage = .75f,

                HeroHealth = 3,
                ChalkUses = 15,
            });

            Hard = new Lazy<GameParameters>(() => new GameParameters()
            {
                MazeWidth = 45,
                MazeHeight = 45,

                MazeDeadEndsRemovePercentage = 65,

                MazeBayonetTrapInsertingPercentage = 3.75f,
                MazeDropTrapInsertingPercentage = 2,

                GuardSpawnCount = 25,

                ChalksInsertingPercentage = 1.75f,
                FoodInsertingPercentage = 0.5f,

                HeroHealth = 2,
                ChalkUses = 25,
            });
        }
    }

    private static Texture2D _cameraEffect;

    public static event Action MenuEnteredNotify;

    public static event Action MenuLeavedNotify;

    private Lazy<GameParameters> _difficulty;

    private Button _startButton;

    private Button _quitButton;

    private Maze _maze;

    private StaticCamera _staticCamera;

    private RadioButtonContainer _difficultySelectButtonsContainer;

    private HashSet<MazeRunnerGameComponent> _components;

    public override event Action<IGameState> ControlGiveUpNotify;

    public GameMenuState()
    {
        MenuEnteredNotify.Invoke();
    }

    public override void Initialize(GraphicsDevice graphicsDevice, Game game)
    {
        base.Initialize(graphicsDevice, game);

        TurnOnMouseVisible(game);

        _difficulty = GameModes.Normal;

        InitializeButtons();
        InitializeCamera();
        InitializeMaze();
        InitializeShadower();
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

    private void InitializeButtons()
    {
        void InitializeGameStartButton(float scaleDivider)
        {
            var boxScale = ViewWidth / scaleDivider;

            _startButton = new StartButton(boxScale);

            _startButton.Initialize();

            _startButton.Position = new Vector2((ViewWidth - _startButton.Width) / 2, (ViewHeight - _startButton.Height) / 2);

            _startButton.ButtonPressed += SoundManager.PlayButtonPressedSound;
            _startButton.ButtonPressed += StartGame;
        }

        void InitializeQuitGameButton(float scaleDivider)
        {
            var boxScale = ViewWidth / scaleDivider;

            _quitButton = new QuitButton(boxScale);

            _quitButton.Initialize();

            _quitButton.Position = new Vector2((ViewWidth - _quitButton.Width) / 2, 5 * ViewHeight / 7 - _quitButton.Height / 2);

            _quitButton.ButtonPressed += SoundManager.PlayButtonPressedSound;
            _quitButton.ButtonPressed += QuitGame;
        }

        void InitializeGameDifficultySelectRadioButtons(float scaleDivider, float buttonsOffsetCoeff)
        {
            var boxScale = ViewWidth / scaleDivider;

            var normalSelectButton = new NormalModeSelectRadioButton(boxScale);

            normalSelectButton.Initialize();

            var normalSelectButtonPosition = new Vector2(
                (ViewWidth - normalSelectButton.Width) / 2,
                _startButton.Position.Y + _startButton.Height * buttonsOffsetCoeff);

            normalSelectButton.Position = normalSelectButtonPosition;
            normalSelectButton.ButtonPressed += () => _difficulty = GameModes.Normal;

            var easySelectButton = new EasyModeSelectRadioButton(boxScale);

            easySelectButton.Initialize();

            easySelectButton.Position = new Vector2(
                normalSelectButtonPosition.X - easySelectButton.Width * buttonsOffsetCoeff,
                normalSelectButtonPosition.Y);

            easySelectButton.ButtonPressed += SoundManager.PlayRadioButtonPressedSound;
            easySelectButton.ButtonPressed += () => _difficulty = GameModes.Easy;

            var hardSelectButton = new HardModeSelectRadioButton(boxScale);

            hardSelectButton.Initialize();

            hardSelectButton.Position = new Vector2(
                normalSelectButtonPosition.X + normalSelectButton.Width * buttonsOffsetCoeff,
                normalSelectButtonPosition.Y);

            hardSelectButton.ButtonPressed += SoundManager.PlayRadioButtonPressedSound;
            hardSelectButton.ButtonPressed += () => _difficulty = GameModes.Hard;

            _difficultySelectButtonsContainer = new RadioButtonContainer(easySelectButton, normalSelectButton, hardSelectButton);

            normalSelectButton.Push();

            normalSelectButton.ButtonPressed += SoundManager.PlayRadioButtonPressedSound;
        }

        var startButtonScaleDivider = 360;

        var quitButtonScaleDivider = 460;

        var difficultyButtonsOffsetCoeff = 1.25f;
        var dificultyButtonsScaleDivider = 560;

        InitializeGameStartButton(startButtonScaleDivider);
        InitializeQuitGameButton(quitButtonScaleDivider);
        InitializeGameDifficultySelectRadioButtons(dificultyButtonsScaleDivider, difficultyButtonsOffsetCoeff);
    }

    private void InitializeMaze()
    {
        var bayonetTrapInsertingPercentage = 2;
        var dropTrapInsertingPercentage = 2;

        var deadEndsRemovePercentage = 75;

        var frameSize = (double)GameConstants.AssetsFrameSize;

        _maze = MazeGenerator.GenerateMaze((int)Math.Ceiling(ViewWidth / frameSize) + 1, (int)Math.Ceiling(ViewHeight / frameSize) + 1);

        MazeGenerator.MakeCyclic(_maze, deadEndsRemovePercentage);

        MazeGenerator.InsertTraps(_maze, () => new BayonetTrap(), bayonetTrapInsertingPercentage);
        MazeGenerator.InsertTraps(_maze, () => new DropTrap(), dropTrapInsertingPercentage);

        MazeGenerator.InsertExit(_maze);

        MazeGenerator.InsertItem(_maze, new Key(), null);

        _maze.InitializeComponents();
    }

    private void InitializeCamera()
    {
        void InitializeCameraEffect()
        {
            var shadowTreshold = ViewHeight / 2.1f;

            _cameraEffect = EffectsHelper.CreateGradientCircleEffect(ViewWidth, ViewHeight, shadowTreshold, GraphicsDevice);
        }

        if (_cameraEffect is null)
        {
            InitializeCameraEffect();
        }

        _staticCamera = new StaticCamera(ViewWidth, ViewHeight)
        {
            Effect = _cameraEffect,
        };
    }

    private void InitializeComponents()
    {
        _components = new HashSet<MazeRunnerGameComponent>
        {
            _startButton, _quitButton, _maze, _staticCamera, _difficultySelectButtonsContainer, Shadower,
        };
    }

    private void StartGame()
    {
        NeedShadowerActivate = true;

        Shadower = new EffectsHelper.Shadower(false);

        Shadower.TresholdReached += () =>
        {
            MenuLeavedNotify.Invoke();
            ControlGiveUpNotify.Invoke(new GameRunningState(_difficulty.Value));
        };
    }

    private void InitializeShadower()
    {
        Shadower = new EffectsHelper.Shadower(true);

        Shadower.TresholdReached += () => NeedShadowerDeactivate = true;
    }

    private void QuitGame()
    {
        Environment.Exit(0);
    }
}