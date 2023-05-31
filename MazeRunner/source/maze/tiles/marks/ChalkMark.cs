using MazeRunner.MazeBase.Tiles.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
