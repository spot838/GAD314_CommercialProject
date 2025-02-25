using UnityEngine;

// this is a state the customer can return to and it will work out what do to next
// customer should start at this state

public class CS_Idle : CustomerState
{
    float CrossFadeTime = 0.1f;

    public CS_Idle(Customer customer) : base(customer)
    {
        this.customer = customer;
    }

    public override void StateStart()
    {
        if (customer == null)
        {
            Debug.LogError("Customer State missing reference to Customer");
            return;
        }

        customer.Animator.CrossFade("Idle", CrossFadeTime);
    }

    public override void StateTick()
    {
        if (!customer.HasBoughtFuel && Station.TryGetAvialableTransactionDesk(out TransactionDesk desk))
        {
            customer.SetNewState(new CS_MovingToTransactionDesk(customer, desk));
        }

        // TODO: else if (!shipHasRefuled)

        else if (customer.HasBoughtFuel)
        {
            customer.SetNewState(new CS_ReturningToShip(customer));
        }

        else // nothing else to do
        {
            customer.SetNewState(new CS_WonderingHallway(customer));
        }
    }

    public override void StateEnd()
    {

    }
}
