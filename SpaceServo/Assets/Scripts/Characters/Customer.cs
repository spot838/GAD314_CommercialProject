using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Customer : Character
{
    [field: SerializeField] public Ship Ship { get; private set; }

    public bool HasRefueled => Ship.Fuel.HasRefueled;
    Transform target;
    public bool KeepIdle;
    //[field: SerializeField] public EntityStat Satisfaction { get; private set; } = new();
    [field: SerializeField] public float Satisfaction { get; private set; } = 0;
    [field: SerializeField] public List<Placeable> RemainingInteractions = new List<Placeable>();
    [field: SerializeField] public List<RoomObject> RoomsVisited = new List<RoomObject>();

    protected override void Update()
    {
        base.Update();
    }

    public void Initilize(Ship ship)
    {
        Ship = ship;
        SetNewState(new CS_Idle(this));
    }

    private void OnDestroy()
    {
        Ship.BeginTakeOff();
        Station.CustomerManager.CustomerDespawn(this);
    }

    public void DestoryCustomer()
    {
        Destroy(this.gameObject);
    }

    public void ModifySatisfaction(float amount)
    {
        Satisfaction = Mathf.Clamp(Satisfaction + amount, Station.Rating.MIN_RATING, Station.Rating.MAX_RATING);
    }
}