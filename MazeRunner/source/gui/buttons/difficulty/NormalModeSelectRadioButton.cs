using MazeRunner.Gui.Buttons.States;
using System;

namespace MazeRunner.Gui.Buttons;

public class NormalModeSelectRadioButton : RadioButton
{
    public override event Action ButtonSelected;

    public NormalModeSelectRadioButton(Action onClick, float boxScale) : base(onClick, boxScale)
    {
    }

    public override void Initialize()
    {
        State = new NormalModeSelectButtonIdleState(this);
    }

    public override void Select()
    {
        ButtonSelected.Invoke();
        IsSelected = true;
    }

    public override void Reset()
    {
        State = new NormalModeSelectButtonResetState(this);
    }

    public override void Push()
    {
        Select();

        State = new NormalModeSelectButtonSelectedState(this);
    }
}