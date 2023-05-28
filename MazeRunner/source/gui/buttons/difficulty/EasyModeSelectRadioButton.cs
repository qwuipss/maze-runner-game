using MazeRunner.Gui.Buttons.States;
using MazeRunner.Wrappers;
using System;

namespace MazeRunner.Gui.Buttons;

public class EasyModeSelectRadioButton : RadioButton
{
    public override event Action ButtonSelected;

    public EasyModeSelectRadioButton(Action onClick) : base(onClick)
    {
    }

    public override void Initialize(ButtonInfo buttonInfo)
    {
        base.Initialize(buttonInfo);

        State = new EasyModeSelectButtonIdleState(SelfInfo);
    }

    public override void Select()
    {
        ButtonSelected.Invoke();
        IsSelected = true;
    }

    public override void Reset()
    {
        State = new EasyModeSelectButtonResetState(SelfInfo);
    }

    public override void Push()
    {
        Select();

        State = new EasyModeSelectButtonSelectedState(SelfInfo);
    }
}
