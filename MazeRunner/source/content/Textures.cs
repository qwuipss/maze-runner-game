using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MazeRunner.Content;

public static class Textures
{
    public static class MazeTiles
    {
        private const string BaseContentDirectory = "mazetiles";

        private const string ContentDirectory = "tiles";

        public static Texture2D Floor { get; private set; }
        public static Texture2D Wall { get; private set; }
        public static Texture2D Exit { get; private set; }

        public static class MazeTraps
        {
            private const string ContentDirectory = $"{BaseContentDirectory}/traps";

            public static Texture2D DropTrap { get; private set; }
            public static Texture2D BayonetTrap { get; private set; }

            public static void Load(Game game)
            {
                DropTrap = game.Content.Load<Texture2D>($"{ContentDirectory}/dropTrap");
                BayonetTrap = game.Content.Load<Texture2D>($"{ContentDirectory}/bayonetTrap");
            }
        }

        public static class MazeItems
        {
            private const string ContentDirectory = $"{BaseContentDirectory}/items";

            public static Texture2D Key { get; private set; }

            public static void Load(Game game)
            {
                Key = game.Content.Load<Texture2D>($"{ContentDirectory}/key");
            }
        }

        public static void Load(Game game)
        {
            var dir = $"{BaseContentDirectory}/{ContentDirectory}";

            Floor = game.Content.Load<Texture2D>($"{dir}/floor");
            Wall = game.Content.Load<Texture2D>($"{dir}/wall");
            Exit = game.Content.Load<Texture2D>($"{dir}/exit");

            MazeTraps.Load(game);
            MazeItems.Load(game);
        }
    }

    public static class Sprites
    {
        private const string ContentDirectory = "sprites";

        public static class Hero
        {
            private const string ContentDirectory = $"{Sprites.ContentDirectory}/hero";

            public static Texture2D HeroIdle { get; private set; }
            public static Texture2D HeroRun { get; private set; }

            public static void Load(Game game)
            {
                HeroIdle = game.Content.Load<Texture2D>($"{ContentDirectory}/idle");
                HeroRun = game.Content.Load<Texture2D>($"{ContentDirectory}/run");
            }
        }
    }

    public static void Load(Game game)
    {
        MazeTiles.Load(game);
        Sprites.Hero.Load(game);
    }
}