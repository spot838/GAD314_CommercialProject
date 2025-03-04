using UnityEngine;

public class InteractablePlaceableObject : PlaceableObject
{
    [Header("Interactable Settings")]
    [field: SerializeField] public InteractionElement[] InteractionElements { get; private set; }
    [field: SerializeField] public float InteractionTime { get; private set; } = 2f; // how long an interaction takes
    [field: SerializeField] public float Satisfaction { get; private set; } = 5f; // satisfaction from interaction
    [field: SerializeField] public string CustomerAnimationName { get; private set; } = "";
    [field: SerializeField] public float ProximityStart { get; private set; } = 1f;

    private void Awake()
    {
        if (InteractionElements.Length == 0)
        {
            InteractionElements = GetComponentsInChildren<InteractionElement>();
        }
    }

    public int AvailableSlots
    {
        get
        {
            int freeSlots = 0;

            foreach  (InteractionElement element in InteractionElements)
            {
                if (element.IsAvailable) freeSlots++;
            }

            return freeSlots;
        }
    }

    public bool HasFreeSlots
    {
        get
        {
            if (InteractionElements.Length == 0) return true;
            return AvailableSlots > 0;
        }
    }

    public bool TryGetFreeInteractionElement(out InteractionElement interactionElement)
    {
        foreach (InteractionElement element in InteractionElements)
        {
            if (element.IsAvailable)
            {
                interactionElement = element;
                return true;
            }
        }

        interactionElement = null;
        return false;
    }
}
