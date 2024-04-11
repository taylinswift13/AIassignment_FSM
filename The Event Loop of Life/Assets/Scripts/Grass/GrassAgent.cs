using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassAgent : Entity
{
    public float maxHP = 100f;
    public float startHP = 50f;
    public float maxSize = 0.14f;

    public float currentHP;

    public float growSpeed = 20f;
    public float witherSpeed = 100f;
    public float stayMatureTime = 2.0f;
    public SpriteRenderer renderer;
    protected override void Init()
    {
        Type = "Grass";
        currentHP = startHP;
        renderer = GetComponent<SpriteRenderer>();
        stateMachine.SwitchState(new GrassState_Grow());
        stayMatureTime += Random.Range(-0.2f, 0.2f);
        growSpeed += Random.Range(-2f, 2f);
        witherSpeed += Random.Range(-10f, 10f);
        startHP += Random.Range(-10f, 10f);
    }

    private void Update()
    {
        base.Update();

        CalculateScale();

        isAliveCheck();
    }

    private void CalculateScale()
    {
        float scale = currentHP / maxHP * maxSize;
        transform.localScale = new Vector3(scale, scale, scale);
    }

    public void DoScaleChange(float number)
    {
        currentHP += number;

        if (currentHP <= 0)
        {
            currentHP = 0;
        }

        if (currentHP >= maxHP)
        {
            currentHP = maxHP;
        }
    }
    private void isAliveCheck()
    {
        if (currentHP <= 0)
        {
            tileManager.RemoveObject(gameObject);
        }
    }
}
