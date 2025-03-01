using UnityEngine;

public class CS_WonderingHallway : CustomerState
{
    float crossFadeTime = 0.1f;

    public CS_WonderingHallway(Customer customer) : base(customer)
    {
        this.customer = customer;
    }

    public override void StateStart()
    {
        customer.Animator.CrossFade("Walk", crossFadeTime);
        customer.NavMeshAgent.SetDestination(randomHallwayPosition);
    }

    public override void StateTick()
    {
        if (customer.ArrivedAtDestination)
        {
            customer.NavMeshAgent.SetDestination(randomHallwayPosition);
        }

        else if (!customer.HasRefueled && !customer.Ship.LandingPad.IsRefueling
            && Station.TryGetAvialableFuelDesk(out TransactionDesk fuelDesk))
        {
            Debug.Log(customer.name + " moving to buy fuel");
            customer.SetNewState(new CS_MovingToTransactionDesk(customer, fuelDesk));
        }

        else if (!customer.HasRefueled && customer.Ship.LandingPad.IsRefueling
            && Station.TryGetAvialableTransactionDesk(out TransactionDesk desk))
        {
            customer.SetNewState(new CS_MovingToTransactionDesk(customer, desk));
        }

        else if (customer.HasRefueled)
        {
            customer.SetNewState(new CS_ReturningToShip(customer));
        }
    }

    public override void StateEnd()
    {
    }

    private Vector3 randomHallwayPosition
    {
        get
        {
            RoomObject randomHallway = Station.RandomHallway;
            FloorTile randomTile = null;
            if (randomHallway != null) 
                randomTile = randomHallway.Floor[Random.Range(0, randomHallway.Floor.Count)];
            if (randomTile != null)
            {
                Vector3 newPosition = randomTile.transform.position;
                newPosition.y = customer.transform.position.y;
                return newPosition;
            }

            return customer.transform.position;
        }
    }
}
