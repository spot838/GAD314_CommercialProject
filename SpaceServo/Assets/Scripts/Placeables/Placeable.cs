using UnityEngine;

// this is the idea of a placeable object not the physical object itself. this contains info like how much it costs, it's icon, etc.

[CreateAssetMenu(menuName = "SpaceServo/New Placeable")]
public class Placeable : ScriptableObject
{
    [field: SerializeField] public PlaceableObject Prefab { get; private set; }
    [field: SerializeField] public string Name { get; private set; } = "NewPlaceableObject";
    [field: SerializeField] public int Cost { get; private set; } = 10;
    [field: SerializeField] public Texture Icon { get; private set; } = null;

    public bool IsTransactionDesk => Prefab.GetComponent<TransactionDesk>() != null;
    public bool IsInteractable => Prefab.GetComponent<InteractablePlaceableObject>() != null;
    public bool RequiresInteraction => IsInteractable || IsTransactionDesk;

}
