using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MazeRunner.Gui.Buttons.States;

public interface IButtonState
{
    public Texture2D Texture { get; }

    public int FrameWidth { get; }

    public Rectangle CurrentAnimationFrame { get; }

    public IButtonState ProcessState(GameTime gameTime);
}
