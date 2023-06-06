using MazeRunner.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MazeRunner.Drawing.Writers;

public abstract class TextWriter : MazeRunnerGameComponent
{
    public abstract float ScaleFactor { get; }

    public abstract string Text { get; }

    public SpriteFont Font { get; init; }

    public Color Color { get; init; }

    public bool IsDead { get; protected set; }

    public virtual float DrawingPriority => 0.05f;
}