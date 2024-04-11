using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepState_EatGrass : State
{
    private bool isStandingOnGrass = false;
    private bool isWolfNear = false;

    private SheepAgent parent;
    private GrassAgent grass;

    private GameObject sheepPrefab;
    private List<Vector2> targetPositionsList;

    public override void Sense()
    {
        SenseIsStandingOnGrass();
        SenseWolf();
    }

    public override void Decide()
    {
        if (isWolfNear == true)
        {
            parent.SwitchState(new SheepState_EvadeWolf());
        }

        if (isStandingOnGrass == false)
        {
            parent.SwitchState(new SheepState_Wander());
        }
    }

    public override void Act(float deltaTime)
    {
        if (grass != null)
        {
            grass.DoScaleChange(-parent.eatingSpeed * deltaTime);
            parent.DoScaleChange(parent.eatingSpeed * deltaTime);

        }
        if (parent.currentHP >= parent.treshhold)
        {
            if (GetTargetTilePositions() == true)
            {
                parent.currentHP -= parent.treshhold / 1.2f;
                GiveBirthToNewSheep();
            }
        }
    }

    public override void Enter()
    {
        parent = (SheepAgent)base.parent;
        parent.renderer.color = parent.eatingColor;
        sheepPrefab = Resources.Load<GameObject>("Prefabs/Sheep");

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
    private void SenseIsStandingOnGrass()
    {
        isStandingOnGrass = false;

        List<Entity> entities = tileManager.GetObjectsInTile(tileManager.GetTile(parent.gameObject));

        foreach (Entity entity in entities)
        {
            if (entity.Type == "Grass")
            {
                if (tileManager.GetAxis(parent.gameObject) == tileManager.GetAxis(entity.gameObject))
                {
                    isStandingOnGrass = true;
                    grass = (GrassAgent)entity;
                }
            }
        }
    }

    private bool GetTargetTilePositions()
    {
        Vector2 axis = tileManager.GetAxis(parent.gameObject);
        List<Tile> adjecentTiles = new List<Tile>();
        adjecentTiles = tileManager.GetAdjecentTiles(axis);
        targetPositionsList = new List<Vector2>();
        foreach (Tile tile in adjecentTiles)
        {
            if (tileManager.GetObjectsInTile(tile).Count == 0)
            {
                targetPositionsList.Add(tile.position);
            }
        }

        if (targetPositionsList.Count != 0)
        {
            return true;
        }
        return false;
    }

    private void GiveBirthToNewSheep()
    {
        int index = Random.Range(0, targetPositionsList.Count);

        tileManager.AddObject(sheepPrefab, targetPositionsList[index] + Vector2.one);
    }
}
