using MazeRunner.MazeBase.Tiles.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MazeRunner.MazeBase.Tiles;

public abstract class MazeTile
{
    public abstract TileType TileType { get; }

    public virtual float FrameRotationAngle { get; set; }

    public virtual Vector2 OriginFrameRotationVector { get; set; }

    public virtual float DrawingPriority
    {
        get
        {
            return 1;
        }
    }

    public virtual Texture2D Texture
    {
        get
        {
            return State.Texture;
        }
    }

    public virtual int FrameSize
    {
        get
        {
            return State.FrameSize;
        }
    }

    public virtual Point CurrentAnimationFramePoint
    {
        get
        {
            return State.CurrentAnimationFramePoint;
        }
    }

    protected virtual IMazeTileState State { get; set; }

    public virtual void Update(GameTime gameTime)
    {
        State = State.ProcessState(gameTime);
    }
}