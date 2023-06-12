using MazeRunner.Content;
using MazeRunner.MazeBase;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MazeRunner.Sprites.States;

public class HeroIdleState : HeroBaseState
{
    public override Texture2D Texture => Textures.Sprites.Hero.Idle;

    public override int FramesCount => 2;

    public override double UpdateTimeDelayMs => 300;

    public HeroIdleState(ISpriteState previousState, Hero hero, Maze maze) : base(previousState, hero, maze)
    {
    }

    public HeroIdleState(Hero hero, Maze maze) : this(null, hero, maze)
    {
    }

    public override ISpriteState ProcessState(GameTime gameTime)
    {
        var movement = ProcessMovement(gameTime);

        if (CollidesWithTraps(Hero, Maze, true, out var trapType))
        {
            PlayDeathSound(trapType);

            return GetTrapCollidingState(trapType);
        }

        if (movement != Vector2.Zero)
        {
            ProcessFrameEffect(movement);

            Hero.Position += movement;

            return new HeroRunState(this, Hero, Maze);
        }

        HandleChalkDrawing(gameTime);

        base.ProcessState(gameTime);

        return this;
    }
}