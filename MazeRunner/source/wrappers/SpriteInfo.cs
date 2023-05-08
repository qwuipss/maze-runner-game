using MazeRunner.Components;
using MazeRunner.Drawing;
using MazeRunner.Sprites;
using Microsoft.Xna.Framework;

namespace MazeRunner.Wrappers;

public class SpriteInfo : MazeRunnerGameComponent
{
    public Sprite Sprite { get; init; }

    public Vector2 Position { get; private set; }

    public SpriteInfo(Sprite sprite, Vector2 position)
    {
        Sprite = sprite;
        Position = position;
    }

    public override void Update(MazeRunnerGame game, GameTime gameTime)
    {
        Sprite.Update(game, gameTime);

        Position = game.SpritesPositions[Sprite];
    }

    public override void Draw(GameTime gameTime)
    {
        Drawer.DrawSprite(Sprite, Position);
    }
}
