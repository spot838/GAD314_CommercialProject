using System.Drawing;
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
    [field: SerializeField] public int BasePrice { get; private set; } = 100;
    [field: SerializeField] public int CostPerTile { get; private set; } = 10;

    public int Cost(int size)
    {
        return BasePrice + (CostPerTile * size);
    }

    public enum EType
    {
        None,
        Hallway,
        FuelPurchase
    }
}
