using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using static MazeRunner.GameBase.Settings;

namespace MazeRunner.Managers;

public static class KeyboardManager
{
    private class CooldownButton
    {
        private readonly Keys _button;

        private readonly double _cooldownMs;

        private double _lastPressElapsedTimeMs;

        private bool _isOnCooldown;

        public CooldownButton(double cooldownMs, Keys button)
        {
            _cooldownMs = cooldownMs;
            _button = button;
        }

        public bool IsPressed(GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();

            if (_isOnCooldown)
            {
                _lastPressElapsedTimeMs += gameTime.ElapsedGameTime.TotalMilliseconds;

                if (_lastPressElapsedTimeMs > _cooldownMs)
                {
                    _isOnCooldown = false;

                    _lastPressElapsedTimeMs -= _cooldownMs;
                }
            }

            if (!_isOnCooldown && keyboardState.IsKeyDown(_button))
            {
                _isOnCooldown = true;

                return true;
            }

            return false;
        }
    }

    private static readonly CooldownButton _pauseSwitchButton;

    private static readonly CooldownButton _chalkDrawingButton;

    static KeyboardManager()
    {
        _pauseSwitchButton = new CooldownButton(250, PauseSwitch);
        _chalkDrawingButton = new CooldownButton(250, ChalkDrawing);
    }

    public static Vector2 ProcessHeroMovement()
    {
        var movementDirection = Vector2.Zero;
        var keyboardState = Keyboard.GetState();

        if (keyboardState.IsKeyDown(MoveUp))
        {
            movementDirection -= Vector2.UnitY;
        }

        if (keyboardState.IsKeyDown(MoveDown))
        {
            movementDirection += Vector2.UnitY;
        }

        if (keyboardState.IsKeyDown(MoveLeft))
        {
            movementDirection -= Vector2.UnitX;
        }

        if (keyboardState.IsKeyDown(MoveRight))
        {
            movementDirection += Vector2.UnitX;
        }

        return movementDirection;
    }

    public static bool IsGamePauseSwitched(GameTime gameTime)
    {
        return _pauseSwitchButton.IsPressed(gameTime);
    }

    public static bool IsChalkDrawingButtonPressed(GameTime gameTime)
    {
        return _chalkDrawingButton.IsPressed(gameTime);
    }
}