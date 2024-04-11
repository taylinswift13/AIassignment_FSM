using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    public Vector2 position;

    public List<GameObject> objects;

    public GridManager parent;

    public Tile(Vector2 position)
    {
        this.position = position;

        this.objects = new List<GameObject>();
    }
}
public class TileManager
{
    public List<Tile> tiles;
    public GridManager grid;
    public enum Direction
    {
        UP, DOWN, LEFT, RIGHT
    }
    public TileManager(List<Tile> tiles, GridManager grid)
    {
        this.tiles = tiles;
        this.grid = grid;
    }
    public Tile GetTile(GameObject gameObject)
    {
        foreach (Tile tile in tiles)
        {
            foreach (GameObject Object in tile.objects)
            {
                if (gameObject == Object)
                {
                    return tile;
                }
            }
        }
        return null;
    }

    public Tile GetTile(Vector2 Axis)
    {
        if (Axis.x > grid.size.x ||
            Axis.y > grid.size.y ||
            Axis.x < 1 ||
            Axis.y < 1)
        {
            return null;
        }

        int index = (int)((Axis.y - 1) * grid.size.x + Axis.x);

        if (index > grid.tiles.Count || index < 1)
        {
            return null;
        }

        Tile tile = grid.tiles[index - 1];
        return tile;
    }
    public void RemoveObject(GameObject Object)
    {
        Tile tile = GetTile(Object);

        tile.objects.Remove(Object);

        GameObject.Destroy(Object);
    }
    public void AddObject(GameObject Object, Vector2 Axis)
    {
        AddObject(Object, GetTile(Axis));
    }

    public void AddObject(GameObject Object, Tile tile)
    {
        GameObject @object = GameObject.Instantiate(Object, tile.position, Quaternion.Euler(0, 0, 0));

        @object.transform.SetParent(GameObject.Find("Tilemap").transform);

        tile.objects.Add(@object);
    }

    public bool CheckOccupancyOfTileType(Tile tile, string type)
    {
        List<Entity> entities = GetObjectsInTile(tile);

        foreach (Entity entity in entities)
        {
            if (entity.Type == type)
            {
                return true;
            }
        }

        return false;
    }
    public List<Entity> GetObjectsInTile(Tile tile)
    {
        List<Entity> entities = new List<Entity>();
        if (tile == null)
        {
            return null;
        }
        foreach (GameObject Object in tile.objects)
        {
            Entity entity = Object.GetComponent<Entity>();
            if (entity != null)
            {
                entities.Add(entity);
            }
        }

        return entities;
    }
    public List<Entity> GetObjectsInTiles(List<Tile> tiles)
    {
        List<Entity> entites = new List<Entity>();

        foreach (Tile tile in tiles)
        {
            foreach (GameObject objects in tile.objects)
            {
                Entity entity = objects.GetComponent<Entity>();
                if (entity != null)
                {
                    entites.Add(entity);
                }
            }
        }

        return entites;
    }
    public List<Entity> GetObjectsOfTypeInTile(Tile tile, string type)
    {
        List<Entity> entites = new List<Entity>();
        List<Entity> allObjects = GetObjectsInTile(tile);
        if (allObjects == null)
        {
            return null;
        }
        foreach (Entity @object in allObjects)
        {
            if (@object.Type == type)
            {
                entites.Add(@object);
            }
        }
        return entites;
    }

    public List<Tile> GetAdjecentTiles(Vector2 axis)
    {
        List<Tile> tileList = new List<Tile>();

        Tile Up = GetTile(axis + new Vector2(0, 1));
        Tile Down = GetTile(axis + new Vector2(0, -1));
        Tile Right = GetTile(axis + new Vector2(1, 0));
        Tile Left = GetTile(axis + new Vector2(-1, 0));

        if (Up != null)
        {
            tileList.Add(Up);
        }
        if (Down != null)
        {
            tileList.Add(Down);
        }
        if (Right != null)
        {
            tileList.Add(Right);
        }
        if (Left != null)
        {
            tileList.Add(Left);
        }

        return tileList;
    }
    public Tile GetAdjecentRandomTile(List<Tile> adjecentTileList)
    {
        if (adjecentTileList == null)
        {
            return null;
        }

        int index = Random.Range(0, adjecentTileList.Count);
        Tile targetTile = adjecentTileList[index];


        return targetTile;
    }

