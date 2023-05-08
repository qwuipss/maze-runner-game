using Microsoft.Xna.Framework;

namespace MazeRunner.MazeBase.Tiles.States;

public class KeyIdleState : KeyBaseState
{
    public override IMazeTileState ProcessState(GameTime gameTime)
    {
        return this;
    }
}
