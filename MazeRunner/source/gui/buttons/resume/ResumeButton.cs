using MazeRunner.Gui.Buttons.States;
using System;

namespace MazeRunner.Gui.Buttons;

public class ResumeButton : Button
{
    public ResumeButton(Action onClick, float boxScale) : base(onClick, boxScale)
    {
    }

    public override void Initialize()
    {
        State = new ResumeButtonIdleState(this);
    }
}