    public Vector2 GetAxis(GameObject gameObject)    //row, column
    {
        Tile this_tile = GetTile(gameObject);
        int index = 0;

        foreach (Tile tile in tiles)//get the index of the tile in the list
        {
            index++;
            if (this_tile == tile)
            {
                break;
            }
        }
        int remainder = index % (int)grid.size.x;
        int y = index / (int)grid.size.x + 1;

        if (remainder == 0)
        {
            y--;
        }
        int x = index - (y - 1) * (int)grid.size.x;

        return new Vector2(x, y);
    }
    public List<Tile> GetTilesInRadius(GameObject center, float radius)
    {
        List<Tile> result = new List<Tile>();

        Tile centerTile = GetTile(center);

        foreach (Tile tile in tiles)
        {
            float distance = Vector2.Distance(centerTile.position, tile.position);

            if (distance <= radius && tile != centerTile)
            {
                result.Add(tile);
            }
        }
        return result;
    }
    public void Move(GameObject target, Direction direction, int distance)
    {
        Vector2 axis = GetAxis(target);
        Vector2 targetAxis = Vector2.zero;
        switch (direction)
        {
            case Direction.UP:
                {
                    int x = (int)axis.x;
                    int y = (int)axis.y + distance;
                    if (y > grid.size.y)
                    {
                        y = (int)grid.size.y;
                    }
                    targetAxis = new Vector2(x, y);
                    break;
                }

            case Direction.DOWN:
                {
                    int x = (int)axis.x;
                    int y = (int)axis.y - distance;

                    if (y < 1) { y = 1; }
                    targetAxis = new Vector2(x, y);
                    break;
                }

            case Direction.RIGHT:
                {
                    int x = (int)axis.x + distance;
                    int y = (int)axis.y;

                    if (x > grid.size.x)
                    {
                        x = (int)grid.size.x;
                    }
                    targetAxis = new Vector2(x, y);
                    break;
                }

            case Direction.LEFT:
                {
                    int x = (int)axis.x - distance;
                    int y = (int)axis.y;
                    if (x < 1) { x = 1; }
                    targetAxis = new Vector2(x, y);
                    break;
                }
        }

        Tile targetTile = GetTile(targetAxis);
        MoveObjectsToTile(target, targetTile);
    }
    public void MoveObjectsToTile(GameObject gameObject, Tile targetTile)
    {
        Tile oldTile = GetTile(gameObject);
        oldTile.objects.Remove(gameObject);
        targetTile.objects.Add(gameObject);
        gameObject.transform.position = targetTile.position;
    }
    public void MoveToOneTile(GameObject gameobject, Vector2 targetPosition, List<string> occupationTileList)
    {
        Vector2 position = gameobject.transform.position;
        Vector2 direction = GetNextMoveDirection(position, targetPosition);
        if (MoveAvailabilityCheck(direction + GetAxis(gameobject), occupationTileList) == false)
        {
            return;
        }

        if (direction == new Vector2(1, 0))
        {
            Move(gameobject, Direction.RIGHT, 1);
        }

        if (direction == new Vector2(-1, 0))
        {
            Move(gameobject, Direction.LEFT, 1);
        }

        if (direction == new Vector2(0, 1))
        {
            Move(gameobject, Direction.UP, 1);
        }

        if (direction == new Vector2(0, -1))
        {
            Move(gameobject, Direction.DOWN, 1);
        }
    }
    public void MoveFromOneTile(GameObject gameobject, Vector2 dangerPosition, List<string> occupationTileList)
    {
        Vector2 position = gameobject.transform.position;
        Vector2 direction = GetNextMoveDirection(position, dangerPosition);
        if (MoveAvailabilityCheck(direction + GetAxis(gameobject), occupationTileList) == false)
        {
            return;
        }

        if (direction == new Vector2(1, 0))
        {
            Move(gameobject, Direction.LEFT, 1);
        }

        if (direction == new Vector2(-1, 0))
        {
            Move(gameobject, Direction.RIGHT, 1);
        }

        if (direction == new Vector2(0, 1))
        {
            Move(gameobject, Direction.DOWN, 1);
        }

        if (direction == new Vector2(0, -1))
        {
            Move(gameobject, Direction.UP, 1);
        }
    }
    private Vector2 GetNextMoveDirection(Vector2 position, Vector2 targetPosition)
    {
        Vector2 direction = Vector2.zero;

        int x = (int)(targetPosition.x - position.x);
        int y = (int)(targetPosition.y - position.y);
        if (x != 0 && y != 0)
        {
            int moveX = Random.Range(0, 2);//50% to move X 50% to move Y
            if (moveX == 0)
            {
                if (x > 0)
                {
                    direction = new Vector2(1, 0);
                }
                else
                {
                    direction = new Vector2(-1, 0);
                }
            }
            else
            {
                if (y > 0)
                {
                    direction = new Vector2(0, 1);
                }
                else
                {
                    direction = new Vector2(0, -1);
                }
            }
        }
        if (x == 0 && y != 0)
        {
            if (y > 0)
            {
                direction = new Vector2(0, 1);
            }
            else
            {
                direction = new Vector2(0, -1);
            }
        }
        if (y == 0 && x != 0)
        {
            if (x > 0)
            {
                direction = new Vector2(1, 0);
            }
            else
            {
                direction = new Vector2(-1, 0);
            }
        }
        return direction;
    }
    public void MoveToTile(GameObject gameObject, Vector2 targetPosition, int steps, List<string> occupationTileList)
    {
        for (int i = 0; i < steps; i++)
        {
            MoveToOneTile(gameObject, targetPosition, occupationTileList);
        }
    }
    public void MoveFromTile(GameObject gameObject, Vector2 dangerPosition, int steps, List<string> occupationTileList)
    {
        for (int i = 0; i < steps; i++)
        {
            MoveToOneTile(gameObject, dangerPosition, occupationTileList);
        }
    }
    public void RandomMoveOneTile(GameObject gameobject, Vector2 gameObjectPos, List<string> occupationTileList)
    {
        List<Vector2> targetPositions = new List<Vector2>();
        //UP
        {
            Vector2 tilePos_up = gameObjectPos + new Vector2(0, 1);
            Tile tile_up = GetTile(tilePos_up + Vector2.one);
            if (tile_up != null)
            {
                if (MoveAvailabilityCheck(tilePos_up, occupationTileList))
                {
                    targetPositions.Add(tilePos_up);
                }
            }
        }
        //DOWN
        {
            Vector2 tilePos_down = gameObjectPos + new Vector2(0, -1);
            Tile tile_down = GetTile(tilePos_down + Vector2.one);
            if (tile_down != null)
            {
                if (MoveAvailabilityCheck(tilePos_down, occupationTileList))
                {
                    targetPositions.Add(tilePos_down);
                }
            }
        }
        //LEFT
        {
            Vector2 tilePos_left = gameObjectPos + new Vector2(-1, 0);
            Tile tile_left = GetTile(tilePos_left + Vector2.one);
            if (tile_left != null)
            {
                if (MoveAvailabilityCheck(tilePos_left, occupationTileList))
                {
                    targetPositions.Add(tilePos_left);
                }
            }
        }
        //RIGHT
        {
            Vector2 tilePos_right = gameObjectPos + new Vector2(1, 0);
            Tile tile_right = GetTile(tilePos_right + Vector2.one);
            if (tile_right != null)
            {
                if (MoveAvailabilityCheck(tilePos_right, occupationTileList))
                {
                    targetPositions.Add(tilePos_right);
                }
            }
        }
        int index = Random.Range(0, targetPositions.Count);
        if (targetPositions.Count != 0)
        {
            Vector2 position = targetPositions[index];
            MoveToOneTile(gameobject, position, occupationTileList);
        }
    }
    private bool MoveAvailabilityCheck(Vector2 position, List<string> typeList)
    {
        if (typeList == null)
        {
            return true;
        }
        else
        {
            Tile tile = GetTile(position + Vector2.one);
            foreach (string type in typeList)
            {
                if (tile != null)
                {
                    if (GetObjectsOfTypeInTile(tile, type).Count != 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
