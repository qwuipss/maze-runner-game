using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Collections.Immutable;
using static MazeRunner.Content.Textures.MazeTiles.MazeItems.Food;

namespace MazeRunner.MazeBase.Tiles.States;

public abstract class FoodBaseState : MazeTileBaseState
{
    protected static readonly IDictionary<Texture2D, float> TextureChancePairs;

    static FoodBaseState()
    {
        TextureChancePairs = new Dictionary<Texture2D, float>
        {
            { Apple, .25f },
            { Bread, .25f },
            { Potato, .25f },
            { Tomato, .25f },
        }
        .ToImmutableDictionary();
    }

    public override int FramesCount => 1;

    protected override double UpdateTimeDelayMs => double.MaxValue;
}