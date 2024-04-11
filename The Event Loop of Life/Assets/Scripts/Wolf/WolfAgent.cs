using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfAgent : Entity
{
    public float startHP = 100f;
    public float currentHP = 100f;
    public float maxHP = 100f;
    public float treshhold = 120f;
    public float minSize = 0.09f;
    public float senseSheepRadius = 1.0f;
    public float moveFrequncy = 0.5f;
    public float eatingSpeed = 10.0f;
    public List<string> obstaclesList = new List<string>();
    public float loseHealthSpeed = 5.0f;
    public SpriteRenderer renderer;
    public Color wanderColor = new Color(1, 1, 1, 1);
    public Color eatingColor = new Color(1, 0.3f, 0.3f, 1);
    public Color findingColor = new Color(1, 1, 0.5f, 1);
    protected override void Init()
    {
        Type = "Wolf";

        currentHP = startHP;
        //Entance
        renderer = GetComponent<SpriteRenderer>();
        stateMachine.SwitchState(new WolfState_Wander());

        moveFrequncy += Random.Range(-0.1f, 0.1f);

        obstaclesList.Add("Wolf");
    }

    private void Update()
    {
        base.Update();

        LoseHealth();

        isAliveCheck();

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
    private void isAliveCheck()
    {
        if (currentHP <= 0)
        {
            tileManager.RemoveObject(gameObject);
        }
    }
}
