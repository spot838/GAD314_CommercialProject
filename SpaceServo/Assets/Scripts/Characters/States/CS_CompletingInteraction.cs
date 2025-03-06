using UnityEngine;

public class CS_CompletingInteraction : CustomerState
{
    float crossFadeTime = 0.1f;
    InteractablePlaceableObject interactable;
    float timer;

    public CS_CompletingInteraction(Customer customer, InteractablePlaceableObject interactable) : base(customer)
    {
        this.customer = customer;
        this.interactable = interactable;
    }

    public override void StateEnd()
    {
        if (interactable.CustomerAnimationName != "")
            customer.Animator.CrossFade(interactable.CustomerAnimationName, crossFadeTime);
        else
            customer.Animator.CrossFade("Idle", crossFadeTime);

        timer = interactable.InteractionTime;
    }

    public override void StateStart()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            customer.SetNewState(new CS_WonderingInRoom(customer, interactable.Room));
        }
    }

    public override void StateTick()
    {
    }

    public override string Status()
    {
        return "Doing something"; // TODO: replace with a verb veriable on the interactable
    }
}
