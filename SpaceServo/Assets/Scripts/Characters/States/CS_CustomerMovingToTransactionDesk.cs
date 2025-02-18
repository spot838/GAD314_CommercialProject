using UnityEngine;

public class CS_CustomerMovingToTransactionDesk : CharacterState
{
    Customer customer;
    float CrossFadeTime = 0.1f;
    TransactionDesk desk;

    public CS_CustomerMovingToTransactionDesk(Character character, TransactionDesk desk) : base(character)
    {
        customer = character.GetComponent<Customer>();
        this.desk = desk;
        desk.CurrentCustomer = customer;
    }

    public override void StateStart()
    {
        customer.Animator.CrossFade("Walk", CrossFadeTime);
        customer.NavMeshAgent.SetDestination(desk.CustomerPosition.position);
    }

    public override void StateTick()
    {
        if (customer.ArrivedAtDestination)
        {
            customer.SetNewState(new CS_CustomerCompletingTransaction(customer, desk));
        }
    }

    public override void StateEnd()
    {

    }

}
