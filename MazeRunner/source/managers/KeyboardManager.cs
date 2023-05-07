using MazeRunner.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using static MazeRunner.Settings;

namespace MazeRunner.source.managers;

public static class KeyboardManager
{
    private static double _elapsedGameTimeMs;

    private const int KeyboardPollingDelayMs = 50;

    public static Vector2 ProcessHeroMovement(Hero hero)
    {
        var movement = Vector2.Zero;
        var speed = hero.Speed;

        if (Keyboard.GetState().IsKeyDown(MoveForward))
        {
            movement -= new Vector2(0, speed.Y);
        }

        if (Keyboard.GetState().IsKeyDown(MoveBack))
        {
            movement += new Vector2(0, speed.Y);
        }

        if (Keyboard.GetState().IsKeyDown(MoveLeft))
        {
            movement -= new Vector2(speed.X, 0);
        }

        if (Keyboard.GetState().IsKeyDown(MoveRight))
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