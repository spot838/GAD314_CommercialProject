using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [System.Serializable]
    public class TutorialPart
    {
        [field: SerializeField] public string TutorialText { get; private set; }
        [field: SerializeField] public EType Type { get; private set; }
        [field: SerializeField] public Objective Objective { get; private set; }
        [field: SerializeField] public EMenuType MenuType { get; private set; }
        [field: SerializeField] public ScriptableObject Config { get; private set; }
        [field: SerializeField] public bool ShowNextObjective { get; private set; }

        public enum EType
        {
            ClickThrough,
            Objective,
            OpenMenu,
            StartRoomBuild,
            StartPlacement
        }

        public enum EMenuType
        {
            None,
            RoomMenu,
            PlaceablesMenu
        }
    }

    [field: SerializeField] public TutorialPart[] Parts { get; private set; }

    public TutorialPart CurrentPart => Parts[index];

    [SerializeField] int index = -1;
    public bool IsRunning => index >= 0 && index < Parts.Length;

    private void Start()
    {
        if (Parts.Length > 0) index = 0;
        UI.ShowTutorial(true);
    }

    public void PartComplete()
    {
        index++;
        UI.UpdateTutorial();
        UI.UpdateObjectives();
    }

    public void ExitTutorial()
    {
        index = -1;
        UI.ShowTutorial(false);
        UI.UpdateObjectives();
    }

    public Objective NextObjective
    {
        get
        {
            if (index >= 0 && index < Parts.Length)
            {
                for (int i = index; i < Parts.Length; i++)
                {
                    if (Parts[i].Type == TutorialPart.EType.Objective)
                    {
                        return Parts[i].Objective;
                    }
                }
            }
            return null;
        }
    }

    public bool ListeningForRoomMenu => IsRunning && CurrentPart.Type == TutorialPart.EType.OpenMenu && CurrentPart.MenuType == TutorialPart.EMenuType.RoomMenu;

    public bool ListentingForPlacablesMenu => IsRunning && CurrentPart.Type == TutorialPart.EType.OpenMenu && CurrentPart.MenuType == TutorialPart.EMenuType.PlaceablesMenu;

    public bool ListentingForFloorBuildStart => IsRunning && CurrentPart.Type == TutorialPart.EType.StartRoomBuild;

    public bool ListentingForPlacementStart => IsRunning && CurrentPart.Type == TutorialPart.EType.StartPlacement;

    public void RoomBuiltStarted(Room roomConfig)
    {
        if ((Room)CurrentPart.Config == roomConfig) PartComplete();
    }

    public void PlacementStarted(Placeable placeable)
    {
        if ((Placeable)CurrentPart.Config == placeable) PartComplete();
    }
}
