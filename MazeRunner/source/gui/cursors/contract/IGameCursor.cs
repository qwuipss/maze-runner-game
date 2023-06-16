using Microsoft.Xna.Framework.Graphics;

namespace MazeRunner.Gui.Cursors;

public interface IGameCursor
{
    public Texture2D Texture { get; }

    public float ScaleFactor { get; }
}