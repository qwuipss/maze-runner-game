#region Usings
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace MazeRunner.Content;

public static class Fonts
{
    private const string ContentDirectory = "fonts";

    public static SpriteFont NotificationFont { get; private set; }

    public static void Load(Game game)
    {
        NotificationFont = game.Content.Load<SpriteFont>($"{ContentDirectory}/notification");
    }
}