public class State
{
    public Entity parent;
    public TileManager tileManager;

    protected float senseFrequency = 0.3f;
    private float accumulateTime = 0.0f;

    public virtual void Enter()
    {
        Sense();
    }

    public void Update(float deltaTime)
    {
        accumulateTime += deltaTime;

        if (accumulateTime >= senseFrequency)
        {
            Sense();
            Decide();
        }

        Act(deltaTime);
    }

    public virtual void Act(float deltaTime)
    {

    }

    public virtual void Sense()
    {

    }

    public virtual void Decide()
    {

    }
}
