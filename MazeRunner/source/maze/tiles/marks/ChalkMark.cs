using MazeRunner.MazeBase.Tiles.States;

namespace MazeRunner.MazeBase.Tiles;

public class ChalkMark : MazeMark
{
    public override TileType TileType => TileType.Mark;

    public override float DrawingPriority => .75f;

    public ChalkMark()
    {
        State = new ChalkMarkIdleState();
    }
}
