using MazeRunner.Components;
using MazeRunner.Drawing;
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

    public override void Update(MazeRunnerGame game, GameTime gameTime)
    {

        if (Sprite is not Hero)
        {
            var heroInfo = game.HeroInfo;

            var hero = heroInfo.Sprite;
            var heroPosition = heroInfo.Position;

            if (Vector2.Distance(Position, heroPosition) >= hero.FrameSize * OptimizationConstants.EnemiesUpdateDistanceCoeff)
            {
                return;
            }
        }

        Sprite.Update(game, gameTime);

        if (Sprite.IsDead)
        {
            NeedDisposeNotify.Invoke(this);
        }
    }

    public override void Draw(GameTime gameTime)
    {
        Drawer.DrawSprite(Sprite, Position);
    }
}
