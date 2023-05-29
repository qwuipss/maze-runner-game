using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Collections.Immutable;
using static MazeRunner.Content.Textures.MazeTiles;

namespace MazeRunner.MazeBase.Tiles.States;

public abstract class WallBaseState : MazeTileBaseState
{
    protected static readonly IDictionary<Texture2D, float> TextureChancePairs;

    static WallBaseState()
    {
        TextureChancePairs = new Dictionary<Texture2D, float>
        {
            { Wall_1, .7f },
            { Wall_2, .25f },
            { Wall_3, .05f },
        }
        .ToImmutableDictionary();
    }

    public override int FramesCount => 1;

    protected override double UpdateTimeDelayMs => double.MaxValue;
}