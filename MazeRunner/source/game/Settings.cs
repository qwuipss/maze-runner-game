using Microsoft.Xna.Framework.Input;

namespace MazeRunner;

internal static class Settings
{
    public const int MazeWidth = 57; // odd only
    public const int MazeHeight = 33; // odd only

    public const int TileSetDimension = 16;

    public const int WindowWidth = MazeWidth * TileSetDimension;
    public const int WindowHeight = MazeHeight * TileSetDimension;

    #region Controls
    public static Keys MoveForward = Keys.W;
    public static Keys MoveBack = Keys.S;
    public static Keys MoveLeft = Keys.A;
    public static Keys MoveRight = Keys.D;
    #endregion
}