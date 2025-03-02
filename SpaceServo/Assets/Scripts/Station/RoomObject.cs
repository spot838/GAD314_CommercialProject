using System.Collections.Generic;
using UnityEngine;

public class RoomObject : MonoBehaviour
{
    [field: SerializeField] public Room Config {  get; private set; }

    public List<FloorTile> Floor { get; private set; } = new List<FloorTile>();
    public List<PlaceableObject> Placeables { get; private set; } = new List<PlaceableObject>();
    public int Cost => Config.Cost(Floor.Count);

    public void Initialize(Room config)
    {
        Config = config;
        name = config.Name;
    }

    public void AddFloorTile(FloorTile tile)
    {
        Floor.Add(tile);
        tile.transform.parent = transform;
        tile.SetRoom(this);
    }

    public void RemoveTile(FloorTile tile)
    {
        Floor.Remove(tile);
    }

    public void AddPlaceable(PlaceableObject placeableObject)
    {
        Placeables.Add(placeableObject);
    }

    public void RemovePlaceable(PlaceableObject placeableObject)
    {
        Placeables.Remove(placeableObject);
    }


    private void OnDestroy()
    {
        Station.RemoveRoom(this);
    }
}
