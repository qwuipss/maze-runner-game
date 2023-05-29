using MazeRunner.Wrappers;
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

    protected ButtonInfo ButtonInfo { get; init; }

    public ButtonBaseState(ButtonInfo buttonInfo)
    {
        ButtonInfo = buttonInfo;
    }

    public int FrameWidth => Texture.Width / FramesCount;

    public int FrameHeight => Texture.Height;

    public RectangleXna CurrentAnimationFrame => new RectangleXna(CurrentAnimationFramePoint, new PointXna(FrameWidth, FrameHeight));

    protected PointXna CurrentAnimationFramePoint { get; set; }

    protected bool IsCursorHoverButton(MouseState mouseState, ButtonInfo buttonInfo)
    {
        var cursorPosition = mouseState.Position;
        var materialCursorBox = new RectangleF(cursorPosition.X, cursorPosition.Y, float.Epsilon, float.Epsilon);

        var buttonPosition = buttonInfo.Position;
        var boxScale = buttonInfo.BoxScale;

        var buttonBox = new RectangleF(buttonPosition.X, buttonPosition.Y, FrameWidth * boxScale, FrameHeight * boxScale);

        return buttonBox.IntersectsWith(materialCursorBox);
    }

    public abstract IButtonState ProcessState(GameTime gameTime);
}
