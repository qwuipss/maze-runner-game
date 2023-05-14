using MazeRunner.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MazeRunner.MazeBase.Tiles.States;

public class WallIdleState : WallBaseState
{
    private readonly Texture2D _texture;

    public WallIdleState()
    {
        _texture = RandomHelper.Choice(Textures);
    }

    public override Texture2D Texture
    {
        get
        {
            return _texture;
        }
    }

    public override IMazeTileState ProcessState(GameTime gameTime)
    {
        return this;
    }
}
