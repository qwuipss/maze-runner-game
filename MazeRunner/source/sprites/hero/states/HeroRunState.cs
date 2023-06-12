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
        SoundManager.Sprites.Hero.PlayRunSound();
    }

    public override ISpriteState ProcessState(GameTime gameTime)
    {
        var movement = ProcessMovement(gameTime);

        if (movement == Vector2.Zero)
        {
            SoundManager.Sprites.Hero.PausePlayingRunSound();

            return new HeroIdleState(this, Hero, Maze);
        }

        Hero.Position += movement;

        ProcessFrameEffect(movement);

        if (CollidesWithTraps(Hero, Maze, true, out var trapType))
        {
            SoundManager.Sprites.Hero.PausePlayingRunSound();

            return GetTrapCollidingState(trapType);
        }

        ProcessItemsColliding();

        HandleChalkDrawing(gameTime);

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