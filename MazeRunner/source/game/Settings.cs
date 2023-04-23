namespace MazeRunner;

internal static class Settings
{
    public static int MazeWidth = 41; // odd only
    public static int MazeHeight = 27; // odd only

    public const int TileWidth = 16;
    public const int TileHeight = 16;

    public static int WindowWidth = MazeWidth * TileWidth;
    public static int WindowHeight = MazeHeight * TileHeight;
}
