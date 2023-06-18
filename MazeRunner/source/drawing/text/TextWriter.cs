using MazeRunner.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MazeRunner.Drawing.Writers;

public abstract class TextWriter : MazeRunnerGameComponent
{
    public abstract event Action WriterDiedNotify;

    public abstract float ScaleFactor { get; }

    public abstract string Text { get; }

    public virtual float DrawingPriority => 0.075f;

    public SpriteFont Font { get; init; }

    public Color Color { get; init; }
}