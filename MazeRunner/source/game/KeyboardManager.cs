#region Usings
using MazeRunner.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
#endregion

namespace MazeRunner;

public static class KeyboardManager
{
    private static double _elapsedGameTimeMs;

    private const int KeyboardPollingDelayMs = 50;

    public static Vector2 ProcessHeroMovement(Hero hero, GameTime gameTime)
    {
        var movement = Vector2.Zero;
        var speed = hero.Speed;

        if (Keyboard.GetState().IsKeyDown(Keys.W))
        {
            movement -= new Vector2(0, speed.Y);
        }
        if (Keyboard.GetState().IsKeyDown(Keys.S))
        {
            movement += new Vector2(0, speed.Y);
        }
        if (Keyboard.GetState().IsKeyDown(Keys.A))
        {
            movement -= new Vector2(speed.X, 0);
        }
        if (Keyboard.GetState().IsKeyDown(Keys.D))
        {
            movement += new Vector2(speed.X, 0);
        }

        return movement;
    }

    public static bool IsPollingTimePassed(GameTime gameTime)
    {
        _elapsedGameTimeMs += gameTime.ElapsedGameTime.TotalMilliseconds;

        if (_elapsedGameTimeMs >= KeyboardPollingDelayMs)
        {
            _elapsedGameTimeMs -= KeyboardPollingDelayMs;

            return true;
        }

        return false;
    }
}