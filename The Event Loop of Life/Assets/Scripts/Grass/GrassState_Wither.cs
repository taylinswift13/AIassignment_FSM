using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassState_Wither : State
{
    private GrassAgent parent;
    private bool trampled = false;
    public override void Enter()
    {
        parent = (GrassAgent)base.parent;
        senseFrequency = 0.1f;
        base.Enter();
    }

    public override void Sense()
    {
        SenseTrampled();
    }
    public override void Act(float deltaTime)
    {
        if (trampled == true)
        {
            return;
        }
        parent.DoScaleChange(-parent.witherSpeed * deltaTime);
        DoColorChange();
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

    private void DoColorChange()
    {
        parent.renderer.color = new Color(0, 0.3f, 0, 1);
    }
}
