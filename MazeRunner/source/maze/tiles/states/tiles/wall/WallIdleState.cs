using Microsoft.Xna.Framework;

namespace MazeRunner.MazeBase.Tiles.States;

public class WallIdleState : WallBaseState
{
    public override IMazeTileState ProcessState(GameTime gameTime)
    {
        return this;
    }
}
