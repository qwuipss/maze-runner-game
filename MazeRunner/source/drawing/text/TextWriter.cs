using MazeRunner.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MazeRunner.Drawing;

public abstract class TextWriter : MazeRunnerGameComponent
{
    public virtual float DrawingPriority
    {
        get
        {
            return 0;
        }
    }

    public abstract float ScaleFactor { get; }

    public abstract string Text { get; }

    public virtual SpriteFont Font { get; init; }

    public virtual Color Color { get; init; }
}
