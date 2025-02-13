using UnityEngine;

// central UI script, accessing the UI should go through this script from all other scripts

// please use the "UI_" prefix for all UI related scripts

public class UI : MonoBehaviour
{
    public static UI Instance;
    [SerializeField] private UI_TopBar topBar;

    public static bool MouseOverUI;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }

    private void Start()
    {
        if (topBar == null) Debug.LogError("UI is missing referance to top bar");
    }

    public static void UpdateMoneyText()
    {
        Instance.topBar.UpdateMoneyText();
    }

    public static void UpdateRatingText()
    {
        Instance.topBar.UpdateRaitingText();
    }
}
