using MazeRunner.Wrappers;

namespace MazeRunner.Gui.Buttons.States;

public abstract class ButtonPushBaseState : ButtonBaseState
{
    protected double ElapsedGameTimeMs { get; set; }

    protected virtual double UpdateTimeDelayMs
    {
        get
        {
            return 20;
        }
    }

    protected ButtonPushBaseState(ButtonInfo buttonInfo) : base(buttonInfo)
    {
    }
}