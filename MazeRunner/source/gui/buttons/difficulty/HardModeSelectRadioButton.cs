using MazeRunner.Gui.Buttons.States;
using System;

namespace MazeRunner.Gui.Buttons;

public class HardModeSelectRadioButton : RadioButton
{
    public override event Action ButtonPressed;

    public HardModeSelectRadioButton(float boxScale) : base(boxScale)
    {
    }

    public override void Initialize()
    {
        State = new HardModeSelectButtonIdleState(this);
    }

    public override void Click()
    {
        ButtonPressed.Invoke();
        IsSelected = true;
    }

    public override void Reset()
    {
        State = new HardModeSelectButtonResetState(this);
    }

    public override void Push()
    {
        Click();

        State = new HardModeSelectButtonSelectedState(this);
    }
}