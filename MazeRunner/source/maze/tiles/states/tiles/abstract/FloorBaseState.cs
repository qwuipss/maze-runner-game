using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Collections.Immutable;
using static MazeRunner.Content.Textures.MazeTiles;

namespace MazeRunner.MazeBase.Tiles.States;

public abstract class FloorBaseState : MazeTileBaseState
{
    protected static readonly IDictionary<Texture2D, float> TextureChancePairs;

    static FloorBaseState()
    {
        TextureChancePairs = new Dictionary<Texture2D, float>
        {
            { Floor_1, .25f },
            { Floor_2, .30f },
            { Floor_3, .20f },
            { Floor_4, .25f }
        }
        .ToImmutableDictionary();
    }

    public override int FramesCount => 1;

    protected override double UpdateTimeDelayMs => double.MaxValue;
}