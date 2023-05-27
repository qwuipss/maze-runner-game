using MazeRunner.Wrappers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MazeRunner.Gui.Buttons.States;

public class ButtonClickedState : ButtonBaseState
{
    private const double AnimationDelayMs = 20;

    private readonly ButtonInfo _buttonInfo;

    private readonly Texture2D _texture;

    private readonly int _framesCount;

    private double _elapsedGameTime;

    public override Texture2D Texture
    {
        get
        {
            return _texture;
        }
    }

    public override int FramesCount
    {
        get
        {
            return _framesCount;
        }
    }

    public ButtonClickedState(ButtonInfo buttonInfo)
    {
        _buttonInfo = buttonInfo;

        _texture = _buttonInfo.OnClickStateInfo.Texture;
        _framesCount = _buttonInfo.OnClickStateInfo.FramesCount;
    }

    public override IButtonState ProcessState(GameTime gameTime)
    {
        _elapsedGameTime += gameTime.ElapsedGameTime.TotalMilliseconds;

        if (_elapsedGameTime > AnimationDelayMs)
        {
            var animationPoint = CurrentAnimationFramePoint;

            if (animationPoint.X == (FramesCount - 1) * FrameWidth)
            {
                _buttonInfo.Button.OnClick.Invoke();

                return new ButtonIdleState(_buttonInfo);
            }

            var framePosX = animationPoint.X + FrameWidth;

            CurrentAnimationFramePoint = new Point(framePosX, 0);
            _elapsedGameTime -= AnimationDelayMs;
        }

        return this;
    }
}
