using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Drawing;
using PointXna = Microsoft.Xna.Framework.Point;
using RectangleXna = Microsoft.Xna.Framework.Rectangle;

namespace MazeRunner.Gui.Buttons.States;

public abstract class ButtonBaseState : IButtonState
{
    public abstract Texture2D Texture { get; }

    public abstract int FramesCount { get; }

    public int FrameWidth => Texture.Width / FramesCount;

    public int FrameHeight => Texture.Height;

    public RectangleXna CurrentAnimationFrame => new(CurrentAnimationFramePoint, new PointXna(FrameWidth, FrameHeight));

    protected PointXna CurrentAnimationFramePoint { get; set; }

    protected Button Button { get; init; }

    protected ButtonBaseState(Button button)
    {
        Button = button;
    }

    protected bool IsCursorHoverButton(MouseState mouseState)
    {
        var cursorPosition = mouseState.Position;
        var materialCursorBox = new RectangleF(cursorPosition.X, cursorPosition.Y, 0, 0);

        var buttonPosition = Button.Position;
        var boxScale = Button.BoxScale;

        var buttonBox = new RectangleF(buttonPosition.X, buttonPosition.Y, FrameWidth * boxScale, FrameHeight * boxScale);

        return buttonBox.IntersectsWith(materialCursorBox);
    }

    public abstract IButtonState ProcessState(GameTime gameTime);
}
