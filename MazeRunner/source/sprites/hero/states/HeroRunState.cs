using MazeRunner.Content;
using MazeRunner.Managers;
using MazeRunner.MazeBase;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MazeRunner.Sprites.States;

public class HeroRunState : HeroBaseState
{
    public override Texture2D Texture => Textures.Sprites.Hero.Run;

    public override int FramesCount => 4;

    public override double UpdateTimeDelayMs => 85;

    public HeroRunState(ISpriteState previousState, Hero hero, Maze maze) : base(previousState, hero, maze)
    {
    }

    public override ISpriteState ProcessState(GameTime gameTime)
    {
        var movement = ProcessMovement(gameTime);

        if (movement == Vector2.Zero)
        {
            return new HeroIdleState(this, Hero, Maze);
        }

        Hero.Position += movement;

        ProcessFrameEffect(movement);

        if (CollidesWithTraps(Hero, Maze, true, out var trapType))
        {
            return GetTrapCollidingState(trapType);
        }

        ProcessItemsColliding();

        HandleChalkCrossDrawing(gameTime);

        base.ProcessState(gameTime);

        return this;
    }

    private void ProcessItemsColliding()
    {
        if (CollisionManager.CollidesWithItems(Hero, Hero.Position, Maze, out var itemInfo))
        {
            itemInfo.Item.Collect();
        }
    }
}