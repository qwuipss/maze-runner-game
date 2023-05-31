using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Collections.Immutable;
using static MazeRunner.Content.Textures.MazeTiles.MazeMarks.Chalk;

namespace MazeRunner.MazeBase.Tiles.States;

public abstract class ChalkMarkBaseState : MazeTileBaseState
{
    protected static readonly IDictionary<Texture2D, float> TextureChancePairs;

    static ChalkMarkBaseState()
    {
        TextureChancePairs = new Dictionary<Texture2D, float>
        {
            { Cross_1, .25f },
            { Cross_2, .25f },
            { Cross_3, .25f },
            { Cross_4, .25f }
        }
        .ToImmutableDictionary();
    }

    public override int FramesCount => 1;

    protected override double UpdateTimeDelayMs => double.MaxValue;
}