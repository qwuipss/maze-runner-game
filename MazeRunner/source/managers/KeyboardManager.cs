using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using static MazeRunner.Settings;

namespace MazeRunner.Managers;

public static class KeyboardManager
{
    public static IEnumerable<Vector2> ProcessHeroMovement()
    {
        var keyboardState = Keyboard.GetState();

        if (keyboardState.IsKeyDown(MoveUp))
        {
            yield return -Vector2.UnitY;
        }

        if (keyboardState.IsKeyDown(MoveDown))
        {
            yield return Vector2.UnitY;
        }

        if (keyboardState.IsKeyDown(MoveLeft))
        {
            yield return -Vector2.UnitX;
        }

        if (keyboardState.IsKeyDown(MoveRight))
        {
            yield return Vector2.UnitX;
        }
    }
}