using MazeRunner.Gui.Buttons.States;
using System;

namespace MazeRunner.Gui.Buttons;

public class MenuButton : Button
{
    public MenuButton(Action onClick, float boxScale) : base(onClick, boxScale)
    {
    }

    public override void Initialize()
    {
        State = new MenuButtonIdleState(this);
    }
}
