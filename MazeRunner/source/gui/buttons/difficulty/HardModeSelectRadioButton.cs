using MazeRunner.Gui.Buttons.States;
using MazeRunner.Wrappers;
using System;

namespace MazeRunner.Gui.Buttons;

public class HardModeSelectRadioButton : RadioButton
{
    public override event Action ButtonSelected;

    public HardModeSelectRadioButton(Action onClick, float boxScale) : base(onClick, boxScale)
    {
    }

    public override void Initialize()
    {
        State = new HardModeSelectButtonIdleState(this);
    }

    public override void Select()
    {
        ButtonSelected.Invoke();
        IsSelected = true;
    }

    public override void Reset()
    {
        State = new HardModeSelectButtonResetState(this);
    }

    public override void Push()
    {
        Select();

        State = new HardModeSelectButtonSelectedState(this);
    }
}