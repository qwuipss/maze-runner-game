using MazeRunner.Gui.Buttons.States;
using MazeRunner.Wrappers;
using System;

namespace MazeRunner.Gui.Buttons;

public class RestartButton : Button
{
    public RestartButton(Action onClick, float boxScale) : base(onClick, boxScale)
    {
    }

    public override void Initialize()
    {
        State = new RestartButtonIdleState(this);
    }
}
