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
    private enum DifficultyMode
    {
        Easy,
        Normal,
        Hard,
    }

    private static class GameModes
    {
        public static readonly GameParameters Easy;

        public static readonly GameParameters Normal;

        public static readonly GameParameters Hard;

        static GameModes()
        {
            Easy = new GameParameters
            {
                MazeWidth = 9,
                MazeHeight = 9,

                MazeDeadEndsRemovePercentage = 55,

                MazeBayonetTrapInsertingPercentage = 2, 
                MazeDropTrapInsertingPercentage = 1.25f,

                GuardSpawnCount = 7,

                ChalksInsertingPercentage = 1,
                FoodInsertingPercentage = .75f,

                HeroHealth = 5,
                ChalkUses = 10,
            };

            Normal = new GameParameters
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
            };

            Hard = new GameParameters
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
            };
        }
    }

    private const float GameMenuMusicMaxVolume = .3f;

    private const int ButtonsCantBeClickedEnterDelayMs = 750;

    private static readonly Dictionary<DifficultyMode, GameParameters> _difficultyParameters;

    private static readonly SoundManager.Music.MusicPlayer _gameMenuMusic;

    private static readonly DifficultyMode _defaultDifficulty;

    private static DifficultyMode? _lastSelectedDifficultyMode;

    private static Texture2D _cameraEffect;

    private Button _startButton;

    private Button _quitButton;

    private Maze _maze;

    private StaticCamera _staticCamera;

    private RadioButtonContainer _difficultySelectButtonsContainer;

    private HashSet<MazeRunnerGameComponent> _components;

    private bool _canButtonsBeClicked;

    public override event Action<IGameState> ControlGiveUpNotify;

    static GameMenuState()
    {
        _gameMenuMusic = new SoundManager.Music.MusicPlayer(Sounds.Music.GameMenu, GameMenuMusicMaxVolume);

        _gameMenuMusic.MusicPlayed +=
            async () => await _gameMenuMusic.PlayAfterDelay(
                RandomHelper.GetRandomMusicPlayingPercentage(), RandomHelper.GetRandomMusicPlayingPercentage());

        _difficultyParameters = GetDifficultyParameters();

        _defaultDifficulty = DifficultyMode.Normal;
    }

    public GameMenuState()
    {
        Task.Factory.StartNew(async () => await _gameMenuMusic.StartPlayingMusicWithFade(RandomHelper.GetRandomMusicPlayingPercentage()));
    }

    public override void Initialize(GraphicsDevice graphicsDevice, Game game)
    {
        base.Initialize(graphicsDevice, game);

        TurnOnMouseVisible(game);

        TemporaryBlockButtonsClicks();
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

    private static Dictionary<DifficultyMode, GameParameters> GetDifficultyParameters()
    {
        return new Dictionary<DifficultyMode, GameParameters>
        {
            { DifficultyMode.Easy, GameModes.Easy },
            { DifficultyMode.Normal, GameModes.Normal },
            { DifficultyMode.Hard, GameModes.Hard },
        };
    }

    private static GameParameters GetDifficultyGameParameters()
    {
        _lastSelectedDifficultyMode ??= _defaultDifficulty;

        return _difficultyParameters[_lastSelectedDifficultyMode.Value];
    }

    private static void PushDifficultySelectButtonWithDifficultyMode(
        RadioButton easySelectButton, RadioButton normalSelectButton, RadioButton hardSelectButton)
    {
        switch (_lastSelectedDifficultyMode)
        {
            case DifficultyMode.Easy:
                easySelectButton.Push();
                break;
            case DifficultyMode.Normal:
                normalSelectButton.Push();
                break;
            case DifficultyMode.Hard:
                hardSelectButton.Push();
                break;
            default:
                throw new NotImplementedException();
        }
    }

    private void InitializeButtons()
    {
        void InitializeGameStartButton(float scaleDivider)
        {
            var boxScale = ViewWidth / scaleDivider;

            _startButton = new StartButton(boxScale, () => _canButtonsBeClicked);

            _startButton.Initialize();

            _startButton.Position = new Vector2((ViewWidth - _startButton.Width) / 2, (ViewHeight - _startButton.Height) / 2);

            _startButton.ButtonPressedNotify += StartGame;
        }

        void InitializeQuitGameButton(float scaleDivider)
        {
            var boxScale = ViewWidth / scaleDivider;

            _quitButton = new QuitButton(boxScale, () => _canButtonsBeClicked);

            _quitButton.Initialize();

            _quitButton.Position = new Vector2((ViewWidth - _quitButton.Width) / 2, 5 * ViewHeight / 7 - _quitButton.Height / 2);

            _quitButton.ButtonPressedNotify += QuitGame;
        }

        void InitializeGameDifficultySelectRadioButtons(float scaleDivider, float buttonsOffsetCoeff)
        {
            var boxScale = ViewWidth / scaleDivider;

            var normalSelectButton = new NormalModeSelectRadioButton(boxScale, () => _canButtonsBeClicked);

            normalSelectButton.Initialize();

            var normalSelectButtonPosition = new Vector2(
                (ViewWidth - normalSelectButton.Width) / 2,
                _startButton.Position.Y + _startButton.Height * buttonsOffsetCoeff);

            normalSelectButton.Position = normalSelectButtonPosition;

            normalSelectButton.ButtonPressedNotify += () => _lastSelectedDifficultyMode = DifficultyMode.Normal;

            var easySelectButton = new EasyModeSelectRadioButton(boxScale, () => _canButtonsBeClicked);

            easySelectButton.Initialize();

            easySelectButton.Position = new Vector2(
                normalSelectButtonPosition.X - easySelectButton.Width * buttonsOffsetCoeff,
                normalSelectButtonPosition.Y);

            easySelectButton.ButtonPressedNotify += () => _lastSelectedDifficultyMode = DifficultyMode.Easy;

            var hardSelectButton = new HardModeSelectRadioButton(boxScale, () => _canButtonsBeClicked);

            hardSelectButton.Initialize();

            hardSelectButton.Position = new Vector2(
                normalSelectButtonPosition.X + normalSelectButton.Width * buttonsOffsetCoeff,
                normalSelectButtonPosition.Y);

            hardSelectButton.ButtonPressedNotify += () => _lastSelectedDifficultyMode = DifficultyMode.Hard;

            _difficultySelectButtonsContainer = new RadioButtonContainer(easySelectButton, normalSelectButton, hardSelectButton);

            _lastSelectedDifficultyMode ??= DifficultyMode.Normal;

            PushDifficultySelectButtonWithDifficultyMode(easySelectButton, normalSelectButton, hardSelectButton);
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
        _canButtonsBeClicked = false;

        NeedShadowerActivate = true;

        Shadower = new EffectsHelper.Shadower(false);

        Shadower.TresholdReached += () =>
        {
            StopPlayingMusic();
            ControlGiveUpNotify.Invoke(new GameRunningState(GetDifficultyGameParameters()));
        };
    }

    private void QuitGame()
    {
        _canButtonsBeClicked = false;

        Environment.Exit(0);
    }

    private void TemporaryBlockButtonsClicks()
    {
        Task.Run(async () =>
        {
            await Task.Delay(ButtonsCantBeClickedEnterDelayMs);

            _canButtonsBeClicked = true;
        });
    }
}