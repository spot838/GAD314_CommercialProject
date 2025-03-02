using UnityEngine;

// this scripts is the central scpirt gives access to all other scripts that run the game session

public class Game : MonoBehaviour
{
    public static Game Instance;

    [SerializeField] private InputReader inputReader;
    [SerializeField] private StationFloorBuilder floorBuilder;
    [SerializeField] private StationPlaceableBuilder placeableBuilder;
    [SerializeField] private CameraController cameraController;
    [SerializeField] private LayerMask staionLayer;
    [SerializeField] private LayerMask placeablesLayer;
    [SerializeField] private LayerMask staionFloorLayer;
    [SerializeField] private LayerMask roomLayer;
    [SerializeField] private Selection selection;

    public static InputReader Input => Instance.inputReader;
    public static StationFloorBuilder FloorBuilder => Instance.floorBuilder;
    public static StationPlaceableBuilder PlaceableBuilder => Instance.placeableBuilder;
    public static CameraController CameraController => Instance.cameraController;
    public static LayerMask StationLayer => Instance.staionLayer;
    public static LayerMask PlaceableLayer => Instance.placeablesLayer;
    public static LayerMask StationFloorLayer => Instance.staionFloorLayer;
    public static LayerMask RoomLayer => Instance.roomLayer;
    public static Selection Selection => Instance.selection;

    public static bool IsPaused => Time.timeScale == 0;

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

    public static void PauseGame(bool pause)
    {
        UI.ShowPauseMenu(pause);
        Time.timeScale = pause ? 0 : 1;
    }
}
