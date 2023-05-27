using MazeRunner.Components;
using MazeRunner.Drawing;
using MazeRunner.Gui.Buttons;
using MazeRunner.Gui.Buttons.States;
using Microsoft.Xna.Framework;

namespace MazeRunner.Wrappers;

public class ButtonInfo : MazeRunnerGameComponent
{
    public Button Button { get; init; }

    public Vector2 Position { get; set; }

    public float BoxScale { get; init; }

    public ButtonStateInfo IdleStateInfo { get; init; }

    public ButtonStateInfo HoverStateInfo { get; init; }

    public ButtonStateInfo OnClickStateInfo { get; init; }

    public ButtonInfo(Button button, float boxScale, ButtonStateInfo idleStateInfo, ButtonStateInfo hoverStateInfo, ButtonStateInfo onClickStateInfo)
    {
        Button = button;
        BoxScale = boxScale;

        IdleStateInfo = idleStateInfo;
        HoverStateInfo = hoverStateInfo;
        OnClickStateInfo = onClickStateInfo;
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
