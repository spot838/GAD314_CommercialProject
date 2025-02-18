using UnityEngine;

public class CS_CustomerCompletingTransaction : CharacterState
{
    Customer customer;
    float CrossFadeTime = 0.1f;
    TransactionDesk desk;

    float timer;

    public CS_CustomerCompletingTransaction(Character character, TransactionDesk desk) : base(character)
    {
        customer = character.GetComponent<Customer>();
        this.desk = desk;
        desk.CurrentCustomer = customer;
        customer.transform.rotation = desk.CustomerPosition.rotation;
    }

    public override void StateStart()
    {
        customer.Animator.CrossFade("Idle", CrossFadeTime);
        timer = desk.TransactionBaseTime;
    }

    public override void StateTick()
    {
        if (timer > 0)
            timer -= Time.deltaTime;

        else // timer reached zero
        {
            customer.SetNewState(new CS_CustomerIdle(character));
        }
    }

    public override void StateEnd()
    {
        desk.CurrentCustomer = null;
        customer.HasBoughtFuel = true;
    }
}
