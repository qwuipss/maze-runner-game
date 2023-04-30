namespace MazeRunner;

internal static class Settings
{
    public const int MazeWidth = 41; // odd only
    public const int MazeHeight = 27; // odd only

    public const int MazeTileSize = 16;

    public const int WindowWidth = MazeWidth * MazeTileSize;
    public const int WindowHeight = MazeHeight * MazeTileSize;
}
