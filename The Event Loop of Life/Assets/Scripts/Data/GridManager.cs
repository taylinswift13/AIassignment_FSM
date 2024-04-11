using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager
{
    public List<Tile> tiles;

    public Vector2 size;

    public Vector2 position;

    public GridManager(Vector2 size, Vector2 position)//Grid Generator
    {
        this.size = size;
        this.position = position;

        tiles = new List<Tile>();

        for (int i = 0; i < this.size.y; i++)
        {
            for (int j = 0; j < this.size.x; j++)
            {
                Tile tile = new Tile(new Vector2(this.position.x + j, this.position.y + i));

                tile.parent = this;

                tiles.Add(tile);
            }
        }
    }
}
