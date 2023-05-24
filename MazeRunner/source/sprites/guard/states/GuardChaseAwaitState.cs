using MazeRunner.Content;
using MazeRunner.Wrappers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        var pathToHero = FindPathToHero(_mazeInfo.Maze, _heroInfo, _guardInfo);

        if (!pathToHero.Any())
        {
            return new GuardIdleState(this, _heroInfo, _guardInfo, _mazeInfo);
        }

        var direction = GetMovementDirection(_heroInfo, _guardInfo, _mazeInfo);

        if (direction == Vector2.Zero)
        {
            return new GuardIdleState(this, _heroInfo, _guardInfo, _mazeInfo);
        }

        if (ProcessMovement(_guardInfo, direction, _mazeInfo.Maze, gameTime))
        {
            return new GuardChaseState(this, _heroInfo, _guardInfo, _mazeInfo);
        }

        return this;
    }
}