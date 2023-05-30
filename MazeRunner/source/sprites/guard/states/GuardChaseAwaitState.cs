using MazeRunner.Content;
using MazeRunner.MazeBase;
using MazeRunner.Wrappers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MazeRunner.Sprites.States;

public class GuardChaseAwaitState : GuardMoveBaseState
{
    private readonly SpriteInfo _heroInfo;

    private readonly SpriteInfo _guardInfo;

    private readonly Maze _maze;

    public GuardChaseAwaitState(ISpriteState previousState, SpriteInfo heroInfo, SpriteInfo guardInfo, Maze maze) : base(previousState)
    {
        _heroInfo = heroInfo;
        _guardInfo = guardInfo;

        _maze = maze;
    }

    public override Texture2D Texture => Textures.Sprites.Guard.Idle;

    public override int FramesCount => 4;

    public override double UpdateTimeDelayMs => double.MaxValue;

    public override ISpriteState ProcessState(GameTime gameTime)
    {
        if (CollidesWithTraps(_guardInfo, _maze, true, out var trapType))
        {
            return GetTrapCollidingState(trapType);
        }

        if (CollidesWithTraps(_guardInfo, _maze, false, out var _))
        {
            return new GuardWalkState(this, _heroInfo, _guardInfo, _maze);
        }

        if (!PathToHeroExist(_heroInfo, _guardInfo, _maze, out var pathToHero))
        {
            return new GuardIdleState(this, _heroInfo, _guardInfo, _maze);
        }

        var direction = GetMovementDirection(_guardInfo, pathToHero);

        if (ProcessMovement(_guardInfo, direction, _maze, gameTime))
        {
            return new GuardChaseState(this, _heroInfo, _guardInfo, _maze);
        }

        return this;
    }
}