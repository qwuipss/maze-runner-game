using MazeRunner.Content;
using MazeRunner.Wrappers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MazeRunner.Sprites.States;

public class HeroIdleState : HeroBaseState
{
    private readonly SpriteInfo _heroInfo;
    private readonly MazeInfo _mazeInfo;

    public HeroIdleState(ISpriteState previousState, SpriteInfo heroInfo, MazeInfo mazeInfo) : base(previousState)
    {
        _heroInfo = heroInfo;
        _mazeInfo = mazeInfo;
    }

    public HeroIdleState(SpriteInfo heroInfo, MazeInfo mazeInfo) : this(null, heroInfo, mazeInfo)
    {
    }

    public override Texture2D Texture
    {
        get
        {
            return Textures.Sprites.Hero.Idle;
        }
    }

    public override int FramesCount
    {
        get
        {
            return 2;
        }
    }

    public override double UpdateTimeDelayMs
    {
        get
        {
            return 300;
        }
    }

    public override ISpriteState ProcessState(GameTime gameTime)
    {
        var movement = ProcessMovement(_heroInfo, _mazeInfo.Maze, gameTime);

        if (CollidesWithTraps(_heroInfo, _mazeInfo, out var trapType))
        {
            var deathState = GetTrapCollidingState(trapType);

            return deathState;
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
