using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfState_FindSheep : State
{
    private float AT = 0.0f;

    private bool isSheepNear = true;
    private bool isStandingOnSheep = false;

    private WolfAgent parent;
    private SheepAgent sheep;

    private Vector2 sheepPos;

    public override void Sense()
    {
        SenseSheepPos();

        SenseIsStandingOnSheep();
    }

    public override void Decide()
    {
        if (isSheepNear == false)
        {
            parent.SwitchState(new WolfState_Wander());
        }

        if (isStandingOnSheep == true)
        {
            sheep.ableToMove = false;
            parent.SwitchState(new WolfState_EatSheep());
        }
    }

    public override void Act(float deltaTime)
    {
        AT += deltaTime;
        if (AT >= parent.moveFrequncy)
        {
            tileManager.MoveToTile(parent.gameObject, sheepPos, 1, parent.obstaclesList);
            AT = 0.0f;
        }
    }

    public override void Enter()
    {
        parent = (WolfAgent)base.parent;
        parent.renderer.color = parent.findingColor;
        base.Enter();
    }
    private void SenseSheepPos()
    {
        List<Tile> tiles = tileManager.GetTilesInRadius(parent.gameObject, parent.senseSheepRadius);
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
                    sheepPos = entity.transform.position;
                }
            }
        }
    }

    private void SenseIsStandingOnSheep()
    {
        isStandingOnSheep = false;

        List<Entity> entities = tileManager.GetObjectsInTile(tileManager.GetTile(parent.gameObject));
        int wolfNumber = 0;

        foreach (Entity entity in entities)
        {
            if (entity.Type == "Wolf")
            {
                wolfNumber++;
            }
        }

        foreach (Entity entity in entities)
        {
            if (entity.Type == "Sheep" && wolfNumber <= 1)
            {
                if (tileManager.GetAxis(parent.gameObject) == tileManager.GetAxis(entity.gameObject))
                {
                    sheep = (SheepAgent)entity;
                    isStandingOnSheep = true;
                }
            }
        }
    }
}
