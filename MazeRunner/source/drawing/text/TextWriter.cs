using MazeRunner.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    protected abstract string Text { get; }

    protected virtual SpriteFont Font { get; init; }

    protected virtual Color Color { get; init; }

    protected virtual bool NeedWriting { get; set; }
}
