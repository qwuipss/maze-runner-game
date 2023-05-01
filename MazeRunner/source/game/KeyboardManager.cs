#region Usings
using MazeRunner.Sprites;
using MazeRunner.Sprites.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
#endregion

namespace MazeRunner;

public static class KeyboardManager
{
    private static double _elapsedGameTimeMs;

    private const int KeyboardPollingDelayMs = 50;

    public static Vector2 ProcessHeroMovement(Hero hero, GameTime gameTime)
    {
        if (!IsPollingTimePassed(gameTime))
            return Vector2.Zero;

        var movement = ProcessHeroMovement(hero);
        ProcessHeroState(hero, movement);
        ProcessHeroFrameEffect(hero, movement);

        return movement;
    }

    private static Vector2 ProcessHeroMovement(Hero hero)
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

    private static void ProcessHeroState(Hero hero, Vector2 movement)
    {
        if (movement == Vector2.Zero)
        {
            if (hero.State is not HeroIdleState)
            {
                hero.State = new HeroIdleState();
            }
        }
        else
        {
            if (hero.State is not HeroRunState)
            {
                hero.State = new HeroRunState();
            }
        }
    }

    private static void ProcessHeroFrameEffect(Hero hero, Vector2 movement)
    {
        if (movement.X > 0)
        {
            hero.FrameEffect = SpriteEffects.None;
        }
        else if (movement.X < 0)
        {
            hero.FrameEffect = SpriteEffects.FlipHorizontally;
        }
    }

    private static bool IsPollingTimePassed(GameTime gameTime)
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