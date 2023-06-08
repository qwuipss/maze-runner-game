using MazeRunner.Gui.Buttons.States;
using System;

namespace MazeRunner.Gui.Buttons;

public class StartButton : Button
{
    public override event Action ButtonPressed;

    public StartButton(float boxScale) : base(boxScale)
    {
    }

    public override void Initialize()
    {
        State = new StartButtonIdleState(this);
    }

    public override void Click()
    {
        ButtonPressed.Invoke();
    }
}
