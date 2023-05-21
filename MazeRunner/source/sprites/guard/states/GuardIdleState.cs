using MazeRunner.Content;
using MazeRunner.Helpers;
using MazeRunner.MazeBase.Tiles;
using MazeRunner.Wrappers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeRunner.Sprites.States;

public class GuardIdleState : GuardBaseState
{
    public GuardIdleState(ISpriteState previousState) : base(previousState)
    {
    }

    public override Texture2D Texture
    {
        get
        {
            return Textures.Sprites.Guard.Idle;
        }
    }

    public override int FramesCount
    {
        get
        {
            return 4;
        }
    }

    public override double UpdateTimeDelayMs
    {
        get
        {
            return 650;
        }
    }

    public override ISpriteState ProcessState(GameTime gameTime)
    {
        ElapsedGameTimeMs += gameTime.ElapsedGameTime.TotalMilliseconds;

        if (ElapsedGameTimeMs > UpdateTimeDelayMs)
        {
            var animationPoint = CurrentAnimationFramePoint;
            var framePosX = (animationPoint.X + FrameSize) % (FrameSize * FramesCount);

            if (animationPoint.X == (FramesCount - 1) * FrameSize && framePosX is 0 && RandomHelper.RandomBoolean())
            {
                return ;
            }

            CurrentAnimationFramePoint = new Point(framePosX, 0);

            ElapsedGameTimeMs -= UpdateTimeDelayMs;
        }

        return this;
    }

    private static bool IsHeroNearby(SpriteInfo heroInfo)
    {
        var distance = Vector2.Distance(heroInfo.Position, );
    }
}
