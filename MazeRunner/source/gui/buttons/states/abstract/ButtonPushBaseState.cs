namespace MazeRunner.Gui.Buttons.States;

public abstract class ButtonPushBaseState : ButtonBaseState
{
    protected ButtonPushBaseState(Button button) : base(button)
    {
    }

    protected double ElapsedGameTimeMs { get; set; }

    protected virtual double UpdateTimeDelayMs => 20;
}