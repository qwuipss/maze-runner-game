using MazeRunner.Gui.Buttons.States;
using System;

namespace MazeRunner.Gui.Buttons;

public class NormalModeSelectRadioButton : RadioButton
{
    public override event Action ButtonPressed;

    public NormalModeSelectRadioButton(float boxScale) : base(boxScale)
    {
    }

    public override void Initialize()
    {
        State = new NormalModeSelectButtonIdleState(this);
    }

    public override void Click()
    {
        ButtonPressed.Invoke();
        IsSelected = true;
    }

    public override void Reset()
    {
        State = new NormalModeSelectButtonResetState(this);
    }

    public override void Push()
    {
        Click();

        State = new NormalModeSelectButtonSelectedState(this);
    }
}