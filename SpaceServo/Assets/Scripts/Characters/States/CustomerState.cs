using UnityEngine;

public abstract class CustomerState : CharacterState
{
    protected Customer customer;

    public CustomerState(Customer customer)
    {
        this.customer = customer;
    }
}
