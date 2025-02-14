using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class Station : MonoBehaviour
{
    public static Station Instance;

    [SerializeField] private StationMoney stationMoney;
    [SerializeField] private StationRating stationRating;

    [Header("DEBUG")]
    [SerializeField] List<PlaceableObject> placeableObjects;
    [SerializeField] List<LandingPad> landingPads;
    [SerializeField] List<TransactionDesk> transactionDesks;
    
    NavMeshSurface navMeshSurface;

    public static GameObject Object => Instance.gameObject;
    public static StationMoney Money => Instance.stationMoney;
    public static StationRating Rating => Instance.stationRating;
    public static List<LandingPad> LandingPads => Instance.landingPads;
    public static List<TransactionDesk> TransactionDesks => Instance.transactionDesks;
    public static NavMeshSurface NavMeshSurface => Instance.navMeshSurface;

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
}
