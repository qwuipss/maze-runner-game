using MazeRunner.Gui.Buttons.States;
using System;

namespace MazeRunner.Gui.Buttons;

public class MenuButton : Button
{
    public override event Action ButtonPressed;

    public MenuButton(float boxScale) : base(boxScale)
    {
    }

    public override void Initialize()
    {
        State = new MenuButtonIdleState(this);
    }

    public override void Click()
    {
        ButtonPressed.Invoke();
    }
}
