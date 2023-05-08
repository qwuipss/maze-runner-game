using Microsoft.Xna.Framework;

namespace MazeRunner.MazeBase.Tiles.States;

public class FloorIdleState : FloorBaseState
{
    public override IMazeTileState ProcessState(GameTime gameTime)
    {
        return this;
    }
}