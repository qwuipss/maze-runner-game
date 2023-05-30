using MazeRunner.Content;
using MazeRunner.MazeBase;
using MazeRunner.Wrappers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MazeRunner.Sprites.States;

public class HeroIdleState : HeroBaseState
{
    private readonly SpriteInfo _heroInfo;

    private readonly Maze _maze;

    public override Texture2D Texture => Textures.Sprites.Hero.Idle;

    public override int FramesCount => 2;

    public override double UpdateTimeDelayMs => 300;

    public HeroIdleState(ISpriteState previousState, SpriteInfo heroInfo, Maze maze) : base(previousState)
    {
        _heroInfo = heroInfo;
        _maze = maze;
    }

    public HeroIdleState(SpriteInfo heroInfo, Maze maze) : this(null, heroInfo, maze)
    {
    }

    public override ISpriteState ProcessState(GameTime gameTime)
    {
        var movement = ProcessMovement(_heroInfo, _maze, gameTime);

        if (CollidesWithTraps(_heroInfo, _maze, true, out var trapType))
        {
            return GetTrapCollidingState(trapType);
        }

        if (movement != Vector2.Zero)
        {
            ProcessFrameEffect(movement);

            _heroInfo.Position += movement;

            return new HeroRunState(this, _heroInfo, _maze);
        }

        base.ProcessState(gameTime);

        return this;
    }
}