using System.Linq;
using UnityEngine;

public class CS_MovingToRoom : CustomerState
{
    float crossFadeTime = 0.1f;
    RoomObject room;

    public CS_MovingToRoom(Customer customer, RoomObject room) : base(customer)
    {
        this.customer = customer;
        this.room = room;
    }

    public override void StateStart()
    {
        Debug.Log(customer.name + " MovingToRoom " + room.name);

        customer.Animator.CrossFade("Walk", crossFadeTime);
        customer.NavMeshAgent.SetDestination(room.DoorTiles[0].transform.position);
        room.IncomingCustomers.Add(customer);

        foreach(Placeable placeable in room.Config.Placeables)
        {
            if (placeable.RequiresInteraction) customer.RemainingInteractions.Add(placeable);
        }
    }

    public override void StateTick()
    {
        if (customer.OnTile.Room == room)
        {
            customer.SetNewState(new CS_WonderingInRoom(customer, room));
        }
    }

    public override void StateEnd()
    {
        
        
    }
}
