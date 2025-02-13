using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_AddPlaceablesItem : MonoBehaviour
{
    [SerializeField] Button button;
    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text costText;
    [SerializeField] RawImage icon;

    Placeable config;

    public void Initilize(Placeable newConfig)
    {
        config = newConfig;
        button.onClick.AddListener(OnButtonPress);

        icon.texture = config.Icon;
        nameText.text = config.Name;
        costText.text = "$" + config.Cost;
    }

    private void OnButtonPress()
    {
        Game.PlaceableBuilder.BeginPlacement(config);

        button.onClick.RemoveListener(OnButtonPress);
        UI.ShowPlaceablesMenu(false);
        UI.MouseOverUI = false;
    }
}
