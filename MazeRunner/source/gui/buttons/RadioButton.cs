using System;

namespace MazeRunner.Gui.Buttons;

public abstract class RadioButton : Button
{
    public bool IsSelected { get; set; }

    protected RadioButton(float boxScale, Func<bool> canBeClicked) : base(boxScale, canBeClicked)
    {
    }

    public abstract void Reset();

    public abstract void Push(bool notifyAboutPush = true);
}
