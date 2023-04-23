#region Usings
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace MazeRunner;

public abstract class MazeTile
{
    public const int Size = Settings.MazeTileSize;

    public abstract Texture2D Texture { get; }
    public abstract CellType CellType { get; }
}
