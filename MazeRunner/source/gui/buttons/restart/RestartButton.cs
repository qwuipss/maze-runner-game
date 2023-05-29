using MazeRunner.Gui.Buttons.States;
using MazeRunner.Wrappers;
using System;

namespace MazeRunner.Gui.Buttons;

public class RestartButton : Button
{
    public RestartButton(Action onClick) : base(onClick)
    {
    }

    public override void Initialize(ButtonInfo buttonInfo)
    {
        base.Initialize(buttonInfo);

        State = new RestartButtonIdleState(buttonInfo);
    }
}
