using MazeRunner.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MazeRunner.Gui.Cursors;

public class BaseCursor : IGameCursor
{
    public float _scaleFactor;

    public Texture2D Texture => Textures.Gui.Cursors.BaseCursor;

    public float ScaleFactor => _scaleFactor;

    public BaseCursor(float scaleDivider, int viewWidth)
    {
        _scaleFactor = viewWidth / scaleDivider;
    }
}
