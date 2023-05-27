using Microsoft.Xna.Framework.Input;

namespace MazeRunner.GameBase;

internal static class Settings
{
    public static int MazeWidth = 51; // odd only
    public static int MazeHeight = 51; // odd only

    #region Controls
    public static Keys MoveUp = Keys.W;
    public static Keys MoveDown = Keys.S;
    public static Keys MoveLeft = Keys.A;
    public static Keys MoveRight = Keys.D;
    #endregion
}