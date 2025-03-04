using UnityEngine;

public class InteractionElement : MonoBehaviour
{
    [field: SerializeField] public InteractablePlaceableObject InteractablePlaceable { get; private set; }
    [field: SerializeField] public Transform CustomerTransform { get; private set; }
    public Customer IncomingCustomer;
    public Customer CurrentCustomer;

    public bool IsAvailable => IncomingCustomer == null && CurrentCustomer == null;

    private void Awake()
    {
        if (InteractablePlaceable == null) InteractablePlaceable = GetComponent<InteractablePlaceableObject>();
        if (InteractablePlaceable == null) InteractablePlaceable = GetComponentInParent<InteractablePlaceableObject>();
    }
}
