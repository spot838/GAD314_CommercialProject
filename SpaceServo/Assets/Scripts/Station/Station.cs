using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEditor;
using UnityEngine;

public class Station : MonoBehaviour
{
    public static Station Instance;

    [SerializeField] private StationMoney stationMoney;
    [SerializeField] private StationRating stationRating;
    [SerializeField] private ShipManager shipManager;
    [SerializeField] private CustomerManager customerManager;
    [SerializeField] private Transform roomTransform;

    [Header("DEBUG")]
    [SerializeField] List<PlaceableObject> placeableObjects;
    [SerializeField] List<LandingPad> landingPads;
    [SerializeField] List<TransactionDesk> transactionDesks;
    [SerializeField] List<RoomObject> rooms = new List<RoomObject>();
    
    NavMeshSurface navMeshSurface;

    public static GameObject Object => Instance.gameObject;
    public static StationMoney Money => Instance.stationMoney;
    public static StationRating Rating => Instance.stationRating;
    public static List<LandingPad> LandingPads => Instance.landingPads;
    public static List<TransactionDesk> TransactionDesks => Instance.transactionDesks;
    public static NavMeshSurface NavMeshSurface => Instance.navMeshSurface;
    public static List<RoomObject> Rooms => Instance.rooms;
    public static ShipManager ShipManager => Instance.shipManager;
    public static CustomerManager CustomerManager => Instance.customerManager;
    public static bool HasRooms => Instance.rooms.Count > 0;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }

    public static void AddPlaceable(PlaceableObject newobject)
    {
        if (newobject.TryGetComponent<LandingPad>(out LandingPad landingPad))
        {
            Instance.landingPads.Add(landingPad);
        }
        else if (newobject.TryGetComponent<TransactionDesk>(out TransactionDesk transactionDesk))
        {
            Instance.transactionDesks.Add(transactionDesk);
        }
        else
        {
            Instance.placeableObjects.Add(newobject);
        }

        newobject.transform.parent = Instance.transform;
    }

    public static void SetNavMeshSurface(NavMeshSurface navMesh)
    {
        Instance.navMeshSurface = navMesh;
    }

    public static bool TryGetAvialableTransactionDesk(out TransactionDesk transactionDesk)
    {
        foreach (TransactionDesk desk in Instance.transactionDesks)
        {
            if (desk.IsAvailable)
            {
                transactionDesk = desk;
                return true;
            }
        }

        transactionDesk = null;
        return false;
    }

    public static void AddRoom(RoomObject room)
    {
        Instance.rooms.Add(room);
        room.transform.parent = Instance.roomTransform.transform;
    }

    public static void RemoveRoom(RoomObject room)
    {
        Instance.rooms.Remove(room);
    }

    public static FloorTile TileAtLocation(Vector3 location)
    {
        foreach(RoomObject room in Instance.rooms)
        {
            foreach(FloorTile tile in room.Floor)
            {
                if (tile.transform.position == location)
                {
                    return tile;
                }
            }
        }

        return null;
    }
}
