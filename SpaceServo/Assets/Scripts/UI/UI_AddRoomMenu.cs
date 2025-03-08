using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_AddRoomMenu : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] UI_AddRoomItem addRoomItemPrefab;
    [SerializeField] Transform contentWindow;

    List<UI_AddRoomItem> AddRoomItems = new List<UI_AddRoomItem>();

    private void OnEnable()
    {
        foreach (Room room in Game.FloorBuilder.RoomConfigs)
        {
            UI_AddRoomItem roomItem = Instantiate(addRoomItemPrefab, contentWindow);
            roomItem.Initilize(room);
            AddRoomItems.Add(roomItem);
        }

        if (Game.Tutorial.ListeningForRoomMenu) Game.Tutorial.PartComplete();

        Game.Input.OnPrimaryPress += NoItemClicked;
    }

    private void OnDisable()
    {
        foreach (UI_AddRoomItem item in AddRoomItems)
        {
            Destroy(item.gameObject);
        }
        AddRoomItems.Clear();

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
        UI.ShowRoomsMenu(false);
    }
}
