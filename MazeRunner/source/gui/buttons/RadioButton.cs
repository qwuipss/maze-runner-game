using System;

namespace MazeRunner.Gui.Buttons;

public abstract class RadioButton : Button
{
    public abstract event Action ButtonSelected;

    public bool IsSelected { get; set; }

    protected RadioButton(Action onClick) : base(onClick)
    {
    }

    public abstract void Select();

    public abstract void Reset();

    public abstract void Push();
}
