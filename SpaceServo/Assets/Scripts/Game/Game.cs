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
    [SerializeField] private DebugModule debugModule;
    [SerializeField] private Tutorial tutorial;
    [SerializeField] private ObjectiveSystem objectiveSystem;

    public static InputReader Input => Instance.inputReader;
    public static StationFloorBuilder FloorBuilder => Instance.floorBuilder;
    public static StationPlaceableBuilder PlaceableBuilder => Instance.placeableBuilder;
    public static CameraController CameraController => Instance.cameraController;
    public static LayerMask StationLayer => Instance.staionLayer;
    public static LayerMask PlaceableLayer => Instance.placeablesLayer;
    public static LayerMask StationFloorLayer => Instance.staionFloorLayer;
    public static LayerMask RoomLayer => Instance.roomLayer;
    public static Selection Selection => Instance.selection;
    public static DebugModule Debug => Instance.debugModule;
    public static Tutorial Tutorial => Instance.tutorial;
    public static bool IsPlacing => Instance.floorBuilder.IsPlacing || Instance.placeableBuilder.IsPlacing;
    public static bool IsPaused => Time.timeScale == 0;
    public static ObjectiveSystem ObjectiveSystem => Instance.objectiveSystem;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }

    public static void PauseGame(bool pause)
    {
        UI.ShowPauseMenu(pause);
        Time.timeScale = pause ? 0 : 1;
    }
}
