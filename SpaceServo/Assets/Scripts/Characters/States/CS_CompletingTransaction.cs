using UnityEngine;

public class CS_CompletingTransaction : CustomerState
{
    float crossFadeTime = 0.1f;
    TransactionDesk desk;

    float timer;

    public CS_CompletingTransaction(Customer customer, TransactionDesk desk) : base(customer)
    {
        this.customer = customer;
        this.desk = desk;
    }

    public override void StateStart()
    {
        desk.CurrentCustomer = customer;
        customer.transform.rotation = desk.CustomerPositionTransform.rotation;

        customer.Animator.CrossFade("Idle", crossFadeTime);
        timer = desk.TransactionBaseTime;
    }

    public override void StateTick()
    {
        if (timer > 0)
            timer -= Time.deltaTime;

        else // timer reached zero
        {
            //customer.SetNewState(new CS_Idle(customer));
            desk.CompleteTransaction();
        }
    }

    public override void StateEnd()
    {
        //desk.CurrentCustomer = null;
        //customer.HasBoughtFuel = true;
    }
}
