using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public static void Load(Game game)
        {
            KeyCollected = game.Content.Load<SoundEffect>($"{ContentDirectory}/keyCollected");
            FoodEaten = game.Content.Load<SoundEffect>($"{ContentDirectory}/foodEaten");
        }
    }

    public static void Load(Game game)
    {
        Buttons.Load(game);
        Notifiers.Load(game);
    }
}
