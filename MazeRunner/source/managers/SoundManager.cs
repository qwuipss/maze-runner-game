using MazeRunner.Content;
using Microsoft.Xna.Framework.Audio;
using System.Threading;
using System.Threading.Tasks;

namespace MazeRunner.Managers;

public static class SoundManager
{
    public class MusicBreaker
    {
        private CancellationTokenSource _cancellationTokenSource;

        public CancellationToken CancellationToken => _cancellationTokenSource.Token;

        public MusicBreaker()
        {
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public void StopMusic()
        {
            _cancellationTokenSource.Cancel();

            _cancellationTokenSource = new CancellationTokenSource();
        }
    }

    private const int FadeDelayMs = 250;

    private const float FadeValue = .05f;

    public const int PauseDelayMs = 125;

    private static readonly SoundEffectInstance _buttonPressed;

    private static readonly SoundEffectInstance _radioButtonPressed;

    private static readonly SoundEffectInstance _keyCollected;

    private static readonly SoundEffectInstance _foodEaten;

    private static readonly SoundEffectInstance _chalkDrawing;

    private static readonly SoundEffectInstance _chalkCollecting;

    private static readonly SoundEffectInstance _heroRun;

    private static readonly SoundEffectInstance _heroGetHit;

    private static readonly SoundEffectInstance _guardAttackMissed;

    private static readonly SoundEffectInstance _guardAttackHit;

    private static readonly SoundEffectInstance _gameWon;

    private static readonly SoundEffectInstance _gameOvered;

    private static readonly SoundEffectInstance _gameMenuMusic;

    private static readonly SoundEffectInstance _gameRunningMusic;

    private static readonly double _gameMenuMusicDurationMs;

    private static readonly float _gameMenuMusicMaxVolume;

    private static readonly double _gameRunningMusicDurationMs;

    private static readonly float _gameRunningMusicMaxVolume;

    static SoundManager()
    {
        _buttonPressed = Sounds.Buttons.Button.CreateInstance();
        _buttonPressed.Volume = .3f;

        _radioButtonPressed = Sounds.Buttons.RadioButton.CreateInstance();
        _radioButtonPressed.Volume = .2f;

        _keyCollected = Sounds.Notifiers.KeyCollected.CreateInstance();
        _keyCollected.Volume = .05f;

        _foodEaten = Sounds.Notifiers.FoodEaten.CreateInstance();
        _foodEaten.Volume = .3f;

        _chalkCollecting = Sounds.Notifiers.ChalkCollecting.CreateInstance();
        _chalkCollecting.Volume = .5f;

        _chalkDrawing = Sounds.Notifiers.ChalkDrawing.CreateInstance();
        _chalkDrawing.Volume = .35f;

        _heroRun = Sounds.Sprites.Hero.Run.CreateInstance();
        _heroRun.Volume = .1f;
        _heroRun.IsLooped = true;

        _heroGetHit = Sounds.Sprites.Hero.GetHit.CreateInstance();
        _heroGetHit.Volume = .1f;

        _gameWon = Sounds.Transiters.GameWon.CreateInstance();
        _gameWon.Volume = .1f;

        _gameOvered = Sounds.Transiters.GameOvered.CreateInstance();
        _gameOvered.Volume = .15f;

        _guardAttackMissed = Sounds.Sprites.Guard.AttackMissed.CreateInstance();
        _guardAttackMissed.Volume = .1f;

        _guardAttackHit = Sounds.Sprites.Guard.AttackHit.CreateInstance();
        _guardAttackHit.Volume = .1f;

        var gameMenuMusic = Sounds.Music.GameMenuMusic;
        _gameMenuMusic = gameMenuMusic.CreateInstance();
        _gameMenuMusic.Volume = 0;
        _gameMenuMusicDurationMs = gameMenuMusic.Duration.TotalMilliseconds;
        _gameMenuMusicMaxVolume = .3f;

        var gameRunningMusic = Sounds.Music.GameRunningMusic;
        _gameRunningMusic = gameRunningMusic.CreateInstance();
        _gameRunningMusic.Volume = 0;
        _gameRunningMusicDurationMs = gameRunningMusic.Duration.TotalMilliseconds;
        _gameRunningMusicMaxVolume = .3f;
    }

