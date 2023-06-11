using MazeRunner.Content;
using MazeRunner.Drawing;
using MazeRunner.GameBase.States;
using MazeRunner.Gui.Buttons;
using MazeRunner.Helpers;
using MazeRunner.Managers;
using MazeRunner.Sprites.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MazeRunner.GameBase;

public class MazeRunnerGame : Game
{
    private const int MinMusicPlayingPercentage = 30;

    private const int MaxMusicPlayingPercentage = 100;

    private readonly GraphicsDeviceManager _graphics;

    private IGameState _gameState;

    public MazeRunnerGame()
    {
        IsFixedTimeStep = true;
        IsMouseVisible = true;

        Content.RootDirectory = "Content";

        _graphics = new GraphicsDeviceManager(this);
    }

    protected override void Initialize()
    {
        void InitializeShadower()
        {
            var viewport = GraphicsDevice.Viewport;

            var width = viewport.Width;
            var height = viewport.Height;

            EffectsHelper.Shadower.InitializeBlackBackground(width, height, GraphicsDevice);
        }

        void InitializeDrawer()
        {
            Drawer.Initialize(this);
        }

        base.Initialize();

        //SetFullScreen();
        InitializeDrawer();
        ApplySounds();
        InitializeShadower();

        _gameState = new GameMenuState();

        _gameState.Initialize(GraphicsDevice, this);

        _gameState.ControlGiveUpNotify += ControlGiveUpHandler;
    }

    protected override void LoadContent()
    {
        Textures.Load(this);
        Fonts.Load(this);
        Sounds.Load(this);
    }

    protected override void Update(GameTime gameTime)
    {
        _gameState.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        _gameState.Draw(gameTime);

        base.Draw(gameTime);
    }

    private static void ApplySounds()
    {
        static async Task PlayAfterDelayAsync(double musicDurationMs, float percentageDelay, Action playAction, CancellationToken cancellationToken)
        {
            var delay = musicDurationMs * percentageDelay / 100;

            await Task.Delay((int)delay, cancellationToken).ContinueWith(task => task.Exception == default);

            if (!cancellationToken.IsCancellationRequested)
            {
                playAction.Invoke();
            }
        }

        var gameMenuMusicBreaker = new SoundManager.MusicBreaker();
        var gameRunningMusicBreaker = new SoundManager.MusicBreaker();

        var gameMenuPlayAfterDelayBreaker = new SoundManager.MusicBreaker();
        var gameRunningPlayAfterDelayBreaker = new SoundManager.MusicBreaker();

        SoundManager.GameMenuMusicEndPlaying +=
            async () => 
            await PlayAfterDelayAsync(
                SoundManager.GameMenuMusicDurationMs, 
                GetRandomMusicPlayingPercentage(), 
                async () => await SoundManager.PlayGameMenuMusicAsync(GetRandomMusicPlayingPercentage(), gameMenuMusicBreaker.CancellationToken), 
                gameMenuPlayAfterDelayBreaker.CancellationToken);

        SoundManager.GameRunningMusicEndPlaying +=
            async () =>
            await PlayAfterDelayAsync(
                SoundManager.GameRunningMusicDurationMs,
                GetRandomMusicPlayingPercentage(),
                async () => await SoundManager.PlayGameRunningMusicAsync(GetRandomMusicPlayingPercentage(), gameRunningMusicBreaker.CancellationToken),
                gameRunningPlayAfterDelayBreaker.CancellationToken);

        GameMenuState.MenuEnteredNotify +=
            async () => await SoundManager.PlayGameMenuMusicAsync(GetRandomMusicPlayingPercentage(), gameMenuMusicBreaker.CancellationToken);
        GameMenuState.MenuLeavedNotify +=
            () =>
            {
                gameMenuMusicBreaker.StopMusic();
                gameMenuPlayAfterDelayBreaker.StopMusic();
            };
            

        GameRunningState.GameStartedNotify +=
            async () => await SoundManager.PlayGameRunningMusicAsync(GetRandomMusicPlayingPercentage(), gameRunningMusicBreaker.CancellationToken);

        GameRunningState.GameOveredNotify += 
            () =>
            {
                gameRunningMusicBreaker.StopMusic();
                gameRunningPlayAfterDelayBreaker.StopMusic();
                SoundManager.PlayGameOveredSound();
            };
        GameRunningState.GameWonNotify +=
            () =>
            {
                gameRunningMusicBreaker.StopMusic();
                gameRunningPlayAfterDelayBreaker.StopMusic();
                SoundManager.PlayGameWonSound();
            };

        GameOverState.GameMenuReturnedNotify += gameRunningMusicBreaker.StopMusic;

        GamePausedState.GamePausedNotify += () => SoundManager.ChangeGameRunningMusicVolume(-50);
        GamePausedState.GameResumedNotify += () => SoundManager.ChangeGameRunningMusicVolume(100);
        GamePausedState.GameMenuReturnedNotify += gameRunningMusicBreaker.StopMusic;

        Button.StaticButtonPressedNotify += SoundManager.PlayButtonPressedSound;
        RadioButton.StaticButtonPressedNotify += SoundManager.PlayRadioButtonPressedSound;

        HeroBaseState.HeroDrewWithChalkNotify += SoundManager.PlayChalkDrawingSound;
        HeroRunState.HeroBeganRunningNotify += SoundManager.PlayHeroRunSound;
        HeroRunState.HeroFinishedRunningNotify += SoundManager.PausePlayingHeroRunSound;
        HeroDiedState.HeroDiedNotify += SoundManager.StopPlayingHeroRunSound;
        HeroFellState.HeroFellNotify += SoundManager.StopPlayingHeroRunSound;

        GuardAttackState.AttackMissedNotify += SoundManager.PlayGuardAttackMissedSound;
        GuardAttackState.AttackHitNotify +=
            async () =>
            {
                SoundManager.PlayGuardAttackHitSound();
                await Task.Delay(SoundManager.PauseDelayMs);
                SoundManager.PlayHeroGetHitSound();
            };
    }

    private static int GetRandomMusicPlayingPercentage()
    {
        return RandomHelper.Next(MinMusicPlayingPercentage, MaxMusicPlayingPercentage + 1);
    }

    private void SetFullScreen()
    {
        _graphics.IsFullScreen = true;

        _graphics.PreferredBackBufferWidth = GraphicsDevice.Adapter.CurrentDisplayMode.Width;
        _graphics.PreferredBackBufferHeight = GraphicsDevice.Adapter.CurrentDisplayMode.Height;

        _graphics.ApplyChanges();
    }

    private void ControlGiveUpHandler(IGameState gameState)
    {
        _gameState = gameState;

        _gameState.Initialize(GraphicsDevice, this);

        _gameState.ControlGiveUpNotify += ControlGiveUpHandler;
    }
}