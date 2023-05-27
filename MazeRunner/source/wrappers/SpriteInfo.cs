using MazeRunner.Components;
using MazeRunner.Drawing;
using MazeRunner.GameBase;
using MazeRunner.GameBase.States;
using MazeRunner.Sprites;
using Microsoft.Xna.Framework;

namespace MazeRunner.Wrappers;

public class SpriteInfo : MazeRunnerGameComponent
{
    public override event GameComponentProvider NeedDisposeNotify;

    public Sprite Sprite { get; init; }

    public Vector2 Position { get; set; }

    public SpriteInfo(Sprite sprite, Vector2 position)
    {
        Sprite = sprite;
        Position = position;
    }

    public override void Update(GameTime gameTime)
    {
        Sprite.Update(gameTime);
    }

    public override void Draw(GameTime gameTime)
    {
        Drawer.DrawSprite(Sprite, Position);
    }
}
