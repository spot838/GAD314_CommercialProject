using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Playables;

public class UI_AddPlaceablesMenu : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] UI_AddPlaceablesItem addPlaceablesItemPrefab;
    [SerializeField] Transform contentWindow;

    List<UI_AddPlaceablesItem> AddPlaceablesItems = new List<UI_AddPlaceablesItem>();

    private void OnEnable()
    {
        if (Game.Selection.Room != null)
        {
            foreach(Placeable roomPlaceable in Game.Selection.Room.Config.Placeables)
            {
                if (roomPlaceable.Prefab == null) continue;
                UI_AddPlaceablesItem placeablesItem = Instantiate(addPlaceablesItemPrefab, contentWindow);
                placeablesItem.Initilize(roomPlaceable);
                AddPlaceablesItems.Add(placeablesItem);
            }

            foreach (Placeable placeable in Game.PlaceableBuilder.Placeables)
            {
                if (placeable.Prefab == null) continue;
                UI_AddPlaceablesItem placeablesItem = Instantiate(addPlaceablesItemPrefab, contentWindow);
                placeablesItem.Initilize(placeable);
                AddPlaceablesItems.Add(placeablesItem);
            }
        }

        else
        {
            foreach (Placeable placeable in Game.PlaceableBuilder.BuildablePlaceables)
            {
                if (placeable.Prefab == null) continue;
                UI_AddPlaceablesItem placeablesItem = Instantiate(addPlaceablesItemPrefab, contentWindow);
                placeablesItem.Initilize(placeable);
                AddPlaceablesItems.Add(placeablesItem);
            }
        }

        if (Game.Tutorial.ListentingForPlacablesMenu) Game.Tutorial.PartComplete();

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
