#region Usings
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace MazeRunner.Content;

public static class Fonts
{
    private const string ContentDirectory = "fonts";

    public static SpriteFont BaseFont { get; private set; }

    public static void Load(Game game)
    {
        BaseFont = game.Content.Load<SpriteFont>($"{ContentDirectory}/notification");
    }
}