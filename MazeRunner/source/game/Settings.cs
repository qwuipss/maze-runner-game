using Microsoft.Xna.Framework.Input;

namespace MazeRunner;

internal static class Settings
{
    public static int MazeWidth = 51; // odd only
    public static int MazeHeight = 41; // odd only

    public const int TileSetDimension = 16;

    public static int WindowWidth = MazeWidth * TileSetDimension;
    public static int WindowHeight = MazeHeight * TileSetDimension;

    public const double FindKeyTextMaxShowTimeMs = 3000;

    #region Controls
    public static Keys MoveForward = Keys.W;
    public static Keys MoveBack = Keys.S;
    public static Keys MoveLeft = Keys.A;
    public static Keys MoveRight = Keys.D;
    #endregion
}