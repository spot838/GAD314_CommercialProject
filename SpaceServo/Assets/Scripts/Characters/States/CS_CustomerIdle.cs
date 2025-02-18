using UnityEngine;

// this is a state the customer can return to and it will work out what do to next
// customer should start at this state

public class CS_CustomerIdle : CharacterState
{
    Customer customer;
    float CrossFadeTime = 0.1f;

    public CS_CustomerIdle(Character character) : base(character)
    {
        this.character = character;
        customer = character.GetComponent<Customer>();
    }

    public override void StateStart()
    {
        if (customer == null)
        {
            Debug.LogError(customer.name + " is not a customer, cannot enter state");
            character.SetNewState(new CS_Idle(character));
            return;
        }

        customer.Animator.CrossFade("Idle", CrossFadeTime);
    }

    public override void StateTick()
    {
        if (!customer.HasBoughtFuel && Station.TryGetAvialableTransactionDesk(out TransactionDesk desk))
        {
            customer.SetNewState(new CS_CustomerMovingToTransactionDesk(customer, desk));
        }

        // TODO: else if (!shipHasRefuled)

        else if (customer.HasBoughtFuel)
        {
            customer.SetNewState(new CS_CustomerReturningToShip(customer));
        }

        else // nothing else to do
        {
            customer.SetNewState(new CS_CustomerWonderingHallway(customer));
        }
    }

    public override void StateEnd()
    {

    }
}
