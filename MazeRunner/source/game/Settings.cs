using Microsoft.Xna.Framework.Input;

namespace MazeRunner;

internal static class Settings
{
    public static int MazeWidth = 21; // odd only
    public static int MazeHeight = 21; // odd only

    #region Controls
    public static Keys MoveForward = Keys.W;
    public static Keys MoveBack = Keys.S;
    public static Keys MoveLeft = Keys.A;
    public static Keys MoveRight = Keys.D;
    #endregion
}