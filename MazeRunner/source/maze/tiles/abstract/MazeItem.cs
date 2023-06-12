using System;

namespace MazeRunner.MazeBase.Tiles;

public abstract class MazeItem : MazeTile
{
    public static event Action StaticItemCollectedNotify;

    public abstract event Action ItemCollectedNotify;

    public abstract ItemType ItemType { get; }

    public override float DrawingPriority => .8f;

    public override TileType TileType => TileType.Item;

    public abstract void Collect();
}