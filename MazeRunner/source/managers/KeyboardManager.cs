using MazeRunner.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using static MazeRunner.Settings;

namespace MazeRunner.Managers;

public static class KeyboardManager
{
    public static IEnumerable<Vector2> ProcessHeroMovement(Sprite hero)
    {
        var speed = hero.Speed;

        if (Keyboard.GetState().IsKeyDown(MoveUp))
        {
            yield return new Vector2(0, -speed.Y);
        }

        if (Keyboard.GetState().IsKeyDown(MoveDown))
        {
            yield return new Vector2(0, speed.Y);
        }

        if (Keyboard.GetState().IsKeyDown(MoveLeft))
        {
            yield return new Vector2(-speed.X, 0);
        }

        if (Keyboard.GetState().IsKeyDown(MoveRight))
        {
            yield return new Vector2(speed.X, 0);
        }
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