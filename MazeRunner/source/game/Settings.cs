using Microsoft.Xna.Framework.Input;

namespace MazeRunner.GameBase;

internal static class Settings
{
    public static Keys PauseSwitchButton => Keys.Escape;

    public static Keys MoveUp => Keys.W;
    public static Keys MoveDown => Keys.S;
    public static Keys MoveLeft => Keys.A;
    public static Keys MoveRight => Keys.D;
}