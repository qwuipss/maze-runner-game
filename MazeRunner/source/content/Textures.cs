using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MazeRunner.Content;

public static class Textures
{
    public static class MazeTiles
    {
        private const string BaseContentDirectory = "mazetiles";

        private const string ContentDirectory = "tiles";

        public static Texture2D Floor_1 { get; private set; }
        public static Texture2D Floor_2 { get; private set; }
        public static Texture2D Floor_3 { get; private set; }
        public static Texture2D Floor_4 { get; private set; }

        public static Texture2D Wall_1 { get; private set; }
        public static Texture2D Wall_2 { get; private set; }
        public static Texture2D Wall_3 { get; private set; }

        public static Texture2D Exit { get; private set; }

        public static class MazeTraps
        {
            private const string ContentDirectory = $"{BaseContentDirectory}/traps";

            public static Texture2D Drop { get; private set; }
            public static Texture2D Bayonet { get; private set; }

            public static void Load(Game game)
            {
                Drop = game.Content.Load<Texture2D>($"{ContentDirectory}/drop");
                Bayonet = game.Content.Load<Texture2D>($"{ContentDirectory}/bayonet");
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

            Floor_1 = game.Content.Load<Texture2D>($"{dir}/floor_1");
            Floor_2 = game.Content.Load<Texture2D>($"{dir}/floor_2");
            Floor_3 = game.Content.Load<Texture2D>($"{dir}/floor_3");
            Floor_4 = game.Content.Load<Texture2D>($"{dir}/floor_4");

            Wall_1 = game.Content.Load<Texture2D>($"{dir}/wall_1");
            Wall_2 = game.Content.Load<Texture2D>($"{dir}/wall_2");
            Wall_3 = game.Content.Load<Texture2D>($"{dir}/wall_3");

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

            public static Texture2D Idle { get; private set; }
            public static Texture2D Run { get; private set; }
            public static Texture2D Dead { get; private set; }
            public static Texture2D Fall { get; private set; }

            public static void Load(Game game)
            {
                Idle = game.Content.Load<Texture2D>($"{ContentDirectory}/idle");
                Run = game.Content.Load<Texture2D>($"{ContentDirectory}/run");
                Dead = game.Content.Load<Texture2D>($"{ContentDirectory}/dead");
                Fall = game.Content.Load<Texture2D>($"{ContentDirectory}/fall");
            }
        }

        public static class Guard
        {
            private const string ContentDirectory = $"{Sprites.ContentDirectory}/guard";

            public static Texture2D Idle { get; private set; }

            public static void Load(Game game)
            {
                Idle = game.Content.Load<Texture2D>($"{ContentDirectory}/idle");
            }
        }

        public static void Load(Game game)
        {
            Hero.Load(game);
            Guard.Load(game);
        }
    }

    public static void Load(Game game)
    {
        MazeTiles.Load(game);
        Sprites.Load(game);
    }
}