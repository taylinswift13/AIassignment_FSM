using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepState_EvadeWolf : State
{
    private float AT = 0.0f;

    private bool isWolfNear = false;

    private SheepAgent parent;

    private Vector2 targetPos;

    public override void Sense()
    {
        SenseWolfPosition();
    }

    public override void Decide()
    {
        if (isWolfNear == false)
        {
            parent.SwitchState(new SheepState_Wander());
        }
    }

    public override void Act(float deltaTime)
    {
        AT += deltaTime;
        if (AT >= parent.moveFrequncy && parent.ableToMove && isWolfNear)
        {
            tileManager.MoveFromTile(parent.gameObject, targetPos, 1, parent.obstaclesList);
            AT = 0.0f;
        }
    }

    public override void Enter()
    {
        parent = (SheepAgent)base.parent;
        parent.renderer.color = parent.runningColor;
        base.Enter();
    }
    private void SenseWolfPosition()
    {
        List<Tile> tiles = tileManager.GetTilesInRadius(parent.gameObject, parent.senseWolfRadius);
        List<Entity> entites = tileManager.GetObjectsInTiles(tiles);
        List<Vector2> wolvesToSheepDistance = new List<Vector2>();
        isWolfNear = false;

        foreach (Entity entity in entites)
        {
            if (entity.Type == "Wolf")
            {
                wolvesToSheepDistance.Add(entity.transform.position - parent.transform.position);
            }
        }
        Vector2 allVectors = Vector2.zero;
        if (wolvesToSheepDistance.Count != 0)
        {
            isWolfNear = true;
            for (int i = 0; i < wolvesToSheepDistance.Count; i++)
            {
                allVectors += wolvesToSheepDistance[i];
            }
            targetPos = new Vector2(parent.transform.position.x, parent.transform.position.y) - allVectors.normalized;
        }
    }
}
