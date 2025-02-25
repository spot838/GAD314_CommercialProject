using UnityEngine;

public class CS_ReturningToShip : CustomerState
{
    float crossFadeTime = 0.1f;

    public CS_ReturningToShip(Customer customer) : base(customer)
    {
        this.customer = customer;
    }

    public override void StateStart()
    {
        customer.Animator.CrossFade("Walk", crossFadeTime);
        customer.NavMeshAgent.SetDestination(customer.Ship.LandingPad.CustomerSpawnPoint.position);
    }

    public override void StateTick()
    {
        if (customer.ArrivedAtDestination)
        {
            Station.Rating.AddCustomerSatisfaction(customer.Satisfaction.ValueCurrentVisual);
            customer.DestoryCustomer();
        }
    }

    public override void StateEnd()
    {
        
    }
}
