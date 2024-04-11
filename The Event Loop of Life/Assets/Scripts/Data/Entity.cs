using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    protected StateManager stateMachine;
    public TileManager tileManager;

    public string Type = "None";

    private void Start()
    {
        stateMachine = new StateManager(this);//Create the state machine

        tileManager = GameManager.instance.tileManager;

        Init();
    }

    protected void Update()
    {
        stateMachine.Update(Time.deltaTime);
    }

    protected virtual void Init()   //Entrance of all game entities
    {

    }

    public void SwitchState(State newState)
    {
        stateMachine.SwitchState(newState);
    }
}
