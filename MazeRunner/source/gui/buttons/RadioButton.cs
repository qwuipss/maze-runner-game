using System;

namespace MazeRunner.Gui.Buttons;

public abstract class RadioButton : Button
{
    public static new event Action StaticButtonPressedNotify;

    public bool IsSelected { get; set; }

    protected RadioButton(float boxScale) : base(boxScale, false)
    {
        ButtonPressedNotify += StaticButtonPressedNotify.Invoke;
    }

    public abstract void Reset();

    public abstract void Push(bool notifyAboutPush = true);
}
