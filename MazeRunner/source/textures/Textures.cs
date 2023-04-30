#region Usings
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace MazeRunner.Content;

public static class Textures
{
    private static bool _texturesLoaded = false;

    #region MazeTiles
    public static Texture2D Floor { get; private set; }
    public static Texture2D Wall { get; private set; }
    public static Texture2D DropTrap { get; private set; }
    public static Texture2D BayonetTrap { get; private set; }
    #endregion

    public static void Load(Game game)
    {
        if (_texturesLoaded)
        {
            throw new ContentLoadException("tiles textures already loaded");
        }

        LoadMazeTiles(game);

        _texturesLoaded = true;
    }

    private static void LoadMazeTiles(Game game)
    {
        Floor = game.Content.Load<Texture2D>("floor");
        Wall = game.Content.Load<Texture2D>("wall");
        DropTrap = game.Content.Load<Texture2D>("dropTrap");
        BayonetTrap = game.Content.Load<Texture2D>("bayonetTrap");
    }
}
