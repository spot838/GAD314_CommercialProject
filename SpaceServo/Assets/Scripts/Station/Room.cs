using UnityEngine;

[CreateAssetMenu(menuName = "SpaceServo/New Room")]
public class Room : ScriptableObject
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public string Description { get; private set; }
    [field: SerializeField] public EType Type { get; private set; }
    [field: SerializeField] public Texture Icon { get; private set; }
    [field: SerializeField] public FloorTile FloorTilePrefab {  get; private set; }
    [field: SerializeField] public Placeable[] Placeables { get; private set; }

    public enum EType
    {
        None,
        Hallway,
        FuelPurchase
    }
}
