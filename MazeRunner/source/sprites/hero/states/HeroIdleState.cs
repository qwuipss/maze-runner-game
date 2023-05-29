using MazeRunner.Content;
using MazeRunner.Wrappers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MazeRunner.Sprites.States;

public class HeroIdleState : HeroBaseState
{
    private readonly SpriteInfo _heroInfo;

    private readonly MazeInfo _mazeInfo;

    public override Texture2D Texture => Textures.Sprites.Hero.Idle;

    public override int FramesCount => 2;

    public override double UpdateTimeDelayMs => 300;

    public HeroIdleState(ISpriteState previousState, SpriteInfo heroInfo, MazeInfo mazeInfo) : base(previousState)
    {
        _heroInfo = heroInfo;
        _mazeInfo = mazeInfo;
    }

    public HeroIdleState(SpriteInfo heroInfo, MazeInfo mazeInfo) : this(null, heroInfo, mazeInfo)
    {
    }

    public override ISpriteState ProcessState(GameTime gameTime)
    {
        var movement = ProcessMovement(_heroInfo, _mazeInfo.Maze, gameTime);

        if (CollidesWithTraps(_heroInfo, _mazeInfo, true, out var trapType))
        {
            return GetTrapCollidingState(trapType);
        }

        if (movement != Vector2.Zero)
        {
            ProcessFrameEffect(movement);

            _heroInfo.Position += movement;

            return new HeroRunState(this, _heroInfo, _mazeInfo);
        }

        base.ProcessState(gameTime);

        return this;
    }
}