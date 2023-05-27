using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using static MazeRunner.GameBase.Settings;

namespace MazeRunner.Managers;

public static class KeyboardManager
{
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
}