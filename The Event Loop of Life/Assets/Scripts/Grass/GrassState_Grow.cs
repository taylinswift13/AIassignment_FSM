using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassState_Grow : State
{
    private bool trampled = false;
    private bool mature = false;

    private GrassAgent parent;
    private GameObject newGrassPrefab;

    public override void Enter()
    {
        parent = (GrassAgent)base.parent;//Cast the parent in Entity into Grass

        senseFrequency = 0.1f;

        newGrassPrefab = Resources.Load<GameObject>("Prefabs/Grass");

        base.Enter();
    }

    public override void Sense()
    {
        SenseTrampled();

        SenseMature();
    }

    public override void Decide()
    {
        if (mature == true)
        {
            SpreadGrass();
            parent.SwitchState(new GrassState_StayMature());
        }
    }

    public override void Act(float deltaTime)
    {
        if (trampled == true)
        {
            return;
        }

        parent.DoScaleChange(parent.growSpeed * deltaTime);
        CalculateTransparency();
    }
    public void CalculateTransparency()
    {
        float alpha = parent.currentHP / parent.maxHP;
        parent.renderer.color = new Color(1, 1, 1, alpha);
    }
    private void SenseTrampled()
    {
        trampled = true;

        List<Entity> sheeps = tileManager.GetObjectsOfTypeInTile(tileManager.GetTile(parent.gameObject), "Sheep");
        List<Entity> wolfs = tileManager.GetObjectsOfTypeInTile(tileManager.GetTile(parent.gameObject), "Wolf");

        if (sheeps.Count == 0 && wolfs.Count == 0)
        {
            trampled = false;
        }
    }

    private void SenseMature()
    {
        if (parent.currentHP >= parent.maxHP)
        {
            mature = true;
        }
    }

    private void SpreadGrass()
    {
        List<Tile> adjecentTiles = tileManager.GetAdjecentTiles(tileManager.GetAxis(parent.gameObject));
        Tile tile = tileManager.GetAdjecentRandomTile(adjecentTiles);


        bool hasGrass = tileManager.CheckOccupancyOfTileType(tile, "Grass");

        if (hasGrass == false)
        {
            tileManager.AddObject(newGrassPrefab, tile);
        }
    }

}
