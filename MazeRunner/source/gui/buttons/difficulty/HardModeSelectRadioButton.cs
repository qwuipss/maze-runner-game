using MazeRunner.Gui.Buttons.States;
using MazeRunner.Wrappers;
using System;

namespace MazeRunner.Gui.Buttons;

public class HardModeSelectRadioButton : RadioButton
{
    public override event Action ButtonSelected;

    public HardModeSelectRadioButton(Action onClick) : base(onClick)
    {
    }

    public override void Initialize(ButtonInfo buttonInfo)
    {
        base.Initialize(buttonInfo);

        State = new HardModeSelectButtonIdleState(SelfInfo);
    }

    public override void Select()
    {
        ButtonSelected.Invoke();
        IsSelected = true;
    }

    public override void Reset()
    {
        State = new HardModeSelectButtonResetState(SelfInfo);
    }

    public override void Push()
    {
        Select();

        State = new HardModeSelectButtonSelectedState(SelfInfo);
    }
}