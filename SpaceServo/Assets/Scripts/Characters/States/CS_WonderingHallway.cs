using UnityEngine;
using UnityEngine.AI;

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
        customer.NavMeshAgent.SetDestination(randomHPosition);
    }

    public override void StateTick()
    {
        if (customer.ArrivedAtDestination)
        {
            customer.NavMeshAgent.SetDestination(randomHPosition);
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

    private Vector3 randomHPosition // a position in the hallway or hanger
    {
        get
        {
            bool havePosition = false;
            int maxAttempts = 5;
            int attempt = 0;

            while (attempt < maxAttempts && !havePosition)
            {
                attempt++;

                RoomObject randomRoom = Station.RandomHallwayOrHanger;
                FloorTile randomTile = null;
                if (randomRoom != null)
                    randomTile = randomRoom.Floor[Random.Range(0, randomRoom.Floor.Count)];
                if (randomTile != null)
                {
                    Vector3 newPosition = randomTile.transform.position;
                    newPosition.y = customer.transform.position.y;

                    if (NavMesh.SamplePosition(newPosition, out NavMeshHit hit, 2.5f, NavMesh.AllAreas))
                    {
                        havePosition = true; // might not need this bool
                        return hit.position;
                    }

                    //else return newPosition;
                }
            }

            Debug.LogWarning(customer.name + ": Ran out of attempts to find a valid navMesh location");
            return customer.transform.position;
        }
    }
}
