using Unity.VisualScripting;
using UnityEngine;

public class CS_MovingToTransactionDesk : CustomerState
{
    float crossFadeTime = 0.1f;
    TransactionDesk desk;
    int positionInCue;

    public CS_MovingToTransactionDesk(Customer customer, TransactionDesk desk) : base(customer)
    {
        this.customer = customer;
        this.desk = desk;
    }

    public override void StateStart()
    {
        //desk.CurrentCustomer = customer;
        desk.CustomerQueue.Add(customer);

        customer.Animator.CrossFade("Walk", crossFadeTime);
        positionInCue = desk.CuePosition(customer);
        customer.NavMeshAgent.SetDestination(desk.CustomerPositionTarget(customer));
    }

    public override void StateTick()
    {
        if (positionInCue != desk.CuePosition(customer))
        {
            customer.Animator.CrossFade("Walk", crossFadeTime);
            positionInCue = desk.CuePosition(customer);
            customer.NavMeshAgent.SetDestination(desk.CustomerPositionTarget(customer));
        }

        else if(positionInCue != 0 && customer.ArrivedAtDestination)
        {
            customer.Animator.CrossFade("Idle", crossFadeTime);
        }


        else if (positionInCue == 0 && customer.ArrivedAtDestination)
        {
            desk.BeginTransaction();
        }
    }

    public override void StateEnd()
    {

    }

}
