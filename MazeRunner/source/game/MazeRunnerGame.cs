using MazeRunner.Content;
using MazeRunner.Drawing;
using MazeRunner.GameBase.States;
using MazeRunner.Gui.Buttons;
using MazeRunner.Helpers;
using MazeRunner.Managers;
using MazeRunner.MazeBase.Tiles;
using MazeRunner.Sprites.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        var gameMenuMusic = SoundManager.Music.GameMenuMusic;
        var gameRunningMusic = SoundManager.Music.GameRunningMusic;

        gameMenuMusic.MusicPlayed +=
            async () => await gameMenuMusic.PlayAfterDelayAsync(GetRandomMusicPlayingPercentage(), GetRandomMusicPlayingPercentage());
        gameRunningMusic.MusicPlayed +=
            async () => await gameRunningMusic.PlayAfterDelayAsync(GetRandomMusicPlayingPercentage(), GetRandomMusicPlayingPercentage());

        GameMenuState.MenuEnteredNotify +=
            async () => await gameMenuMusic.StartPlayingMusicWithFadeAsync(GetRandomMusicPlayingPercentage());
        GameMenuState.MenuLeavedNotify +=
            () =>
            {
                gameMenuMusic.StopPlaying();
                gameMenuMusic.StopWaitingPlayDelay();
            };


        GameRunningState.GameStartedNotify +=
            async () => await gameRunningMusic.StartPlayingMusicWithFadeAsync(GetRandomMusicPlayingPercentage());

        GameRunningState.GameOveredNotify +=
            () =>
            {
                gameRunningMusic.StopPlaying();
                gameRunningMusic.StopWaitingPlayDelay();
                SoundManager.Transiters.PlayGameOveredSound();
            };
        GameRunningState.GameWonNotify +=
            () =>
            {
                gameRunningMusic.StopPlaying();
                gameRunningMusic.StopWaitingPlayDelay();
                SoundManager.Transiters.PlayGameWonSound();
            };

        GameOverState.GameMenuReturnedNotify += gameRunningMusic.StopPlaying;

        GamePausedState.GamePausedNotify += () => gameRunningMusic.ChangeMusicVolume(-50);
        GamePausedState.GameResumedNotify += () => gameRunningMusic.ChangeMusicVolume(100);
        GamePausedState.GameMenuReturnedNotify +=
            () =>
            {
                gameRunningMusic.StopPlaying();
                gameRunningMusic.StopWaitingPlayDelay();
            };

        Button.StaticButtonPressedNotify += SoundManager.Buttons.PlayButtonPressedSound;
        RadioButton.StaticButtonPressedNotify += SoundManager.Buttons.PlayRadioButtonPressedSound;

        Chalk.ItemCollectedStaticNotify += SoundManager.Notifiers.PlayChalkCollectedSound;
        Food.ItemCollectedStaticNotify += SoundManager.Notifiers.PlayFoodEatenSound;
        Key.ItemCollectedStaticNotify += SoundManager.Notifiers.PlayKeyCollectedSound;

        HeroBaseState.HeroDrewWithChalkNotify += SoundManager.Notifiers.PlayChalkDrawingSound;
        HeroRunState.HeroBeganRunningNotify += SoundManager.Sprites.Hero.PlayRunSound;
        HeroRunState.HeroFinishedRunningNotify += SoundManager.Sprites.Hero.PausePlayingRunSound;
        HeroDiedState.HeroDiedNotify += SoundManager.Sprites.Hero.StopPlayingRunSound;
        HeroFellState.HeroFellNotify += SoundManager.Sprites.Hero.StopPlayingRunSound;

        GuardAttackState.AttackMissedNotify += SoundManager.Sprites.Guard.PlayAttackMissedSound;
        GuardAttackState.AttackHitNotify +=
            async () =>
            {
                SoundManager.Sprites.Guard.PlayAttackHitSound();
                await Task.Delay(SoundManager.PauseDelayMs);
                SoundManager.Sprites.Hero.PlayGetHitSound();
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