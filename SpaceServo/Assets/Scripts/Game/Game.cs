using UnityEngine;

// this scripts is the central scpirt gives access to all other scripts that run the game session

public class Game : MonoBehaviour
{
    public static Game Instance;

    [SerializeField] private InputReader inputReader;
    [SerializeField] private StationMoney stationMoney;
    [SerializeField] private StationRating stationRating;

    public static InputReader Input => Instance.inputReader;
    public static StationMoney Money => Instance.stationMoney;
    public static StationRating Rating => Instance.stationRating;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (inputReader == null) Debug.LogError("Game is missing input system reference");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
