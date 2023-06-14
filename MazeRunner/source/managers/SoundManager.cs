using MazeRunner.Content;
using MazeRunner.GameBase;
using MazeRunner.MazeBase.Tiles;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MazeRunner.Managers;

public static class SoundManager
{
    public class SoundEffectData
    {
        public SoundEffect SoundEffect { get; init; }

        public float Volume { get; set; }

        public SoundEffectData(SoundEffect soundEffect, float volume = 0)
        {
            SoundEffect = soundEffect;
            Volume = volume;
        }

        public SoundEffectData Copy()
        {
            return new SoundEffectData(SoundEffect, Volume);
        }
    }

    public static class Buttons
    {
        private static readonly SoundEffectData _buttonPressedSoundData;

        private static readonly SoundEffectData _radioButtonPressedSoundData;

        static Buttons()
        {
            _buttonPressedSoundData = new SoundEffectData(Sounds.Buttons.Button, .3f);
            _radioButtonPressedSoundData = new SoundEffectData(Sounds.Buttons.RadioButton, .2f);
        }

        public static void PlayButtonPressedSound()
        {
            Play(_buttonPressedSoundData);
        }

        public static void PlayRadioButtonPressedSound()
        {
            Play(_radioButtonPressedSoundData);
        }
    }

    public static class Notifiers
    {
        private static readonly SoundEffectData _keyCollected;

        private static readonly SoundEffectData _foodEatenSoundData;

        private static readonly SoundEffectData _chalkCollectingSoundData;

        private static readonly SoundEffectData _chalkDrawingSoundData;

        static Notifiers()
        {
            _keyCollected = new SoundEffectData(Sounds.Notifiers.KeyCollected, .15f);

            _foodEatenSoundData = new SoundEffectData(Sounds.Notifiers.FoodEaten, .3f);

            _chalkCollectingSoundData = new SoundEffectData(Sounds.Notifiers.ChalkCollecting, .5f);

            _chalkDrawingSoundData = new SoundEffectData(Sounds.Notifiers.ChalkDrawing, .35f);
        }

        public static void PlayChalkDrawingSound()
        {
            Play(_chalkDrawingSoundData);
        }

        public static void PlayChalkCollectedSound()
        {
            Play(_chalkCollectingSoundData);
        }

        public static void PlayFoodEatenSound()
        {
            Play(_foodEatenSoundData);
        }

        public static void PlayKeyCollectedSound()
        {
            Play(_keyCollected);
        }
    }

    public static class Sprites
    {
        public static class Common
        {
            private const float AbyssFallSoundMaxVolume = .5f;

            private const float AbyssFallSoundPlayingRadius = GameConstants.AssetsFrameSize * 2.5f;

            private static readonly SoundEffectData _abyssFallSoundData;

            static Common()
            {
                _abyssFallSoundData = new SoundEffectData(Sounds.Sprites.Common.AbyssFall);
            }

            public static void PlayAbyssFallSound(float distanceToObject)
            {
                PlayDynamicVolumeSound(distanceToObject, AbyssFallSoundPlayingRadius, GetDynamicVolume, _abyssFallSoundData);
            }

            private static float GetDynamicVolume(float distanceToObject)
            {
                return SoundManager.GetDynamicVolume(distanceToObject, AbyssFallSoundMaxVolume, AbyssFallSoundPlayingRadius);
            }
        }

        public static class Hero
        {
            private static readonly SoundEffectInstance _runSoundEffectInstance;

            private static readonly SoundEffectData _getPiercedSoundData;

            private static readonly SoundEffectData _getHitSoundData;

            private static readonly SoundEffectData _dyingFall;

            static Hero()
            {
                var soundsVolume = .1f;

                _runSoundEffectInstance = Sounds.Sprites.Hero.Run.CreateInstance();
                _runSoundEffectInstance.Volume = soundsVolume;
                _runSoundEffectInstance.IsLooped = true;

                _getPiercedSoundData = new SoundEffectData(Sounds.Sprites.Hero.GetPierced, soundsVolume);

                _getHitSoundData = new SoundEffectData(Sounds.Sprites.Hero.GetHit, soundsVolume);

                _dyingFall = new SoundEffectData(Sounds.Sprites.Hero.DyingFall, soundsVolume);
            }

