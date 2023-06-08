using MazeRunner.Gui.Buttons.States;
using System;

namespace MazeRunner.Gui.Buttons;

public class QuitButton : Button
{
    public override event Action ButtonPressed;

    public QuitButton(float boxScale) : base(boxScale)
    {
    }

    public override void Initialize()
    {
        State = new QuitButtonIdleState(this);
    }

    public override void Click()
    {
        ButtonPressed.Invoke();
    }
}
