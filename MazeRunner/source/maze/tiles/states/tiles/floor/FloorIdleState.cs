using MazeRunner.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MazeRunner.MazeBase.Tiles.States;

public class FloorIdleState : FloorBaseState
{
    private readonly Texture2D _texture;

    public override Texture2D Texture => _texture;

    public FloorIdleState()
    {
        _texture = RandomHelper.Choice(TextureChancePairs);
    }

    public override IMazeTileState ProcessState(GameTime gameTime)
    {
        return this;
    }
}