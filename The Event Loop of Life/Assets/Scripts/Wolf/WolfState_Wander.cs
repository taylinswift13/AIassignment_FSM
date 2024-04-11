using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfState_Wander : State
{
    private float AT = 0.0f;

    private bool isSheepNear = false;

    private WolfAgent parent;

    public override void Enter()
    {
        parent = (WolfAgent)base.parent;
        parent.renderer.color = parent.wanderColor;
    }

    public override void Sense()
    {
        SenseIfSheepNear();
    }

    public override void Decide()
    {
        if (isSheepNear)
        {
            parent.SwitchState(new WolfState_FindSheep());
        }
    }

    public override void Act(float deltaTime)
    {
        AT += deltaTime;
        if (AT >= parent.moveFrequncy)
        {
            AT = 0.0f;

            tileManager.RandomMoveOneTile(parent.gameObject, parent.transform.position, parent.obstaclesList);
        }
    }
    private void SenseIfSheepNear()
    {
        List<Tile> tiles = tileManager.GetTilesInRadius(parent.gameObject, parent.senseSheepRadius);
        List<Entity> entites = tileManager.GetObjectsInTiles(tiles);
        isSheepNear = false;

        foreach (Tile tile in tiles)
        {
            List<Entity> entities = tileManager.GetObjectsInTile(tile);

            bool hasWolf = false;

            foreach (Entity entity in entities)
            {
                if (entity.Type == "Wolf")
                {
                    hasWolf = true;
                }
            }

            foreach (Entity entity in entities)
            {
                if (entity.Type == "Sheep" && hasWolf == false)
                {
                    isSheepNear = true;
                }
            }
        }
    }
}
