using UnityEngine;

public class Station : MonoBehaviour
{
    public static Station Instance;

    [SerializeField] private StationMoney stationMoney;
    [SerializeField] private StationRating stationRating;

    public static GameObject Object => Instance.gameObject;
    public static StationMoney Money => Instance.stationMoney;
    public static StationRating Rating => Instance.stationRating;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }
}
