using UnityEngine;

public class StateManager
{
    private State currentState;
    private Entity Parent;

    public void SwitchState(State newState)
    {
        currentState = newState;

        newState.parent = Parent;
        newState.tileManager = Parent.tileManager;

        currentState.Enter();
    }


    public void Update(float deltaTime)
    {
        if (currentState != null)
        {
            currentState.Update(deltaTime);
        }
    }

    public StateManager(Entity parent)
    {
        this.Parent = parent;
    }
}
