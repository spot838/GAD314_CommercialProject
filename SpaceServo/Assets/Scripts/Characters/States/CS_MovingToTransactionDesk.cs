using UnityEngine;

public class CS_MovingToTransactionDesk : CustomerState
{
    float crossFadeTime = 0.1f;
    TransactionDesk desk;

    public CS_MovingToTransactionDesk(Customer customer, TransactionDesk desk) : base(customer)
    {
        this.customer = customer;
        this.desk = desk;
    }

    public override void StateStart()
    {
        desk.CurrentCustomer = customer;


        customer.Animator.CrossFade("Walk", crossFadeTime);
        customer.NavMeshAgent.SetDestination(desk.CustomerPosition.position);
    }

    public override void StateTick()
    {
        if (customer.ArrivedAtDestination)
        {
            //customer.SetNewState(new CS_CompletingTransaction(customer, desk));
            desk.BeginTransaction();
        }
    }

    public override void StateEnd()
    {

    }

}
