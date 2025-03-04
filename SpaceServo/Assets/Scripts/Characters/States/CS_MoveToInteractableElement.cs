using UnityEngine;

public class CS_MoveToInteractableElement : CustomerState
{
    float crossFadeTime = 0.1f;
    InteractionElement interactionElement;

    public CS_MoveToInteractableElement(Customer customer, InteractionElement interactionElement) : base(customer)
    {
        this.customer = customer;
        this.interactionElement = interactionElement;
    }

    public override void StateStart()
    {
        customer.Animator.CrossFade("Walk", crossFadeTime);
        interactionElement.IncomingCustomer = customer;

        customer.NavMeshAgent.SetDestination(interactionElement.CustomerTransform.position);

        customer.RemainingInteractions.RemoveAt(0);
    }

    public override void StateTick()
    {
        if (Vector3.Distance(customer.transform.position, interactionElement.CustomerTransform.position) < interactionElement.InteractablePlaceable.ProximityStart)
        {
            customer.SetNewState(new CS_CompletingElementInteraction(customer, interactionElement));
        }
    }

    public override void StateEnd()
    {
        interactionElement.IncomingCustomer = null;
    }
}
