using MazeRunner.Gui.Buttons.States;
using System;

namespace MazeRunner.Gui.Buttons;

public class EasyModeSelectRadioButton : RadioButton
{
    public override event Action ButtonPressed;

    public EasyModeSelectRadioButton(float boxScale) : base(boxScale)
    {
    }

    public override void Initialize()
    {
        State = new EasyModeSelectButtonIdleState(this);
    }

    public override void Click()
    {
        ButtonPressed.Invoke();
        IsSelected = true;
    }

    public override void Reset()
    {
        State = new EasyModeSelectButtonResetState(this);
    }

    public override void Push()
    {
        Click();

        State = new EasyModeSelectButtonSelectedState(this);
    }
}
