namespace MazeRunner;

internal static class Settings
{
    public static int MazeWidth = 41; // odd only
    public static int MazeHeight = 27; // odd only

    public const int MazeTileSize = 16;

    public static int WindowWidth = MazeWidth * MazeTileSize;
    public static int WindowHeight = MazeHeight * MazeTileSize;
}
