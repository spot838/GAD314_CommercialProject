using UnityEngine;

public class CS_CustomerWonderingHallway : CharacterState
{
    Customer customer;
    float CrossFadeTime = 0.1f;

    public CS_CustomerWonderingHallway(Character character) : base(character)
    {
        this.character = character;
        customer = character.GetComponent<Customer>();
    }

    public override void StateStart()
    {
        customer.Animator.CrossFade("Walk", CrossFadeTime);
        customer.NavMeshAgent.SetDestination(RandomHallwayPosition);
    }

    public override void StateTick()
    {
        if (customer.ArrivedAtDestination)
        {
            customer.NavMeshAgent.SetDestination(RandomHallwayPosition);
        }

        else if (!customer.HasBoughtFuel && Station.TryGetAvialableTransactionDesk(out TransactionDesk desk))
        {
            customer.SetNewState(new CS_CustomerMovingToTransactionDesk(customer, desk));
        }
    }

    public override void StateEnd()
    {
    }

    private Vector3 RandomHallwayPosition
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