            public static async void PlayGetHitSoundWithDelay()
            {
                await Task.Delay(PauseDelayMs);

                Play(_getHitSoundData);
            }

            public static async void PlayGetPiercedSoundWithDelay()
            {
                await Task.Delay((int)(PauseDelayMs * 1.25));

                Play(_getPiercedSoundData);
            }

            public static async void PlayDyingFallSoundWithDelay()
            {
                await Task.Delay((int)(PauseDelayMs * 3.2));

                Play(_dyingFall);
            }

            public static void PlayRunSound()
            {
                Play(_runSoundEffectInstance);
            }

            public static void PauseRunSoundIfPlaying()
            {
                PauseIfPlaying(_runSoundEffectInstance);
            }

            public static void StopPlayingRunSoundIfPlaying()
            {
                StopIfNot(_runSoundEffectInstance);
            }

            public static void PlayTrapDeathSound(TrapType trapType)
            {
                switch (trapType)
                {
                    case TrapType.Bayonet:
                        PlayGetPiercedSoundWithDelay();
                        PlayDyingFallSoundWithDelay();
                        break;
                    case TrapType.Drop:
                        Common.PlayAbyssFallSound(0);
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        public static class Guard
        {
            private const float DeathSoundPlayingRadius = GameConstants.AssetsFrameSize * 2.5f;

            private const float DeathSoundMaxVolume = .4f;

            private const float RunSoundPlayingRadius = GameConstants.AssetsFrameSize * 2.5f;

            private const float RunSoundMaxVolume = .4f;

            private static readonly SoundEffectData _attackMissedSoundData;

            private static readonly SoundEffectData _attackHitSoundData;

            private static readonly SoundEffectData _runSoundData;

            private static readonly SoundEffectData _getPiercedSoundData;

            private static readonly SoundEffectData _dyingFallSoundData;

            public static SoundEffectData RunSoundData => _runSoundData.Copy();

            static Guard()
            {
                var attckSoundsVolume = .1f;

                _attackMissedSoundData = new SoundEffectData(Sounds.Sprites.Guard.AttackMissed, attckSoundsVolume);
                _attackHitSoundData = new SoundEffectData(Sounds.Sprites.Guard.AttackHit, attckSoundsVolume);
                _runSoundData = new SoundEffectData(Sounds.Sprites.Guard.Run);
                _getPiercedSoundData = new SoundEffectData(Sounds.Sprites.Guard.GetPierced);
                _dyingFallSoundData = new SoundEffectData(Sounds.Sprites.Guard.DyingFall);
            }

            public static void PlayAttackMissedSound()
            {
                Play(_attackMissedSoundData);
            }

            public static void PlayAttackHitSound()
            {
                Play(_attackHitSoundData);
            }

            public static void ProcessRunSoundPlaying(MazeRunner.Sprites.Guard guard, float distanceToObject)
            {
                ProcessPlayingContinuousDynamicVolumeSound(distanceToObject, RunSoundPlayingRadius, GetRunSoundDynamicVolume, guard.RunSoundEffectInstance);
            }

            public static void PauseRunSoundIfPlaying(MazeRunner.Sprites.Guard guard)
            {
                PauseIfPlaying(guard.RunSoundEffectInstance);
            }

            public static void DisposeRunSound(MazeRunner.Sprites.Guard guard)
            {
                guard.RunSoundEffectInstance.Dispose();
            }

            public static void PlayTrapDeathSound(TrapType trapType, float distanceToObject)
            {
                switch (trapType)
                {
                    case TrapType.Drop:
                        Common.PlayAbyssFallSound(distanceToObject);
                        break;
                    case TrapType.Bayonet:
                        PlayGetPiercedSound(distanceToObject);
                        PlayDyingFallSoundWithDelay(distanceToObject);
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }

            private static void PlayGetPiercedSound(float distanceToObject)
            {
                PlayDynamicVolumeSound(distanceToObject, DeathSoundPlayingRadius, GetDeathSoundDynamicVolume, _getPiercedSoundData);
            }

            private static async void PlayDyingFallSoundWithDelay(float distanceToObject)
            {
                await Task.Delay((int)(PauseDelayMs * 2.9));

                PlayDynamicVolumeSound(distanceToObject, DeathSoundPlayingRadius, GetDeathSoundDynamicVolume, _dyingFallSoundData);
            }

            private static float GetRunSoundDynamicVolume(float distanceToObject)
            {
                var volume = GetDynamicVolume(distanceToObject, RunSoundMaxVolume, RunSoundPlayingRadius);

                return volume;
            }

            private static float GetDeathSoundDynamicVolume(float distanceToObject)
            {
                var volume = GetDynamicVolume(distanceToObject, DeathSoundMaxVolume, DeathSoundPlayingRadius);

                return volume;
            }
        }
    }

    public static class Exit
    {
        private static readonly SoundEffectData _keyOpeningSoundData;

        private static readonly SoundEffectData _openSoundData;

        static Exit()
        {
            _keyOpeningSoundData = new SoundEffectData(Sounds.Exit.KeyOpening, .3f);
            _openSoundData = new SoundEffectData(Sounds.Exit.Open, .05f);
        }

        public static void PlayKeyOpeningSound()
        {
            Play(_keyOpeningSoundData);
        }

        public static async void PlayOpenSoundWithDelay()
        {
            await Task.Delay(PauseDelayMs * 3);

            Play(_openSoundData);
        }
    }

    public static class Transiters
    {
        private static readonly SoundEffectData _gameWonSoundData;

        private static readonly SoundEffectData _gameOveredSoundData;

        static Transiters()
        {
            _gameWonSoundData = new SoundEffectData(Sounds.Transiters.GameWon, .1f);
            _gameOveredSoundData = new SoundEffectData(Sounds.Transiters.GameOvered, .15f);
        }

        public static void PlayGameWonSound()
        {
            Play(_gameWonSoundData);
        }

        public static void PlayGameOveredSound()
        {
            Play(_gameOveredSoundData);
        }
    }

    public static class Music
    {
        private class MusicBreaker
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

        public class MusicPlayer
        {
            private readonly SoundEffectInstance _music;

            private readonly double _musicDuration;

            private readonly MusicBreaker _musicBreaker;

            public event Action MusicPlayed;

            public float MusicMaxVolume { get; init; }

            public MusicPlayer(SoundEffect music, float maxVolume)
            {
                _music = music.CreateInstance();
                _music.Volume = 0;
                _musicDuration = music.Duration.TotalMilliseconds;

                _musicBreaker = new MusicBreaker();

                MusicMaxVolume = maxVolume;
            }

            public async Task StartPlayingMusicWithFade(float playingDurationPercentage)
            {
                var playingDurationMs = _musicDuration * playingDurationPercentage / 100;
                var cancellationToken = _musicBreaker.CancellationToken;

                Play(_music);

                while (_music.Volume < MusicMaxVolume)
                {
                    var newVolume = _music.Volume + FadeValue;
                    newVolume = newVolume > MusicMaxVolume ? MusicMaxVolume : newVolume;

                    _music.Volume = newVolume;

                    await Task.Delay(FadeDelayMs, cancellationToken).ContinueWith(task => { });
                }

                await Task.Delay((int)playingDurationMs, cancellationToken).ContinueWith(task => { });

                await StopPlayingMusicWithFade();

                if (!cancellationToken.IsCancellationRequested)
                {
                    MusicPlayed.Invoke();
                }
            }

            public async Task PlayAfterDelay(float percentageDelay, float playingDurationPercentage)
            {
                var delay = _musicDuration * percentageDelay / 100;
                var cancellationToken = _musicBreaker.CancellationToken;

                await Task.Delay((int)delay, cancellationToken).ContinueWith(task => { });

                if (!cancellationToken.IsCancellationRequested)
                {
                    await StartPlayingMusicWithFade(playingDurationPercentage);
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
                    newVolume = newVolume > MusicMaxVolume ? MusicMaxVolume : newVolume;
                }

                _music.Volume = newVolume;
            }

            private async Task StopPlayingMusicWithFade()
            {
                while (_music.Volume > FadeValue)
                {
                    var newVolume = _music.Volume - FadeValue;
                    newVolume = newVolume < 0 ? 0 : newVolume;

                    _music.Volume = newVolume;

                    await Task.Delay(FadeDelayMs);
                }

                StopIfNot(_music);
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
            private static readonly SoundEffectData _activateSoundEffectData;

            private static readonly SoundEffectData _deactivateSoundEffectData;

            static Drop()
            {
                _activateSoundEffectData = new SoundEffectData(Sounds.Traps.Drop.Activate);
                _deactivateSoundEffectData = new SoundEffectData(Sounds.Traps.Drop.Deactivate);
            }

            public static void PlayActivateSound(float distanceToObject)
            {
                Traps.PlayActivateSound(_activateSoundEffectData, distanceToObject);
            }

            public static void PlayDeactivateSound(float distanceToObject)
            {
                Traps.PlayDeactivateSound(_deactivateSoundEffectData, distanceToObject);
            }
        }

        public static class Bayonet
        {
            private static readonly SoundEffectData _activateSoundData;

            private static readonly SoundEffectData _deactivateSoundData;

            static Bayonet()
            {
                _activateSoundData = new SoundEffectData(Sounds.Traps.Bayonet.Activate);
                _deactivateSoundData = new SoundEffectData(Sounds.Traps.Bayonet.Deactivate);
            }

            public static void PlayActivateSound(float distanceToObject)
            {
                Traps.PlayActivateSound(_activateSoundData, distanceToObject);
            }

            public static void PlayDeactivateSound(float distanceToObject)
            {
                Traps.PlayDeactivateSound(_deactivateSoundData, distanceToObject);
            }
        }

        private const float SoundPlayingRadius = GameConstants.AssetsFrameSize * 2.5f;

        private const float SoundMaxVolume = .35f;

        private static void PlayActivateSound(SoundEffectData activateSoundEffectData, float distanceToObject)
        {
            PlayDynamicVolumeSound(distanceToObject, SoundPlayingRadius, GetDynamicVolume, activateSoundEffectData);
        }

        private static void PlayDeactivateSound(SoundEffectData deactivateSoundEffectData, float distanceToObject)
        {
            PlayDynamicVolumeSound(distanceToObject, SoundPlayingRadius, GetDynamicVolume, deactivateSoundEffectData);
        }

        private static float GetDynamicVolume(float distanceToObject)
        {
            var volume = SoundManager.GetDynamicVolume(distanceToObject, SoundMaxVolume, SoundPlayingRadius);

            return volume;
        }
    }

    private const int FadeDelayMs = 250;

    private const float FadeValue = .05f;

    public const int PauseDelayMs = 125;

    private static void Play(SoundEffectData soundEffectData)
    {
        var soundEffectInstance = soundEffectData.SoundEffect.CreateInstance();
        soundEffectInstance.Volume = soundEffectData.Volume;

        Play(soundEffectInstance);
    }

    private static void Play(SoundEffectInstance soundEffectInstance)
    {
        if (!soundEffectInstance.IsDisposed)
        {
            soundEffectInstance.Play();
        }
    }

    private static void PauseIfPlaying(SoundEffectInstance soundEffectInstance)
    {
        if (!soundEffectInstance.IsDisposed && soundEffectInstance.State is SoundState.Playing)
        {
            soundEffectInstance.Pause();
        }
    }

    private static void StopIfNot(SoundEffectInstance soundEffectInstance)
    {
        if (!soundEffectInstance.IsDisposed && soundEffectInstance.State is not SoundState.Stopped)
        {
            soundEffectInstance.Stop();
        }
    }

    private static float GetDynamicVolume(float distanceToObject, float maxVolume, float soundPlayingRadius)
    {
        return maxVolume * (1 - distanceToObject / soundPlayingRadius);
    }

    private static void PlayDynamicVolumeSound(
        float distanceToObject, float soundPlayingRadius, Func<float, float> dynamicVolumeGetter, SoundEffectData soundEffectData)
    {
        if (distanceToObject > soundPlayingRadius)
        {
            return;
        }

        var volume = dynamicVolumeGetter.Invoke(distanceToObject);
        soundEffectData.Volume = volume;

        Play(soundEffectData);
    }

    private static void ProcessPlayingContinuousDynamicVolumeSound(
        float distanceToObject, float soundPlayingRadius, Func<float, float> dynamicVolumeGetter, SoundEffectInstance soundEffectInstance)
    {
        if (distanceToObject > soundPlayingRadius)
        {
            PauseIfPlaying(soundEffectInstance);

            return;
        }

        soundEffectInstance.Volume = dynamicVolumeGetter.Invoke(distanceToObject);

        if (soundEffectInstance.State is not SoundState.Playing)
        {
            Play(soundEffectInstance);
        }
    }
}
