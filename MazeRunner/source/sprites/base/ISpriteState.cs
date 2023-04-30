#region Usings
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace MazeRunner.Sprites;

public interface ISpriteState
{
    public Texture2D Texture { get; }

    public int FramesCount { get; }

    public int Width { get; }

    public int Height { get; }

    public Point GetCurrentAnimationPoint(GameTime gameTime);

    public ISpriteState ProcessState();
}
