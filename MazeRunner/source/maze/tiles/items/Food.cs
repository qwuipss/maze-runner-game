using MazeRunner.Helpers;
using MazeRunner.MazeBase.Tiles.States;
using MazeRunner.Sprites;
using Microsoft.Xna.Framework;
using System;
using System.Drawing;

namespace MazeRunner.MazeBase.Tiles;

public class Food : MazeItem
{
    private const float HitBoxOffset = 4;

    private const float HitBoxSize = 8;

    private readonly Hero _hero;

    public override event Action ItemCollectedNotify;

    public Food(Hero hero)
    {
        _hero = hero;

        State = new FoodIdleState();
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
            return ItemType.Food;
        }
    }

    public override RectangleF GetHitBox(Vector2 position)
    {
        return HitBoxHelper.GetHitBox(position, HitBoxOffset, HitBoxOffset, HitBoxSize, HitBoxSize);
    }

    public override void Collect()
    {
        _hero.Health++;

        ItemCollectedNotify.Invoke();
    }
}
