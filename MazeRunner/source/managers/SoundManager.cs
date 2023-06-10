using MazeRunner.Content;
using Microsoft.Xna.Framework.Audio;

namespace MazeRunner.Managers;

public static class SoundManager
{
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

    static SoundManager()
    {
        _buttonPressed = Sounds.Buttons.Button.CreateInstance();
        _buttonPressed.Volume = .8f;

        _radioButtonPressed = Sounds.Buttons.RadioButton.CreateInstance();
        _radioButtonPressed.Volume = .55f;

        _keyCollected = Sounds.Notifiers.KeyCollected.CreateInstance();
        _keyCollected.Volume = .15f;

        _foodEaten = Sounds.Notifiers.FoodEaten.CreateInstance();
        _foodEaten.Volume = .5f;

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

        _gameMenuMusic = Sounds.Music.GameMenuMusic.CreateInstance();
        _gameMenuMusic.Volume = .25f;

        _gameRunningMusic = Sounds.Music.GameRunningMusic.CreateInstance();
        _gameRunningMusic.Volume = .2f;
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

    public static void PlayGameRunningMusic()
    {
        _gameRunningMusic.Play();
    }

    public static void StopPlayingGameRunningMusic()
    {
        _gameRunningMusic.Stop();
    }

    public static void PlayGameMenuMusic()
    {
        _gameMenuMusic.Play();
    }

    public static void StopPlayingGameMenuMusic()
    {
        _gameMenuMusic.Stop();
    }
}
