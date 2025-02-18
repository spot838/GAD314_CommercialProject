using UnityEngine;

public class CS_CustomerReturningToShip : CharacterState
{
    Customer customer;
    float CrossFadeTime = 0.1f;

    public CS_CustomerReturningToShip(Character character) : base(character)
    {
        this.character = character;
        customer = character.GetComponent<Customer>();
    }

    public override void StateStart()
    {
        customer.Animator.CrossFade("Walk", CrossFadeTime);
        customer.NavMeshAgent.SetDestination(customer.Ship.LandingPad.CustomerSpawnPoint.position);
    }

    public override void StateTick()
    {
        if (customer.ArrivedAtDestination)
        {
            customer.DestoryCustomer();
        }
    }

    public override void StateEnd()
    {
        
    }
}
