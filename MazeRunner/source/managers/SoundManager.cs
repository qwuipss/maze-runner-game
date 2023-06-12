using MazeRunner.Content;
using MazeRunner.Extensions;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MazeRunner.Managers;

public static class SoundManager
{
    private class MultiSoundPlayer
    {
        private readonly SoundEffect _soundEffect;

        private readonly SoundEffectInstance _soundEffectInstance;

        private readonly float _volume;

        private SoundEffectInstance SoundEffectInstance
        {
            get
            {
                var soundEffect = _soundEffect.CreateInstance();
                soundEffect.Volume = _volume;

                return soundEffect;
            }
        }

        public MultiSoundPlayer(SoundEffect soundEffect, float volume)
        {
            if (!volume.InRange(0, 1))
            {
                throw new ArgumentOutOfRangeException(nameof(volume));
            }

            _volume = volume;

            _soundEffect = soundEffect;
            
            _soundEffectInstance = _soundEffect.CreateInstance();
            _soundEffectInstance.Volume = volume;
        }

        public void Play()
        {
            if (_soundEffectInstance.State is SoundState.Playing)
            {
                SoundEffectInstance.Play();
            }
            else
            {
                _soundEffectInstance.Play();
            }
        }
    }

    public class MusicBreaker
    {
        private Lazy<CancellationTokenSource> _cancellationTokenSource;

        public CancellationToken CancellationToken => _cancellationTokenSource.Value.Token;

        public MusicBreaker()
        {
            _cancellationTokenSource = new Lazy<CancellationTokenSource>();
        }

        public void Break()
        {
            _cancellationTokenSource.Value.Cancel();

            _cancellationTokenSource = new Lazy<CancellationTokenSource>();
        }
    }

    public static class Buttons
    {
        private static readonly MultiSoundPlayer _buttonPressedSoundPlayer;

        private static readonly MultiSoundPlayer _radioButtonPressedSoundPlayer;

        static Buttons()
        {
            _buttonPressedSoundPlayer = new MultiSoundPlayer(Sounds.Buttons.Button, .3f);
            _radioButtonPressedSoundPlayer = new MultiSoundPlayer(Sounds.Buttons.RadioButton, .2f);
        }

        public static void PlayButtonPressedSound()
        {
            _buttonPressedSoundPlayer.Play();
        }

        public static void PlayRadioButtonPressedSound()
        {
            _radioButtonPressedSoundPlayer.Play();
        }
    }

    public static class Notifiers
    {
        private static readonly SoundEffectInstance _keyCollected;

        private static readonly MultiSoundPlayer _foodEatenSoundPlayer;

        private static readonly MultiSoundPlayer _chalkCollectingSoundPlayer;

        private static readonly MultiSoundPlayer _chalkDrawingSoundPlayer;

        static Notifiers()
        {
            _keyCollected = Sounds.Notifiers.KeyCollected.CreateInstance();
            _keyCollected.Volume = .05f;

            _foodEatenSoundPlayer = new MultiSoundPlayer(Sounds.Notifiers.FoodEaten, .3f);

            _chalkCollectingSoundPlayer = new MultiSoundPlayer(Sounds.Notifiers.ChalkCollecting, .5f);

            _chalkDrawingSoundPlayer = new MultiSoundPlayer(Sounds.Notifiers.ChalkDrawing, .35f);
        }

        public static void PlayChalkDrawingSound()
        {
            _chalkDrawingSoundPlayer.Play();
        }

        public static void PlayChalkCollectedSound()
        {
            _chalkCollectingSoundPlayer.Play();
        }

        public static void PlayFoodEatenSound()
        {
            _foodEatenSoundPlayer.Play();
        }

        public static void PlayKeyCollectedSound()
        {
            Play(_keyCollected);
        }
    }

    public static class Sprites
    {
        public static class Hero
        {
            private static readonly SoundEffectInstance _run;

            private static readonly MultiSoundPlayer _getHitSoundPlayer;

            static Hero()
            {
                var soundVolume = .1f;

                _run = Sounds.Sprites.Hero.Run.CreateInstance();
                _run.Volume = soundVolume;
                _run.IsLooped = true;

                _getHitSoundPlayer = new MultiSoundPlayer(Sounds.Sprites.Hero.GetHit, soundVolume);
            }

            public static void PlayGetHitSound()
            {
                _getHitSoundPlayer.Play();
            }

            public static void PlayRunSound()
            {
                Play(_run);
            }

            public static void PausePlayingRunSound()
            {
                Pause(_run);
            }

            public static void StopPlayingRunSound()
            {
                Stop(_run);
            }
        }

        public static class Guard
        {
            private static readonly MultiSoundPlayer _attackMissedSoundPlayer;

            private static readonly MultiSoundPlayer _attackHitSoundPlayer;

            static Guard()
            {
                var soundVolume = .1f;

                _attackMissedSoundPlayer = new MultiSoundPlayer(Sounds.Sprites.Guard.AttackMissed, soundVolume);

                _attackHitSoundPlayer = new MultiSoundPlayer(Sounds.Sprites.Guard.AttackHit, soundVolume);
            }

            public static void PlayAttackMissedSound()
            {
                _attackMissedSoundPlayer.Play();
            }

