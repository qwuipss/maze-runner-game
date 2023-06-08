using MazeRunner.Gui.Buttons.States;
using System;

namespace MazeRunner.Gui.Buttons;

public class ResumeButton : Button
{
    public override event Action ButtonPressed;

    public ResumeButton(float boxScale) : base(boxScale)
    {
    }

    public override void Initialize()
    {
        State = new ResumeButtonIdleState(this);
    }

    public override void Click()
    {
        ButtonPressed.Invoke();
    }
}
