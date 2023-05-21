using MazeRunner.Wrappers;

namespace MazeRunner.Sprites.States;

public class GuardChaseState : GuardMoveBaseState
{
    private readonly SpriteInfo _heroInfo;

    private readonly SpriteInfo _guardInfo;
    private readonly MazeInfo _mazeInfo;

    public GuardChaseState(ISpriteState previousState, SpriteInfo heroInfo, SpriteInfo guardInfo, MazeInfo mazeInfo) : base(previousState)
    {
        _heroInfo = heroInfo;

        _guardInfo = guardInfo;
        _mazeInfo = mazeInfo;
    }


}
