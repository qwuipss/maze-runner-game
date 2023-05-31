using MazeRunner.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MazeRunner.MazeBase.Tiles.States;

public class ChalkMarkIdleState : ChalkMarkBaseState
{
    private readonly Texture2D _texture;

    public override Texture2D Texture => _texture;

    public ChalkMarkIdleState()
    {
        _texture = RandomHelper.Choice(TextureChancePairs);
    }

    public override IMazeTileState ProcessState(GameTime gameTime)
    {
        return this;
    }
}