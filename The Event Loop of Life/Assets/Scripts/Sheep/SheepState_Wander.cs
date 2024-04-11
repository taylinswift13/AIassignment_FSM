using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepState_Wander : State
{
    private float timer = 0.0f;

    private bool isGrassNear = false;
    private bool isWolfNear = false;

    private SheepAgent parent;

    public override void Enter()
    {
        parent = (SheepAgent)base.parent;
        parent.renderer.color = parent.wanderColor;
    }
    public override void Sense()
    {
        SenseIfGrassNear();

        SenseIfWolfNear();
    }

    public override void Decide()
    {
        if (isWolfNear == true)
        {
            parent.SwitchState(new SheepState_EvadeWolf());
        }
        else
        {
            if (isGrassNear)
            {
                parent.SwitchState(new SheepState_FindGrass());
            }
        }
    }
    public override void Act(float deltaTime)
    {
        timer += deltaTime;

        if (timer >= parent.moveFrequncy && parent.ableToMove)
        {
            parent.renderer.color = new Color(1, 1, 1, 1);

            tileManager.RandomMoveOneTile(parent.gameObject, parent.transform.position, parent.obstaclesList);
            timer = 0.0f;
        }
    }
    private void SenseIfGrassNear()
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
                }
            }
        }
    }

    private void SenseIfWolfNear()//Radius
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
}
