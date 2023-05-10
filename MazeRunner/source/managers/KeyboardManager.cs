using MazeRunner.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using static MazeRunner.Settings;

namespace MazeRunner.Managers;

public static class KeyboardManager
{
    public static Vector2 ProcessHeroMovement(Sprite hero)
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

    public static bool IsPollingTimePassed(double pollingTimeMs, ref double elapsedTime, GameTime gameTime)
    {
        elapsedTime += gameTime.ElapsedGameTime.TotalMilliseconds;

        if (elapsedTime >= pollingTimeMs)
        {
            elapsedTime -= pollingTimeMs;

            return true;
        }

        return false;
    }
}