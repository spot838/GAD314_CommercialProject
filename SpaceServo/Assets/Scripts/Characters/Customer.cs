using UnityEngine;
using UnityEngine.Rendering;

public class Customer : Character
{
    [field: SerializeField] public Ship Ship { get; private set; }

    bool hasBoughtFuel;
    Transform target;
    public bool KeepIdle;

    protected override void Update()
    {
        base.Update();
        if (KeepIdle) return;

        else if (!IsMoving && target == null)
        {
            if (!hasBoughtFuel && Station.TryGetAvialableTransactionDesk(out TransactionDesk desk))
            {
                print("customer moving to buy fuel");
                target = desk.CustomerPosition;
                NavMeshAgent.SetDestination(target.position);
                desk.CurrentCustomer = this;
            }

            else if (hasBoughtFuel) // later this will change to "shipHasRefuled" and customer finished w/e they were doing
            {
                target = Ship.LandingPad.CustomerSpawnPoint;
                NavMeshAgent.SetDestination(target.position);
            }

        }

        else if (hasBoughtFuel && !IsMoving && HasArrivedAtNavMeshDestination && target == Ship.LandingPad.CustomerSpawnPoint)
        {
            // apply customer satisfaction to station rating
            Ship.BeginTakeOff();
            Destroy(this.gameObject);
        }
    }

    public void Initilize(Ship ship)
    {
        Ship = ship;
    }

    private void OnDestroy()
    {
        Game.CustomerManager.CustomerDespawn(this);
    }

    public void CompleteFuelPurchase()
    {
        hasBoughtFuel = true;
        target = null;
        KeepIdle = false;
    }
}
