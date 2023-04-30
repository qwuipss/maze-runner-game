#region Usings
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace MazeRunner.Content;

public static class Textures
{
    public static class MazeTiles
    {
        private const string ContentDirectory = "mazetiles";

        public static Texture2D Floor { get; private set; }
        public static Texture2D Wall { get; private set; }

        public static class MazeTraps
        {
            private const string ContentDirectory = $"{MazeTiles.ContentDirectory}/mazetraps";

            public static Texture2D DropTrap { get; private set; }
            public static Texture2D BayonetTrap { get; private set; }

            public static void Load(Game game)
            {
                DropTrap = game.Content.Load<Texture2D>($"{ContentDirectory}/dropTrap");
                BayonetTrap = game.Content.Load<Texture2D>($"{ContentDirectory}/bayonetTrap");
            }
        }

        public static void Load(Game game)
        {
            Floor = game.Content.Load<Texture2D>($"{ContentDirectory}/floor");
            Wall = game.Content.Load<Texture2D>($"{ContentDirectory}/wall");

            MazeTraps.Load(game);
        }
    }

    public static void Load(Game game)
    {
        MazeTiles.Load(game);
    }
}
