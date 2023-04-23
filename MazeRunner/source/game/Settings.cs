namespace MazeRunner;

internal static class Settings
{
    public static int MazeWidth = 41; // odd only
    public static int MazeHeight = 27; // odd only

    public const int MazeTileWidth = 16;
    public const int MazeTileHeight = 16;

    public static int WindowWidth = MazeWidth * MazeTileWidth;
    public static int WindowHeight = MazeHeight * MazeTileHeight;
}
