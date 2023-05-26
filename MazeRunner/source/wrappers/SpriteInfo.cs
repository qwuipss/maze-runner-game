using MazeRunner.Components;
using MazeRunner.Drawing;
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

    public override void Update(GameRunningState game, GameTime gameTime)
    {
        if (Sprite is not Hero)
        {
            var heroInfo = game.HeroInfo;

            var hero = heroInfo.Sprite;
            var heroPosition = heroInfo.Position;

            if (Vector2.Distance(Position, heroPosition) > Optimization.GetEnemyUpdateDistance(this))
            {
                return;
            }

            if (Sprite.IsDead
             && Vector2.Distance(Position, heroPosition) > Optimization.GetEnemyDisposingDistance(this))
            {
                NeedDisposeNotify?.Invoke(this);
            }
        }

        Sprite.Update(game, gameTime);
    }

    public override void Draw(GameTime gameTime)
    {
        Drawer.DrawSprite(Sprite, Position);
    }
}
