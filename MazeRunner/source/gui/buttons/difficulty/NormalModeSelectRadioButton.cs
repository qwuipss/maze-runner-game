using MazeRunner.Gui.Buttons.States;
using MazeRunner.Wrappers;
using System;

namespace MazeRunner.Gui.Buttons;

public class NormalModeSelectRadioButton : RadioButton
{
    public override event Action ButtonSelected;

    public NormalModeSelectRadioButton(Action onClick) : base(onClick)
    {
    }

    public override void Initialize(ButtonInfo buttonInfo)
    {
        base.Initialize(buttonInfo);

        State = new NormalModeSelectButtonIdleState(SelfInfo);
    }

    public override void Select()
    {
        ButtonSelected.Invoke();
        IsSelected = true;
    }

    public override void Reset()
    {
        State = new NormalModeSelectButtonResetState(SelfInfo);
    }

    public override void Push()
    {
        Select();

        State = new NormalModeSelectButtonSelectedState(SelfInfo);
    }
}