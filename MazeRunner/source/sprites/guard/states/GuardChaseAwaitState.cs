using MazeRunner.Content;
using MazeRunner.Wrappers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MazeRunner.Sprites.States;

public class GuardChaseAwaitState : GuardMoveBaseState
{
    private readonly SpriteInfo _heroInfo;
    private readonly SpriteInfo _guardInfo;

    private readonly MazeInfo _mazeInfo;

    public GuardChaseAwaitState(ISpriteState previousState, SpriteInfo heroInfo, SpriteInfo guardInfo, MazeInfo mazeInfo) : base(previousState)
    {
        _heroInfo = heroInfo;
        _guardInfo = guardInfo;

        _mazeInfo = mazeInfo;
    }

    public override Texture2D Texture
    {
        get
        {
            return Textures.Sprites.Guard.Idle;
        }
    }

    public override int FramesCount
    {
        get
        {
            return 4;
        }
    }

    public override double UpdateTimeDelayMs
    {
        get
        {
            return double.MaxValue;
        }
    }

    public override ISpriteState ProcessState(GameTime gameTime)
    {
        if (CollidesWithTraps(_guardInfo, _mazeInfo, false, out var _))
        {
            return new GuardWalkState(this, _heroInfo, _guardInfo, _mazeInfo);
        }

        if (!PathToHeroExist(_heroInfo, _guardInfo, _mazeInfo, out var pathToHero))
        {
            return new GuardIdleState(this, _heroInfo, _guardInfo, _mazeInfo);
        }

        var direction = GetMovementDirection(_guardInfo, pathToHero);

        if (ProcessMovement(_guardInfo, direction, _mazeInfo.Maze, gameTime))
        {
            return new GuardChaseState(this, _heroInfo, _guardInfo, _mazeInfo);
        }

        return this;
    }
}