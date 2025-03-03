using UnityEngine;
using UnityEngine.Rendering;

public class Customer : Character
{
    [field: SerializeField] public Ship Ship { get; private set; }

    public bool HasRefueled => Ship.Fuel.HasRefueled;
    Transform target;
    public bool KeepIdle;
    [field: SerializeField] public EntityStat Satisfaction { get; private set; } = new();

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
        Station.Rating.AddCustomerSatisfaction(Satisfaction.ValueCurrent);
        Ship.BeginTakeOff();
        Station.CustomerManager.CustomerDespawn(this);
    }

    public void DestoryCustomer()
    {
        // apply customer satisfaction to station rating
        /*float custStatisfactionDifference = Satisfaction.ValueCurrent - Satisfaction.ValueBase;
        if (custStatisfactionDifference > 0)
        {
            Debug.Log($"Customer Satisfaction: {Satisfaction.ValueCurrent.ToString()}, Positive by {custStatisfactionDifference}");
        }
        else if (custStatisfactionDifference < 0)
        {
            Debug.Log($"Customer Satisfaction: {Satisfaction.ValueCurrent.ToString()}, Negative by {custStatisfactionDifference}");
        }
        else
        {
            Debug.Log($"Customer Satisfaction: {Satisfaction.ValueCurrent.ToString()}, Neutral");
        }*/
        Destroy(this.gameObject);
    }
}