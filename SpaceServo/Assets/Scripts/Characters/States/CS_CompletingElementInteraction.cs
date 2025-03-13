using UnityEngine;

public class CS_CompletingElementInteraction : CustomerState
{
    float crossFadeTime = 0.1f;
    InteractionElement interactionElement;
    float timer;

    public CS_CompletingElementInteraction(Customer customer, InteractionElement interactionElement) : base(customer)
    {
        this.interactionElement = interactionElement;
    }

    public override void StateStart()
    {
        if (interactionElement.InteractablePlaceable.CustomerAnimationName != "")
        {
            customer.Animator.CrossFade(interactionElement.InteractablePlaceable.CustomerAnimationName, crossFadeTime);
            customer.transform.position = interactionElement.CustomerTransform.position;
            customer.transform.rotation = interactionElement.CustomerTransform.rotation;
            customer.NavMeshAgent.enabled = false;
            customer.Rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        }


        else
            customer.Animator.CrossFade("Idle", crossFadeTime);


        interactionElement.CurrentCustomer = customer;
        timer = interactionElement.InteractablePlaceable.InteractionTime;
    }

    public override void StateTick()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            customer.SetNewState(new CS_WonderingInRoom(customer, interactionElement.InteractablePlaceable.Room));
        }
    }

    public override void StateEnd()
    {
        interactionElement.CurrentCustomer = null;
        customer.ModifySatisfaction(interactionElement.InteractablePlaceable.Satisfaction);

        if (interactionElement.InteractablePlaceable.CustomerAnimationName != "")
        {
            customer.NavMeshAgent.enabled = true;
            customer.Rigidbody.freezeRotation = false;
            customer.Rigidbody.constraints = RigidbodyConstraints.None;
        }
    }

    public override string Status()
    {
        return "Doing something"; // add variable to interactables eg "eating" "doing whatever"
    }
}
