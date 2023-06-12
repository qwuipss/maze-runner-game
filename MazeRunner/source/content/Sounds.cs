using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace MazeRunner.Content;

public static class Sounds
{
    private const string BaseContentDirectory = "sounds";

    public static class Buttons
    {
        private const string ContentDirectory = $"{BaseContentDirectory}/buttons";

        public static SoundEffect Button { get; private set; }

        public static SoundEffect RadioButton { get; private set; }

        public static void Load(Game game)
        {
            Button = game.Content.Load<SoundEffect>($"{ContentDirectory}/button");
            RadioButton = game.Content.Load<SoundEffect>($"{ContentDirectory}/radioButton");
        }
    }

    public static class Notifiers
    {
        private const string ContentDirectory = $"{BaseContentDirectory}/notifiers";

        public static SoundEffect KeyCollected { get; private set; }

        public static SoundEffect FoodEaten { get; private set; }

        public static SoundEffect ChalkDrawing { get; private set; }

        public static SoundEffect ChalkCollecting { get; private set; }

        public static void Load(Game game)
        {
            KeyCollected = game.Content.Load<SoundEffect>($"{ContentDirectory}/keyCollected");
            FoodEaten = game.Content.Load<SoundEffect>($"{ContentDirectory}/foodEaten");
            ChalkDrawing = game.Content.Load<SoundEffect>($"{ContentDirectory}/chalkDrawing");
            ChalkCollecting = game.Content.Load<SoundEffect>($"{ContentDirectory}/chalkCollecting");
        }
    }

    public static class Sprites
    {
        public static class Hero
        {
            private const string ContentDirectory = $"{Sprites.ContentDirectory}/hero";

            public static SoundEffect Run { get; private set; }

            public static SoundEffect GetHit { get; private set; }

            public static void Load(Game game)
            {
                Run = game.Content.Load<SoundEffect>($"{ContentDirectory}/run");
                GetHit = game.Content.Load<SoundEffect>($"{ContentDirectory}/getHit");
            }
        }

        public static class Guard
        {
            private const string ContentDirectory = $"{Sprites.ContentDirectory}/guard";

            public static SoundEffect AttackMissed { get; private set; }

            public static SoundEffect AttackHit { get; private set; }

            public static void Load(Game game)
            {
                AttackMissed = game.Content.Load<SoundEffect>($"{ContentDirectory}/attackMissed");
                AttackHit = game.Content.Load<SoundEffect>($"{ContentDirectory}/attackHit");
            }
        }

        private const string ContentDirectory = $"{BaseContentDirectory}/sprites";

        public static void Load(Game game)
        {
            Hero.Load(game);
            Guard.Load(game);
        }
    }

    public static class Music
    {
        private const string ContentDirectory = $"{BaseContentDirectory}/music";

        public static SoundEffect GameMenu { get; private set; }

        public static SoundEffect GameRunningMusic { get; private set; }

        public static void Load(Game game)
        {
            GameMenu = game.Content.Load<SoundEffect>($"{ContentDirectory}/gameMenuMusic");
            GameRunningMusic = game.Content.Load<SoundEffect>($"{ContentDirectory}/gameRunningMusic");
        }
    }

    public static class Transiters
    {
        private const string ContentDirectory = $"{BaseContentDirectory}/transiters";

        public static SoundEffect GameWon { get; private set; }

        public static SoundEffect GameOvered { get; private set; }

        public static void Load(Game game)
        {
            GameWon = game.Content.Load<SoundEffect>($"{ContentDirectory}/gameWon");
            GameOvered = game.Content.Load<SoundEffect>($"{ContentDirectory}/gameOver");
        }
    }

    public static class Traps
    {
        public static class Bayonet
        {
            private const string ContentDirectory = $"{Traps.ContentDirectory}/bayonet";

            public static SoundEffect Activate { get; private set; }

            public static SoundEffect Deactivate { get; private set; }

            public static void Load(Game game)
            {
                Activate = game.Content.Load<SoundEffect>($"{ContentDirectory}/activate");
                Deactivate = game.Content.Load<SoundEffect>($"{ContentDirectory}/deactivate");
            }
        }

        public static class Drop
        {
            private const string ContentDirectory = $"{Traps.ContentDirectory}/drop";

            public static SoundEffect Activate { get; private set; }

            public static SoundEffect Deactivate { get; private set; }

            public static void Load(Game game)
            {
                Activate = game.Content.Load<SoundEffect>($"{ContentDirectory}/activate");
                Deactivate = game.Content.Load<SoundEffect>($"{ContentDirectory}/deactivate");
            }
        }

        private const string ContentDirectory = $"{BaseContentDirectory}/traps";

        public static void Load(Game game)
        {
            Bayonet.Load(game);
            Drop.Load(game);
        }
    }

    public static void Load(Game game)
    {
        Buttons.Load(game);
        Notifiers.Load(game);
        Sprites.Load(game);
        Music.Load(game);
        Transiters.Load(game);
        Traps.Load(game);
    }
}
