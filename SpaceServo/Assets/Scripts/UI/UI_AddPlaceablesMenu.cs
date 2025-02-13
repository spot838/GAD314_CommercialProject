using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_AddPlaceablesMenu : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] UI_AddPlaceablesItem addPlaceablesItemPrefab;
    [SerializeField] Transform contentWindow;

    List<UI_AddPlaceablesItem> AddPlaceablesItems = new List<UI_AddPlaceablesItem>();

    private void OnEnable()
    {
        foreach(Placeable placeable in Game.PlaceableBuilder.Placeables)
        {
            UI_AddPlaceablesItem placeablesItem = Instantiate(addPlaceablesItemPrefab, contentWindow);
            placeablesItem.Initilize(placeable);
            AddPlaceablesItems.Add(placeablesItem);
        }

        Game.Input.OnPrimaryPress += NoItemClicked;
    }

    private void OnDisable()
    {
        foreach(UI_AddPlaceablesItem item in AddPlaceablesItems)
        {
            Destroy(item.gameObject);
        }
        AddPlaceablesItems.Clear();

        Game.Input.OnPrimaryPress -= NoItemClicked;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UI.MouseOverUI = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UI.MouseOverUI = false;
    }

    private void NoItemClicked()
    {
        if (UI.MouseOverUI) return;
        UI.ShowPlaceablesMenu(false);
    }
}
