using MazeRunner.Gui.Buttons.States;
using System;

namespace MazeRunner.Gui.Buttons;

public class RestartButton : Button
{
    public override event Action ButtonPressedNotify;

    public RestartButton(float boxScale, Func<bool> canBeClicked) : base(boxScale, canBeClicked)
    {
    }

    public override void Initialize()
    {
        State = new RestartButtonIdleState(this);
    }

    public override void Click(bool notifyAboutPush = true)
    {
        if (notifyAboutPush)
        {
            ButtonPressedNotify.Invoke();
        }
    }
}
