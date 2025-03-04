using UnityEngine;
using UnityEngine.AI;

public class CS_WonderingInRoom : CustomerState
{
    float crossFadeTime = 0.1f;
    RoomObject room;

    public CS_WonderingInRoom(Customer customer, RoomObject room) : base(customer)
    {
        this.customer = customer;
        this.room = room;
    }

    public override void StateStart()
    {
        Debug.Log(customer.name + " WonderingInRoom " + room.name);

        customer.Animator.CrossFade("Walk", crossFadeTime);
    }

    public override void StateTick()
    {
        if (customer.RemainingInteractions.Count == 0)
        {
            customer.RoomsVisited.Add(room);
            customer.SetNewState(new CS_WonderingHallway(customer));
        }

        else if (customer.RemainingInteractions[0].IsTransactionDesk &&
            room.TryGetAvailableDesk(out TransactionDesk desk))
        {
            customer.SetNewState(new CS_MovingToTransactionDesk(customer, desk));
        }

        else if (customer.RemainingInteractions[0].IsInteractable &&
            room.TryGetAvailableElement(customer.RemainingInteractions[0], out InteractionElement intElement))
        {
            customer.SetNewState(new CS_MoveToInteractableElement(customer, intElement));
        }

        else if (customer.RemainingInteractions[0].IsInteractable &&
            room.TryGetAvailableInteractable(customer.RemainingInteractions[0], out InteractablePlaceableObject interactable))
        {
            customer.SetNewState(new CS_MoveToInteractable(customer, interactable));
        }

        else if (customer.ArrivedAtDestination)
        {
            customer.NavMeshAgent.SetDestination(randomPosition);
        }
    }

    public override void StateEnd()
    {
        if (room.IncomingCustomers.Contains(customer)) room.IncomingCustomers.Remove(customer);
    }

    private Vector3 randomPosition
    {
        get
        {
            bool havePosition = false;
            int maxAttempts = 5;
            int attempt = 0;

            while (attempt < maxAttempts && !havePosition)
            {
                attempt++;

                FloorTile randomTile = null;
                if (room != null)
                    randomTile = room.Floor[Random.Range(0, room.Floor.Count)];
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
