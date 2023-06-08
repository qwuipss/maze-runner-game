using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeRunner.Drawing.Writers;

public class GameTutorialWriter : TextWriter
{
    private string _showingText;

    public override event Action WriterDiedNotify;

    public override float ScaleFactor => .3f;

    public override string Text => _showingText;

    public void Initialize()
    {

    }

    public override void Draw(GameTime gameTime)
    {
    }

    public override void Update(GameTime gameTime)
    {
    }
}
