using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepAgent : Entity
{
    public float startHP = 100f;
    public float currentHP = 100f;
    public float maxHP = 100f;
    public float minSize = 0.09f;
    public float treshhold = 120f;

    public float senseGrassRadius = 1.0f;
    public float senseWolfRadius = 1.0f;

    public float moveFrequncy = 0.5f;

    public float eatingSpeed = 10.0f;
    public bool ableToMove = true;

    public float loseHealthSpeed = 3.0f;

    public List<string> obstaclesList = new List<string>();
    public SpriteRenderer renderer;
    public Color wanderColor = new Color(1, 1, 1, 1);
    public Color eatingColor = new Color(0, 1, 0, 1);
    public Color findingColor = new Color(1, 0.5f, 1, 1);
    public Color runningColor = new Color(1, 1, 0.5f, 1);

    protected override void Init()
    {
        Type = "Sheep";

        currentHP = startHP;
        //Entance
        renderer = GetComponent<SpriteRenderer>();
        stateMachine.SwitchState(new SheepState_Wander());
        obstaclesList.Add("Sheep");
        obstaclesList.Add("Wolf");
    }

    private void Update()
    {
        base.Update();

        LoseHealth();

        isALiveCheck();

        CalculateScale();
    }

    public void DoScaleChange(float number)
    {
        currentHP += number;

        if (currentHP <= 0)
        {
            currentHP = 0;
        }
    }

    private void LoseHealth()
    {
        currentHP -= loseHealthSpeed * Time.deltaTime;
    }
    private void CalculateScale()
    {
        float scale = currentHP / maxHP * minSize * 2;
        if (scale <= minSize)
        {
            scale = minSize;
        }
        transform.localScale = new Vector3(scale, scale, scale);
    }
    private void isALiveCheck()
    {
        if (currentHP <= 0)
        {
            tileManager.RemoveObject(gameObject);
        }
    }
}
