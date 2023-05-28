using MazeRunner.Components;
using MazeRunner.Wrappers;
using Microsoft.Xna.Framework;

namespace MazeRunner.Gui.Buttons;

public class RadioButtonContainer : MazeRunnerGameComponent
{
    protected ButtonInfo[] ButtonsInfo { get; init; }

    public RadioButtonContainer(params ButtonInfo[] buttonsInfo)
    {
        ButtonsInfo = buttonsInfo;

        foreach (var buttonInfo in ButtonsInfo)
        {
            var radionButton = (RadioButton)buttonInfo.Button;
            radionButton.ButtonSelected += ResetButtons;
        }
    }

    public override void Draw(GameTime gameTime)
    {
        foreach (var buttonInfo in ButtonsInfo)
        {
            buttonInfo.Draw(gameTime);
        }
    }

    public override void Update(GameTime gameTime)
    {
        foreach (var buttonInfo in ButtonsInfo)
        {
            buttonInfo.Update(gameTime);
        }
    }

    protected void ResetButtons()
    {
        foreach (var buttonInfo in ButtonsInfo)
        {
            var radioButton = (RadioButton)buttonInfo.Button;

            if (radioButton.IsSelected)
            {
                radioButton.Reset();
            }
        }
    }
}
