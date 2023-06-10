using MazeRunner.Components;
using Microsoft.Xna.Framework;

namespace MazeRunner.Gui.Buttons;

public class RadioButtonContainer : MazeRunnerGameComponent
{
    protected RadioButton[] Buttons { get; init; }

    public RadioButtonContainer(params RadioButton[] buttons)
    {
        Buttons = buttons;

        foreach (var button in Buttons)
        {
            button.ButtonPressedNotify += ResetButtons;
        }
    }

    public override void Draw(GameTime gameTime)
    {
        foreach (var button in Buttons)
        {
            button.Draw(gameTime);
        }
    }

    public override void Update(GameTime gameTime)
    {
        foreach (var button in Buttons)
        {
            button.Update(gameTime);
        }
    }

    protected void ResetButtons()
    {
        foreach (var button in Buttons)
        {
            if (button.IsSelected)
            {
                button.Reset();
            }
        }
    }
}
