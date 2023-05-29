using MazeRunner.Gui.Buttons.States;
using MazeRunner.Wrappers;
using System;

namespace MazeRunner.Gui.Buttons;

public class QuitButton : Button
{
    public QuitButton(Action onClick) : base(onClick)
    {
    }

    public override void Initialize(ButtonInfo buttonInfo)
    {
        base.Initialize(buttonInfo);

        State = new QuitButtonIdleState(buttonInfo);
    }
}
