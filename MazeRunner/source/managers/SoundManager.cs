using MazeRunner.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System.Threading;
using System.Threading.Tasks;

namespace MazeRunner.Managers;

public static class SoundManager
{
    private const int FadeDelayMs = 250;

    private const float FadeValue = .05f;

    private const float GameMenuMusicMaxVolume = .25f;

    private static readonly SoundEffectInstance _buttonPressed;

    private static readonly SoundEffectInstance _radioButtonPressed;

    private static readonly SoundEffectInstance _keyCollected;

    private static readonly SoundEffectInstance _foodEaten;

    private static readonly SoundEffectInstance _chalkDrawing;

    private static readonly SoundEffectInstance _chalkCollecting;

    private static readonly SoundEffectInstance _heroRun;

    private static readonly SoundEffectInstance _guardAttackMissed;

    private static readonly SoundEffectInstance _guardAttackHit;

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
        _keyCollected.Volume = .15f;

        _foodEaten = Sounds.Notifiers.FoodEaten.CreateInstance();
        _foodEaten.Volume = .3f;

        _chalkCollecting = Sounds.Notifiers.ChalkCollecting.CreateInstance();
        _chalkCollecting.Volume = .5f;

        _chalkDrawing = Sounds.Notifiers.ChalkDrawing.CreateInstance();
        _chalkDrawing.Volume = .35f;

        _heroRun = Sounds.Sprites.Hero.Run.CreateInstance();
        _heroRun.Volume = .1f;
        _heroRun.IsLooped = true;

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

    public static void PlayGuardAttackMissedSound()
    {
        _guardAttackMissed.Play();
    }

    public static void PlayGuardAttackHitSound()
    {
        _guardAttackHit.Play();
    }

    public async static Task PlayGameRunningMusicAsync(float playingDurationPercentage)
    {
        await StartPlayingMusicWithFadeAsync(_gameRunningMusic, _gameRunningMusicDurationMs, _gameRunningMusicMaxVolume, playingDurationPercentage);
    }

    public static async Task StopPlayingGameRunningMusicAsync()
    {
        await StopPlayingMusicWithFadeAsync(_gameRunningMusic);
    }

    public async static Task PlayGameMenuMusicAsync(float playingDurationPercentage)
    {
        await StartPlayingMusicWithFadeAsync(_gameMenuMusic, _gameMenuMusicDurationMs, _gameMenuMusicMaxVolume, playingDurationPercentage);
    }

    public async static Task StopPlayingGameMenuMusicAsync()
    {
        await StopPlayingMusicWithFadeAsync(_gameMenuMusic);
    }

    private async static Task StartPlayingMusicWithFadeAsync(
        SoundEffectInstance music, double musicDurationMs, float musicMaxVolume, float playingDurationPercentage)
    {
        var playingDurationMs = musicDurationMs * playingDurationPercentage / 100;

        music.Play();

        while (music.Volume < musicMaxVolume)
        {
            var newVolume = music.Volume + FadeValue;
            newVolume = newVolume > musicMaxVolume ? musicMaxVolume : newVolume;

            music.Volume = newVolume;

            await Task.Delay(FadeDelayMs);
        }

        await Task.Delay((int)playingDurationMs);

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
