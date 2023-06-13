using MazeRunner.Cameras;
using MazeRunner.Components;
using MazeRunner.Content;
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
using System.Threading.Tasks;

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
                FoodInsertingPercentage = 25, // .75

                HeroHealth = 500, //5
                ChalkUses = 10,
            });

            Normal = new Lazy<GameParameters>(() => new GameParameters()
            {
                MazeWidth = 35,
                MazeHeight = 35,

                MazeDeadEndsRemovePercentage = 60,

                MazeBayonetTrapInsertingPercentage = 2.25f,
                MazeDropTrapInsertingPercentage = 45f, //1.75

                GuardSpawnCount = 35, //15

                ChalksInsertingPercentage = 1.25f,
                FoodInsertingPercentage = .75f,

                HeroHealth = 1, //3
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

    private const float GameMenuMusicMaxVolume = .3f;

    private static readonly SoundManager.Music.MusicPlayer _gameMenuMusic;

    private static Texture2D _cameraEffect;

    private Lazy<GameParameters> _difficulty;

    private Button _startButton;

    private Button _quitButton;

    private Maze _maze;

    private StaticCamera _staticCamera;

    private RadioButtonContainer _difficultySelectButtonsContainer;

    private HashSet<MazeRunnerGameComponent> _components;

    public override event Action<IGameState> ControlGiveUpNotify;

    static GameMenuState()
    {
        _gameMenuMusic = new SoundManager.Music.MusicPlayer(Sounds.Music.GameMenu, GameMenuMusicMaxVolume);

        _gameMenuMusic.MusicPlayed +=
            async () => await _gameMenuMusic.PlayAfterDelay(
                RandomHelper.GetRandomMusicPlayingPercentage(), RandomHelper.GetRandomMusicPlayingPercentage());
    }

    public GameMenuState()
    {
        Task.Factory.StartNew(async () => await _gameMenuMusic.StartPlayingMusicWithFade(RandomHelper.GetRandomMusicPlayingPercentage()));
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

    private static void StopPlayingMusic()
    {
        _gameMenuMusic.StopPlaying();
        _gameMenuMusic.StopPlaying();
    }

    private void InitializeButtons()
    {
        void InitializeGameStartButton(float scaleDivider)
        {
            var boxScale = ViewWidth / scaleDivider;

            _startButton = new StartButton(boxScale);

            _startButton.Initialize();

            _startButton.Position = new Vector2((ViewWidth - _startButton.Width) / 2, (ViewHeight - _startButton.Height) / 2);

            _startButton.ButtonPressedNotify += StartGame;
        }

        void InitializeQuitGameButton(float scaleDivider)
        {
            var boxScale = ViewWidth / scaleDivider;

            _quitButton = new QuitButton(boxScale);

            _quitButton.Initialize();

            _quitButton.Position = new Vector2((ViewWidth - _quitButton.Width) / 2, 5 * ViewHeight / 7 - _quitButton.Height / 2);

            _quitButton.ButtonPressedNotify += QuitGame;
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
            normalSelectButton.ButtonPressedNotify += () => _difficulty = GameModes.Normal;

            var easySelectButton = new EasyModeSelectRadioButton(boxScale);

            easySelectButton.Initialize();

            easySelectButton.Position = new Vector2(
                normalSelectButtonPosition.X - easySelectButton.Width * buttonsOffsetCoeff,
                normalSelectButtonPosition.Y);

            easySelectButton.ButtonPressedNotify += () => _difficulty = GameModes.Easy;

            var hardSelectButton = new HardModeSelectRadioButton(boxScale);

            hardSelectButton.Initialize();

            hardSelectButton.Position = new Vector2(
                normalSelectButtonPosition.X + normalSelectButton.Width * buttonsOffsetCoeff,
                normalSelectButtonPosition.Y);

            hardSelectButton.ButtonPressedNotify += () => _difficulty = GameModes.Hard;

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

    private void InitializeMaze()
    {
        var bayonetTrapInsertingPercentage = 2;
        var dropTrapInsertingPercentage = 2;

        var deadEndsRemovePercentage = 75;

        var frameSize = (double)GameConstants.AssetsFrameSize;

        _maze = MazeGenerator.GenerateMaze((int)Math.Ceiling(ViewWidth / frameSize) + 1, (int)Math.Ceiling(ViewHeight / frameSize) + 1);

        MazeGenerator.MakeCyclic(_maze, deadEndsRemovePercentage);

        MazeGenerator.InsertTraps(_maze, () => new BayonetTrap(null), bayonetTrapInsertingPercentage);
        MazeGenerator.InsertTraps(_maze, () => new DropTrap(null), dropTrapInsertingPercentage);

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

    private void InitializeShadower()
    {
        Shadower = new EffectsHelper.Shadower(true);

        Shadower.TresholdReached += () => NeedShadowerDeactivate = true;
    }

    private void InitializeComponents()
    {
        _components = new HashSet<MazeRunnerGameComponent>
        {
            _maze, _staticCamera, Shadower, _difficultySelectButtonsContainer, _quitButton, _startButton,
        };
    }

    private void StartGame()
    {
        NeedShadowerActivate = true;

        Shadower = new EffectsHelper.Shadower(false);

        Shadower.TresholdReached += () =>
        {
            StopPlayingMusic();
            ControlGiveUpNotify.Invoke(new GameRunningState(_difficulty.Value));
        };
    }

    private void QuitGame()
    {
        Environment.Exit(0);
    }
}