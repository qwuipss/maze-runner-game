using MazeRunner.Content;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MazeRunner.Managers;

public static class SoundManager
{
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
        private static readonly SoundEffectInstance _buttonPressed;

        private static readonly SoundEffectInstance _radioButtonPressed;

        static Buttons()
        {
            _buttonPressed = Sounds.Buttons.Button.CreateInstance();
            _buttonPressed.Volume = .3f;

            _radioButtonPressed = Sounds.Buttons.RadioButton.CreateInstance();
            _radioButtonPressed.Volume = .2f;
        }

        public static void PlayButtonPressedSound()
        {
            Play(_buttonPressed);
        }

        public static void PlayRadioButtonPressedSound()
        {
            Play(_radioButtonPressed);
        }
    }

    public static class Notifiers
    {
        private static readonly SoundEffectInstance _keyCollected;

        private static readonly SoundEffectInstance _foodEaten;

        private static readonly SoundEffectInstance _chalkDrawing;

        private static readonly SoundEffectInstance _chalkCollecting;

        static Notifiers()
        {
            _keyCollected = Sounds.Notifiers.KeyCollected.CreateInstance();
            _keyCollected.Volume = .05f;

            _foodEaten = Sounds.Notifiers.FoodEaten.CreateInstance();
            _foodEaten.Volume = .3f;

            _chalkCollecting = Sounds.Notifiers.ChalkCollecting.CreateInstance();
            _chalkCollecting.Volume = .5f;

            _chalkDrawing = Sounds.Notifiers.ChalkDrawing.CreateInstance();
            _chalkDrawing.Volume = .35f;
        }

        public static void PlayChalkDrawingSound()
        {
            Play(_chalkDrawing);
        }

        public static void PlayChalkCollectedSound()
        {
            Play(_chalkCollecting);
        }

        public static void PlayKeyCollectedSound()
        {
            Play(_keyCollected);
        }

        public static void PlayFoodEatenSound()
        {
            Play(_foodEaten);
        }
    }

    public static class Sprites
    {
        public static class Hero
        {
            private static readonly SoundEffectInstance _run;

            private static readonly SoundEffectInstance _getHit;

            private static readonly float _soundsVolume;

            static Hero()
            {
                _soundsVolume = .1f;

                _run = Sounds.Sprites.Hero.Run.CreateInstance();
                _run.Volume = _soundsVolume;
                _run.IsLooped = true;

                _getHit = Sounds.Sprites.Hero.GetHit.CreateInstance();
                _getHit.Volume = _soundsVolume;
            }

            public static void PlayGetHitSound()
            {
                Play(_getHit);
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
            private static readonly SoundEffect _attackMissed;

            private static readonly SoundEffect _attackHit;

            private static readonly float _soundsVolume;

            public static SoundEffectInstance AttackMissed
            {
                get
                {
                    var sound = _attackMissed.CreateInstance();
                    sound.Volume = _soundsVolume;

                    return sound;
                }
            }

            public static SoundEffectInstance AttackHit
            {
                get
                {
                    var sound = _attackHit.CreateInstance();
                    sound.Volume = _soundsVolume;

                    return sound;
                }
            }

            static Guard()
            {
                _soundsVolume = .1f;

                _attackMissed = Sounds.Sprites.Guard.AttackMissed;

                _attackHit = Sounds.Sprites.Guard.AttackHit;
            }

            public static void PlayAttackMissedSound(SoundEffectInstance attackMissedSound)
            {
                Play(attackMissedSound);
            }

            public async static Task PlayAttackHitAndHeroGetHitSoundsAsync(SoundEffectInstance attakHitSound)
            {
                await Task.Factory.StartNew(
                    async () =>
                    {
                        PlayAttackHitSound(attakHitSound);
                        await Task.Delay(PauseDelayMs);
                        Hero.PlayGetHitSound();
                    });
            }

            private static void PlayAttackHitSound(SoundEffectInstance attackHitSound)
            {
                Play(attackHitSound);
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

            private readonly MusicBreaker _playAfterDelayBreaker;

            public event Action MusicPlayed;

            public float MaxVolume { get; init; }

            public MusicPlayer(SoundEffect music, float maxVolume)
            {
                _music = music.CreateInstance();
                _music.Volume = 0;
                _musicDuration = music.Duration.TotalMilliseconds;

                _musicBreaker = new MusicBreaker();
                _playAfterDelayBreaker = new MusicBreaker();

                MaxVolume = maxVolume;
            }

            public async Task StartPlayingMusicWithFadeAsync(float playingDurationPercentage)
            {
                var playingDurationMs = _musicDuration * playingDurationPercentage / 100;

                _music.Play();

                while (_music.Volume < MaxVolume)
                {
                    var newVolume = _music.Volume + FadeValue;
                    newVolume = newVolume > MaxVolume ? MaxVolume : newVolume;

                    _music.Volume = newVolume;

                    await Task.Delay(FadeDelayMs, CancellationToken.None);
                }

                var cancellationToken = _musicBreaker.CancellationToken;

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

                var cancellationToken = _playAfterDelayBreaker.CancellationToken;

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

            public void StopWaitingPlayingDelay()
            {
                _playAfterDelayBreaker.Break();
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