    public static void PlayButtonPressedSound()
    {
        _buttonPressed.Play();
    }

    public static void PlayRadioButtonPressedSound()
    {
        _radioButtonPressed.Play();
    }

    public static void PlayKeyCollectedSound()
    {
        _keyCollected.Play();
    }

    public static void PlayFoodEatenSound()
    {
        _foodEaten.Play();
    }

    public static void PlayGameWonSound()
    {
        _gameWon.Play();
    }

    public static void PlayGameOveredSound()
    {
        _gameOvered.Play();
    }

    public static void PlayChalkDrawingSound()
    {
        _chalkDrawing.Play();
    }

    public static void PlayChalkCollectingSound()
    {
        _chalkCollecting.Play();
    }

    public static void PlayHeroRunSound()
    {
        _heroRun.Play();
    }

    public static void PausePlayingHeroRunSound()
    {
        _heroRun.Pause();
    }

    public static void StopPlayingHeroRunSound()
    {
        _heroRun.Stop();
    }

    public static void PlayGuardAttackMissedSound()
    {
        _guardAttackMissed.Play();
    }

    public static void PlayGuardAttackHitSound()
    {
        _guardAttackHit.Play();
    }

    public static void PlayHeroGetHitSound()
    {
        _heroGetHit.Play();
    }

    public static void ChangeGameRunningMusicVolume(float changePercentage)
    {
        if (changePercentage is 0)
        {
            return;
        }

        var volumeChange = _gameRunningMusic.Volume * changePercentage / 100;

        var newVolume = _gameRunningMusic.Volume + volumeChange;

        if (changePercentage < 0)
        {
            newVolume = newVolume < 0 ? 0 : newVolume;
        }
        else
        {
            newVolume = newVolume > _gameRunningMusicMaxVolume ? _gameRunningMusicMaxVolume : newVolume;
        }

        _gameRunningMusic.Volume = newVolume;
    }

    public async static Task PlayGameRunningMusicAsync(float playingDurationPercentage, CancellationToken cancellationToken)
    {
        await StartPlayingMusicWithFadeAsync(
            _gameRunningMusic, _gameRunningMusicDurationMs, _gameRunningMusicMaxVolume, playingDurationPercentage, cancellationToken);
    }

    public async static Task PlayGameMenuMusicAsync(float playingDurationPercentage, CancellationToken cancellationToken)
    {
        await StartPlayingMusicWithFadeAsync(
            _gameMenuMusic, _gameMenuMusicDurationMs, _gameMenuMusicMaxVolume, playingDurationPercentage, cancellationToken);
    }
    static readonly Semaphore locker = new(1, 1);
    private async static Task StartPlayingMusicWithFadeAsync(
        SoundEffectInstance music, double musicDurationMs, float musicMaxVolume, float playingDurationPercentage, CancellationToken cancellationToken)
    {
        var playingDurationMs = musicDurationMs * playingDurationPercentage / 100;

        music.Play();

        while (music.Volume < musicMaxVolume)
        {
            var newVolume = music.Volume + FadeValue;
            newVolume = newVolume > musicMaxVolume ? musicMaxVolume : newVolume;

            music.Volume = newVolume;

            await Task.Delay(FadeDelayMs, CancellationToken.None);
        }

        await Task.Delay((int)playingDurationMs, cancellationToken).ContinueWith(task => task.Exception == default);

        await StopPlayingMusicWithFadeAsync(music);
    }

    private async static Task StopPlayingMusicWithFadeAsync(SoundEffectInstance music)
    {
        while (music.Volume > FadeValue)
        {
            var newVolume = music.Volume - FadeValue;
            newVolume = newVolume < 0 ? 0 : newVolume;

            music.Volume = newVolume;

            await Task.Delay(FadeDelayMs);
        }

        music.Stop();
    }
}
