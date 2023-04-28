#region Usings
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace MazeRunner;

public static class TilesTextures
{
    private static bool _texturesLoaded = false;

    public static Texture2D Floor { get; private set; }
    public static Texture2D Wall { get; private set; }
    public static Texture2D DropTrap { get; private set; }
    public static Texture2D BayonetTrap { get; private set; }

    public static void LoadTextures(Game game)
    {
        if (_texturesLoaded)
        {
            throw new ContentLoadException("tiles textures already loaded");
        }

        Floor = game.Content.Load<Texture2D>("floor");
        Wall = game.Content.Load<Texture2D>("wall");
        DropTrap = game.Content.Load<Texture2D>("dropTrap");
        BayonetTrap = game.Content.Load<Texture2D>("bayonetTrap");

        _texturesLoaded = true;
    }
}
