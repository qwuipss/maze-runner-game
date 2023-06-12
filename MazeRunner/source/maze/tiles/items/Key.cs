using MazeRunner.Helpers;
using MazeRunner.MazeBase.Tiles.States;
using Microsoft.Xna.Framework;
using System;
using System.Drawing;

namespace MazeRunner.MazeBase.Tiles;

public class Key : MazeItem
{
    private const float HitBoxOffset = 4;

    private const float HitBoxSize = 8;

    public new static event Action ItemCollectedStaticNotify;

    public override event Action ItemCollectedNotify;

    public Key()
    {
        State = new KeyIdleState();

        ItemCollectedNotify += ItemCollectedStaticNotify.Invoke;
    }

    public override TileType TileType
    {
        get
        {
            return TileType.Item;
        }
    }

    public override ItemType ItemType
    {
        get
        {
            return ItemType.Key;
        }
    }

    public override RectangleF GetHitBox(Vector2 position)
    {
        return HitBoxHelper.GetHitBox(position, HitBoxOffset, HitBoxOffset, HitBoxSize, HitBoxSize);
    }

    public override void Collect()
    {
        ItemCollectedNotify.Invoke();
    }
}
