#region Usings
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace MazeRunner;

public static class TilesTextures
{
    public static Texture2D Floor { get; private set; }
    public static Texture2D Wall { get; private set; }

    public static void LoadTextures(Game game)
    {
        Floor = game.Content.Load<Texture2D>("floor");
        Wall = game.Content.Load<Texture2D>("wall");
    }
}
