using MazeRunner.Gui.Buttons.States;
using System;

namespace MazeRunner.Gui.Buttons;

public class ResumeButton : Button
{
    public override event Action ButtonPressedNotify;

    public ResumeButton(float boxScale) : base(boxScale)
    {
    }

    public override void Initialize()
    {
        State = new ResumeButtonIdleState(this);
    }

    public override void Click(bool notifyAboutPush = true)
    {
        if (notifyAboutPush)
        {
            ButtonPressedNotify.Invoke();
        }
    }
}
