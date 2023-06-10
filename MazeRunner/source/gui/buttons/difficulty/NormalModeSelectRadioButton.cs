using MazeRunner.Gui.Buttons.States;
using System;

namespace MazeRunner.Gui.Buttons;

public class NormalModeSelectRadioButton : RadioButton
{
    public override event Action ButtonPressedNotify;

    public NormalModeSelectRadioButton(float boxScale) : base(boxScale)
    {
    }

    public override void Initialize()
    {
        State = new NormalModeSelectButtonIdleState(this);
    }

    public override void Click(bool notifyAboutPush = true)
    {
        if (notifyAboutPush)
        {
            ButtonPressedNotify.Invoke();
        }

        IsSelected = true;
    }

    public override void Reset()
    {
        State = new NormalModeSelectButtonResetState(this);
    }

    public override void Push(bool notifyAboutPush = false)
    {
        Click(notifyAboutPush);

        State = new NormalModeSelectButtonSelectedState(this, notifyAboutPush);
    }
}