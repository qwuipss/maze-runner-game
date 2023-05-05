namespace MazeRunner;

internal static class Settings
{
    public const int MazeWidth = 15; // odd only
    public const int MazeHeight = 15; // odd only

    public const int TileSetDimension = 16;

    public const int WindowWidth = MazeWidth * TileSetDimension;
    public const int WindowHeight = MazeHeight * TileSetDimension;
}
