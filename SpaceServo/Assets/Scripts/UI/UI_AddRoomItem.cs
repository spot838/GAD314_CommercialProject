using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_AddRoomItem : MonoBehaviour
{
    [SerializeField] Button button;
    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text costText;
    [SerializeField] RawImage icon;

    Room config;

    public void Initilize(Room newConfig)
    {
        config = newConfig;
        button.onClick.AddListener(OnButtonPress);

        icon.texture = config.Icon;
        nameText.text = config.Name;
        costText.text = "$" + config.BasePrice + " + " + config.CostPerTile + " Per/Tile";
    }

    private void OnButtonPress()
    {
        Game.FloorBuilder.StartPlacingFloor(config);

        button.onClick.RemoveListener(OnButtonPress);
        UI.ShowRoomsMenu(false);
        UI.MouseOverUI = false;
    }
}
