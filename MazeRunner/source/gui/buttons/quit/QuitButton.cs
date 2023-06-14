using MazeRunner.Gui.Buttons.States;
using System;

namespace MazeRunner.Gui.Buttons;

public class QuitButton : Button
{
    public override event Action ButtonPressedNotify;

    public QuitButton(float boxScale, Func<bool> canBeClicked) : base(boxScale, canBeClicked)
    {
    }

    public override void Initialize()
    {
        State = new QuitButtonIdleState(this);
    }

    public override void Click(bool notifyAboutPush = true)
    {
        if (notifyAboutPush)
        {
            ButtonPressedNotify.Invoke();
        }
    }
}
