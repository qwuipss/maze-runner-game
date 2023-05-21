using MazeRunner.MazeBase.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeRunner.Sprites.States;

public abstract class GuardBaseState : SpriteBaseState
{
    protected GuardBaseState(ISpriteState previousState) : base(previousState)
    {
    }

    protected override GuardBaseState GetTrapCollidingState(TrapType trapType)
    {
        return this;
    }
}
