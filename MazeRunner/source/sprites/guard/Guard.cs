using MazeRunner.Helpers;
using MazeRunner.Sprites.States;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeRunner.Sprites;

public class Guard : Sprite
{
    public Guard()
    {
        State = new GuardIdleState(null);
    }

    public override Vector2 Speed
    {
        get
        {
            return new Vector2(30, 30);
        }
    }

    public override RectangleF GetHitBox(Vector2 position)
    {
        return HitBoxHelper.GetHitBox(position, 16, 16); //
    }
}
