using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepState_FindGrass : State
{
    private float AT = 0.0f;

    private bool isWolfNear = false;
    private bool isGrassNear = true;
    private bool isStandingOnGrass = false;

    private SheepAgent parent;
    private Vector2 grassPos;

    public override void Sense()
    {
        SenseGrassPosition();

        SenseIsStandingOnGrass();

        SenseWolf();
    }

    public override void Decide()
    {
        if (isWolfNear == true)
        {
            parent.SwitchState(new SheepState_EvadeWolf());
        }

        if (isGrassNear == false)
        {
            parent.SwitchState(new SheepState_Wander());
        }

        if (isStandingOnGrass == true)
        {
            parent.SwitchState(new SheepState_EatGrass());
        }
    }

    public override void Act(float deltaTime)
    {
        AT += deltaTime;
        if (AT >= parent.moveFrequncy && parent.ableToMove)
        {
            tileManager.MoveToTile(parent.gameObject, grassPos, 1, parent.obstaclesList);
            AT = 0.0f;
        }
    }

    public override void Enter()
    {
        parent = (SheepAgent)base.parent;
        parent.renderer.color = parent.findingColor;
        base.Enter();
    }
    private void SenseWolf()//Radius
    {
        List<Tile> tiles = tileManager.GetTilesInRadius(parent.gameObject, parent.senseWolfRadius);
        List<Entity> entites = tileManager.GetObjectsInTiles(tiles);

        isWolfNear = false;

        foreach (Entity entity in entites)
        {
            if (entity.Type == "Wolf")
            {
                isWolfNear = true;
            }
        }
    }

    private void SenseGrassPosition()//UP DOWN LEFT RIGHT
    {
        List<Tile> tiles = tileManager.GetAdjecentTiles(tileManager.GetAxis(parent.gameObject));

        isGrassNear = false;

        foreach (Tile tile in tiles)
        {
            List<Entity> entities = tileManager.GetObjectsInTile(tile);

            bool hasSheep = false;

            foreach (Entity entity in entities)//If the target is being eaten then skip
            {
                if (entity.Type == "Sheep")
                {
                    hasSheep = true;
                }
            }

            foreach (Entity entity in entities)
            {
                if (entity.Type == "Grass" && hasSheep == false)
                {
                    isGrassNear = true;
                    grassPos = entity.transform.position;
                }
            }
        }
    }

    private void SenseIsStandingOnGrass()
    {
        isStandingOnGrass = false;

        List<Entity> entities = tileManager.GetObjectsInTile(tileManager.GetTile(parent.gameObject));

        int sheepNum = 0;

        foreach (Entity entity in entities)
        {
            if (entity.Type == "Sheep")
            {
                sheepNum++;
            }
        }
        foreach (Entity entity in entities)
        {
            if (entity.Type == "Grass" && sheepNum <= 1)
            {
                if (tileManager.GetAxis(parent.gameObject) == tileManager.GetAxis(entity.gameObject))
                {
                    isStandingOnGrass = true;
                }
            }
        }
    }
}
