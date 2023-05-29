using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using static MazeRunner.GameBase.Settings;

namespace MazeRunner.Managers;

public static class KeyboardManager
{
    private const double PauseSwitchPressDelayMs = 250;

    private static double _pauseSwitchLastPressElapsedTimeMs;

    private static bool _isPauseSwitchOnCooldown;

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
        var keyboardState = Keyboard.GetState();

        if (_isPauseSwitchOnCooldown)
        {
            _pauseSwitchLastPressElapsedTimeMs += gameTime.ElapsedGameTime.TotalMilliseconds;

            if (_pauseSwitchLastPressElapsedTimeMs > PauseSwitchPressDelayMs)
            {
                _isPauseSwitchOnCooldown = false;

                _pauseSwitchLastPressElapsedTimeMs -= PauseSwitchPressDelayMs;
            }
        }

        if (!_isPauseSwitchOnCooldown && keyboardState.IsKeyDown(PauseSwitchButton))
        {
            _isPauseSwitchOnCooldown = true;

            return true;
        }

        return false;
    }
}