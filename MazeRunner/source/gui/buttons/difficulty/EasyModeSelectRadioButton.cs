using MazeRunner.Gui.Buttons.States;
using MazeRunner.Wrappers;
using System;

namespace MazeRunner.Gui.Buttons;

public class EasyModeSelectRadioButton : RadioButton
{
    public override event Action ButtonSelected;

    public EasyModeSelectRadioButton(Action onClick, float boxScale) : base(onClick, boxScale)
    {
    }

    public override void Initialize()
    {
        State = new EasyModeSelectButtonIdleState(this);
    }

    public override void Select()
    {
        ButtonSelected.Invoke();
        IsSelected = true;
    }

    public override void Reset()
    {
        State = new EasyModeSelectButtonResetState(this);
    }

    public override void Push()
    {
        Select();

        State = new EasyModeSelectButtonSelectedState(this);
    }
}