            public async static Task PlayAttackHitAndHeroGetHitSoundsAsync()
            {
                await Task.Factory.StartNew(
                    async () =>
                    {
                        PlayAttackHitSound();
                        await Task.Delay(PauseDelayMs);
                        Hero.PlayGetHitSound();
                    });
            }

            private static void PlayAttackHitSound()
            {
                _attackHitSoundPlayer.Play();
            }
        }
    }

    public static class Transiters
    {
        private static readonly SoundEffectInstance _gameWon;

        private static readonly SoundEffectInstance _gameOvered;

        static Transiters()
        {
            _gameWon = Sounds.Transiters.GameWon.CreateInstance();
            _gameWon.Volume = .1f;

            _gameOvered = Sounds.Transiters.GameOvered.CreateInstance();
            _gameOvered.Volume = .15f;
        }

        public static void PlayGameWonSound()
        {
            Play(_gameWon);
        }

        public static void PlayGameOveredSound()
        {
            Play(_gameOvered);
        }
    }

    public static class Music
    {
        public class MusicPlayer
        {
            private readonly SoundEffectInstance _music;

            private readonly double _musicDuration;

            private readonly MusicBreaker _musicBreaker;

            public event Action MusicPlayed;

            public float MaxVolume { get; init; }

            public MusicPlayer(SoundEffect music, float maxVolume)
            {
                _music = music.CreateInstance();
                _music.Volume = 0;
                _musicDuration = music.Duration.TotalMilliseconds;

                _musicBreaker = new MusicBreaker();

                MaxVolume = maxVolume;
            }

            public async Task StartPlayingMusicWithFadeAsync(float playingDurationPercentage)
            {
                var playingDurationMs = _musicDuration * playingDurationPercentage / 100;
                var cancellationToken = _musicBreaker.CancellationToken;

                _music.Play();

                while (_music.Volume < MaxVolume)
                {
                    var newVolume = _music.Volume + FadeValue;
                    newVolume = newVolume > MaxVolume ? MaxVolume : newVolume;

                    _music.Volume = newVolume;

                    await Task.Delay(FadeDelayMs, cancellationToken).ContinueWith(task => { });
                }

                await Task.Delay((int)playingDurationMs, cancellationToken).ContinueWith(task => { });

               await StopPlayingMusicWithFadeAsync();

                if (!cancellationToken.IsCancellationRequested)
                {
                    MusicPlayed.Invoke();
                }
            }

            public async Task PlayAfterDelayAsync(float percentageDelay, float playingDurationPercentage)
            {
                var delay = _musicDuration * percentageDelay / 100;
                var cancellationToken = _musicBreaker.CancellationToken;

                await Task.Delay((int)delay, cancellationToken).ContinueWith(task => { });

                if (!cancellationToken.IsCancellationRequested)
                {
                    await StartPlayingMusicWithFadeAsync(playingDurationPercentage);
                }
            }

            public void StopPlaying()
            {
                _musicBreaker.Break();
            }

            public void ChangeVolume(float changePercentage)
            {
                if (changePercentage is 0)
                {
                    return;
                }

                var volumeChange = _music.Volume * changePercentage / 100;

                var newVolume = _music.Volume + volumeChange;

                if (changePercentage < 0)
                {
                    newVolume = newVolume < 0 ? 0 : newVolume;
                }
                else
                {
                    newVolume = newVolume > MaxVolume ? MaxVolume : newVolume;
                }

                _music.Volume = newVolume;
            }

            private async Task StopPlayingMusicWithFadeAsync()
            {
                while (_music.Volume > FadeValue)
                {
                    var newVolume = _music.Volume - FadeValue;
                    newVolume = newVolume < 0 ? 0 : newVolume;

                    _music.Volume = newVolume;

                    await Task.Delay(FadeDelayMs);
                }

                _music.Stop();
            }
        }

        public static readonly MusicPlayer GameMenuMusic;

        public static readonly MusicPlayer GameRunningMusic;

        static Music()
        {
            GameMenuMusic = new MusicPlayer(Sounds.Music.GameMenu, .3f);
            GameRunningMusic = new MusicPlayer(Sounds.Music.GameRunningMusic, .3f);
        }
    }

    public static class Traps
    {
        public static class Drop
        {
            private static readonly SoundEffectInstance _activate;

            private static readonly SoundEffectInstance _deactivate;

            static Drop()
            {
                _activate = Sounds.Traps.Drop.Activate.CreateInstance();
                _deactivate = Sounds.Traps.Drop.Deactivate.CreateInstance();
            }
        }

        public static class Bayonet
        {
            private static readonly SoundEffectInstance _activate;

            private static readonly SoundEffectInstance _deactivate;

            static Bayonet()
            {
                _activate = Sounds.Traps.Bayonet.Activate.CreateInstance();
                _deactivate = Sounds.Traps.Bayonet.Deactivate.CreateInstance();
            }
        }
    }

    private const int FadeDelayMs = 250;

    private const float FadeValue = .05f;

    public const int PauseDelayMs = 125;

    private static void Play(SoundEffectInstance soundEffect)
    {
        soundEffect.Play();
    }

    private static void Pause(SoundEffectInstance soundEffect)
    {
        soundEffect.Pause();
    }

    private static void Stop(SoundEffectInstance soundEffect)
    {
        soundEffect.Stop();
    }
}
