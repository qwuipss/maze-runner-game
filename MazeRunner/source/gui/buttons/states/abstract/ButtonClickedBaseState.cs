using MazeRunner.Wrappers;

namespace MazeRunner.Gui.Buttons.States;

public abstract class ButtonClickedBaseState : ButtonBaseState
{
    protected double ElapsedGameTimeMs { get; set; }

    protected virtual double UpdateTimeDelayMs
    {
        get
        {
            return 20;
        }
    }

    protected ButtonClickedBaseState(ButtonInfo buttonInfo) : base(buttonInfo)
    {
    }
}