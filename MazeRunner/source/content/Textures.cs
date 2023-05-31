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

        public static class MazeMarks
        {
            private const string ContentDirectory = $"{BaseContentDirectory}/marks";

            public static class Chalk
            {
                private const string ContentDirectory = $"{MazeMarks.ContentDirectory}/chalk";

                public static Texture2D Cross_1 { get; private set; }
                public static Texture2D Cross_2 { get; private set; }
                public static Texture2D Cross_3 { get; private set; }
                public static Texture2D Cross_4 { get; private set; }

                public static void Load(Game game)
                {
                    Cross_1 = game.Content.Load<Texture2D>($"{ContentDirectory}/cross_1");
                    Cross_2 = game.Content.Load<Texture2D>($"{ContentDirectory}/cross_2");
                    Cross_3 = game.Content.Load<Texture2D>($"{ContentDirectory}/cross_3");
                    Cross_4 = game.Content.Load<Texture2D>($"{ContentDirectory}/cross_4");
                }
            }

            public static void Load(Game game)
            {
                Chalk.Load(game);
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
            MazeMarks.Load(game);
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
            public static Texture2D DamageTaking { get; private set; }
            public static Texture2D Dead { get; private set; }
            public static Texture2D Fall { get; private set; }

            public static void Load(Game game)
            {
                Idle = game.Content.Load<Texture2D>($"{ContentDirectory}/idle");
                Run = game.Content.Load<Texture2D>($"{ContentDirectory}/run");
                DamageTaking = game.Content.Load<Texture2D>($"{ContentDirectory}/damageTaking");
                Dead = game.Content.Load<Texture2D>($"{ContentDirectory}/dead");
                Fall = game.Content.Load<Texture2D>($"{ContentDirectory}/fall");
            }
        }

        public static class Guard
        {
            private const string ContentDirectory = $"{Sprites.ContentDirectory}/guard";

            public static Texture2D Idle { get; private set; }
            public static Texture2D Run { get; private set; }
            public static Texture2D Attack { get; private set; }
            public static Texture2D Dead { get; private set; }
            public static Texture2D Fall { get; private set; }

            public static void Load(Game game)
            {
                Idle = game.Content.Load<Texture2D>($"{ContentDirectory}/idle");
                Run = game.Content.Load<Texture2D>($"{ContentDirectory}/run");
                Attack = game.Content.Load<Texture2D>($"{ContentDirectory}/attack");
                Dead = game.Content.Load<Texture2D>($"{ContentDirectory}/dead");
                Fall = game.Content.Load<Texture2D>($"{ContentDirectory}/fall");
            }
        }

        public static void Load(Game game)
        {
            Hero.Load(game);
            Guard.Load(game);
        }
    }

    public static class Gui
    {
        private const string ContentDirectory = "gui";

        public static class Buttons
        {
            private const string ContentDirectory = $"{Gui.ContentDirectory}/buttons";

            public static class Start
            {
                private const string ContentDirectory = $"{Buttons.ContentDirectory}/start";

                public static Texture2D Idle { get; private set; }
                public static Texture2D Hover { get; private set; }
                public static Texture2D Click { get; private set; }

                public static void Load(Game game)
                {
                    Idle = game.Content.Load<Texture2D>($"{ContentDirectory}/idle");
                    Hover = game.Content.Load<Texture2D>($"{ContentDirectory}/hover");
                    Click = game.Content.Load<Texture2D>($"{ContentDirectory}/click");
                }
            }

            public static class Menu
            {
                private const string ContentDirectory = $"{Buttons.ContentDirectory}/menu";

                public static Texture2D Idle { get; private set; }
                public static Texture2D Hover { get; private set; }
                public static Texture2D Click { get; private set; }

                public static void Load(Game game)
                {
                    Idle = game.Content.Load<Texture2D>($"{ContentDirectory}/idle");
                    Hover = game.Content.Load<Texture2D>($"{ContentDirectory}/hover");
                    Click = game.Content.Load<Texture2D>($"{ContentDirectory}/click");
                }
            }

            public static class Restart
            {
                private const string ContentDirectory = $"{Buttons.ContentDirectory}/restart";

                public static Texture2D Idle { get; private set; }
                public static Texture2D Hover { get; private set; }
                public static Texture2D Click { get; private set; }

                public static void Load(Game game)
                {
                    Idle = game.Content.Load<Texture2D>($"{ContentDirectory}/idle");
                    Hover = game.Content.Load<Texture2D>($"{ContentDirectory}/hover");
                    Click = game.Content.Load<Texture2D>($"{ContentDirectory}/click");
                }
            }

            public static class Resume
            {
                private const string ContentDirectory = $"{Buttons.ContentDirectory}/resume";

                public static Texture2D Idle { get; private set; }
                public static Texture2D Hover { get; private set; }
                public static Texture2D Click { get; private set; }

                public static void Load(Game game)
                {
                    Idle = game.Content.Load<Texture2D>($"{ContentDirectory}/idle");
                    Hover = game.Content.Load<Texture2D>($"{ContentDirectory}/hover");
                    Click = game.Content.Load<Texture2D>($"{ContentDirectory}/click");
                }
            }

            public static class Quit
            {
                private const string ContentDirectory = $"{Buttons.ContentDirectory}/quit";

                public static Texture2D Idle { get; private set; }
                public static Texture2D Hover { get; private set; }
                public static Texture2D Click { get; private set; }

                public static void Load(Game game)
                {
                    Idle = game.Content.Load<Texture2D>($"{ContentDirectory}/idle");
                    Hover = game.Content.Load<Texture2D>($"{ContentDirectory}/hover");
                    Click = game.Content.Load<Texture2D>($"{ContentDirectory}/click");
                }
            }

            public static class EasyModeSelect
            {
                private const string ContentDirectory = $"{Buttons.ContentDirectory}/difficulty/easy";

                public static Texture2D Idle { get; private set; }
                public static Texture2D Hover { get; private set; }
                public static Texture2D Click { get; private set; }

                public static void Load(Game game)
                {
                    Idle = game.Content.Load<Texture2D>($"{ContentDirectory}/idle");
                    Hover = game.Content.Load<Texture2D>($"{ContentDirectory}/hover");
                    Click = game.Content.Load<Texture2D>($"{ContentDirectory}/click");
                }
            }

            public static class NormalModeSelect
            {
                private const string ContentDirectory = $"{Buttons.ContentDirectory}/difficulty/normal";

                public static Texture2D Idle { get; private set; }
                public static Texture2D Hover { get; private set; }
                public static Texture2D Click { get; private set; }

                public static void Load(Game game)
                {
                    Idle = game.Content.Load<Texture2D>($"{ContentDirectory}/idle");
                    Hover = game.Content.Load<Texture2D>($"{ContentDirectory}/hover");
                    Click = game.Content.Load<Texture2D>($"{ContentDirectory}/click");
                }
            }

            public static class HardModeSelect
            {
                private const string ContentDirectory = $"{Buttons.ContentDirectory}/difficulty/hard";

                public static Texture2D Idle { get; private set; }
                public static Texture2D Hover { get; private set; }
                public static Texture2D Click { get; private set; }

                public static void Load(Game game)
                {
                    Idle = game.Content.Load<Texture2D>($"{ContentDirectory}/idle");
                    Hover = game.Content.Load<Texture2D>($"{ContentDirectory}/hover");
                    Click = game.Content.Load<Texture2D>($"{ContentDirectory}/click");
                }
            }

            public static void Load(Game game)
            {
                Start.Load(game);
                Restart.Load(game);
                Menu.Load(game);
                Resume.Load(game);
                Quit.Load(game);

                EasyModeSelect.Load(game);
                NormalModeSelect.Load(game);
                HardModeSelect.Load(game);
            }
        }

        public static class StateShowers
        {
            private const string ContentDirectory = $"{Gui.ContentDirectory}/stateShowers";

            public static Texture2D Heart { get; private set; }
            public static Texture2D Chalk { get; private set; }

            public static void Load(Game game)
            {
                Heart = game.Content.Load<Texture2D>($"{ContentDirectory}/heart");
                Chalk = game.Content.Load<Texture2D>($"{ContentDirectory}/chalk");
            }
        }

        public static void Load(Game game)
        {
            Buttons.Load(game);
            StateShowers.Load(game);
        }
    }

    public static void Load(Game game)
    {
        MazeTiles.Load(game);
        Sprites.Load(game);
        Gui.Load(game);
    }
}