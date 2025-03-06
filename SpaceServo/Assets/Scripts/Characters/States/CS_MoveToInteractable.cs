using UnityEngine;

public class CS_MoveToInteractable : CustomerState
{
    float crossFadeTime = 0.1f;
    InteractablePlaceableObject interactable;

    public CS_MoveToInteractable(Customer customer, InteractablePlaceableObject interactable) : base(customer)
    {
        this.customer = customer;
        this.interactable = interactable;
    }

    public override void StateStart()
    {
        customer.Animator.CrossFade("Walk", crossFadeTime);

        customer.NavMeshAgent.SetDestination(interactable.transform.position);

        customer.RemainingInteractions.RemoveAt(0);
    }

    public override void StateTick()
    {
        if (Vector3.Distance(customer.transform.position, interactable.transform.position) < interactable.ProximityStart)
        {
            customer.SetNewState(new CS_CompletingInteraction(customer, interactable));
        }
    }

    public override void StateEnd()
    {
    }

    public override string Status()
    {
        return "Walking to " + interactable.Config.Name;
    }
}
