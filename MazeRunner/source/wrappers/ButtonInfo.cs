using MazeRunner.Components;
using MazeRunner.Drawing;
using MazeRunner.Gui.Buttons;
using Microsoft.Xna.Framework;

namespace MazeRunner.Wrappers;

public class ButtonInfo : MazeRunnerGameComponent
{
    public Button Button { get; init; }

    public float BoxScale { get; init; }

    public Vector2 Position { get; set; }

    public ButtonInfo(Button button, float boxScale)
    {
        Button = button;
        BoxScale = boxScale;
    }

    public override void Draw(GameTime gameTime)
    {
        Drawer.DrawButton(Button, Position, BoxScale);
    }

    public override void Update(GameTime gameTime)
    {
        Button.Update(gameTime);
    }
}
