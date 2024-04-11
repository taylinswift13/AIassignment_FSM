using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfState_EatSheep : State
{
    private bool isStandingOnSheep = false;

    private WolfAgent parent;
    private SheepAgent sheep;

    private GameObject wolfPrefab;
    private List<Vector2> targetPositionsList;

    public override void Sense()
    {
        SenseIsStandingOnSheep();
    }

    public override void Decide()
    {
        if (isStandingOnSheep == false)
        {
            parent.SwitchState(new WolfState_Wander());
        }
    }

    public override void Act(float deltaTime)
    {
        if (sheep != null)
        {
            sheep.DoScaleChange(-parent.eatingSpeed * deltaTime);
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
        parent = (WolfAgent)base.parent;
        parent.renderer.color = parent.eatingColor;
        wolfPrefab = Resources.Load<GameObject>("Prefabs/Wolf");

        base.Enter();
    }
    private void SenseIsStandingOnSheep()
    {
        isStandingOnSheep = false;

        List<Entity> entities = tileManager.GetObjectsInTile(tileManager.GetTile(parent.gameObject));

        foreach (Entity entity in entities)
        {
            if (entity.Type == "Sheep")
            {
                if (tileManager.GetAxis(parent.gameObject) == tileManager.GetAxis(entity.gameObject))
                {
                    isStandingOnSheep = true;
                    sheep = (SheepAgent)entity;
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

        tileManager.AddObject(wolfPrefab, targetPositionsList[index] + Vector2.one);
    }
}
