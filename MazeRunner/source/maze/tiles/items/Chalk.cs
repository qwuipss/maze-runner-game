using MazeRunner.Helpers;
using MazeRunner.MazeBase.Tiles.States;
using MazeRunner.Sprites;
using Microsoft.Xna.Framework;
using System;
using System.Drawing;

namespace MazeRunner.MazeBase.Tiles;

public class Chalk : MazeItem
{
    private const float HitBoxOffset = 4;

    private const float HitBoxSize = 8;

    private readonly Hero _hero;

    public new static event Action ItemCollectedStaticNotify;

    public override event Action ItemCollectedNotify;

    public Chalk(Hero hero)
    {
        _hero = hero;

        State = new ChalkIdleState();

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
            return ItemType.Chalk;
        }
    }

    public override RectangleF GetHitBox(Vector2 position)
    {
        return HitBoxHelper.GetHitBox(position, HitBoxOffset, HitBoxOffset, HitBoxSize, HitBoxSize);
    }

    public override void Collect()
    {
        _hero.ChalkUses += RandomHelper.Next(1, 4);

        ItemCollectedNotify.Invoke();
    }
}
