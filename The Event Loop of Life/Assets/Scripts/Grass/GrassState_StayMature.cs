using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassState_StayMature : State
{
    private bool mature = false;
    private GrassAgent parent;
    private float matureAT = 0.0f;

    public override void Enter()
    {
        parent = (GrassAgent)base.parent;
        senseFrequency = 0.1f;
        base.Enter();
    }

    public override void Sense()
    {
        SenseStayMatureOrWither();
    }

    public override void Decide()
    {
        if (matureAT >= parent.stayMatureTime)
        {
            parent.SwitchState(new GrassState_Wither());
        }
    }

    public override void Act(float deltaTime)
    {
        if (mature == true)
        {
            matureAT += deltaTime;
            DoColorChange();
        }
    }
    private void DoColorChange()
    {
        parent.renderer.color = new Color(0, 1, 0, 1);
    }

    private void SenseStayMatureOrWither()
    {
        if (parent.currentHP >= parent.maxHP)
        {
            mature = true;
        }
    }
}
