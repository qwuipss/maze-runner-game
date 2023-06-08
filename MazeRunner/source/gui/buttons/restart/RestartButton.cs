using MazeRunner.Gui.Buttons.States;
using System;

namespace MazeRunner.Gui.Buttons;

public class RestartButton : Button
{
    public override event Action ButtonPressed;

    public RestartButton(float boxScale) : base(boxScale)
    {
    }

    public override void Initialize()
    {
        State = new RestartButtonIdleState(this);
    }

    public override void Click()
    {
        ButtonPressed.Invoke();
    }
}
