using System;

namespace MazeRunner.Gui.Buttons;

public abstract class RadioButton : Button
{
    public bool IsSelected { get; set; }

    protected RadioButton(float boxScale) : base(boxScale)
    {
    }

    public abstract void Reset();

    public abstract void Push();
}
