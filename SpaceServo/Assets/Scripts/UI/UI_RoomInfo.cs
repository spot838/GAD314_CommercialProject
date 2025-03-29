using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_RoomInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] TMP_Text roomNameText;
    [SerializeField] Button closeButton;
    [SerializeField] Button addFloorButton;
    [SerializeField] Button addPlaceableButton;
    [SerializeField] Button destroyRoomButton;
    [SerializeField] Transform content;
    [SerializeField] UI_RoomInfo_Placeable placeablePrefab;

    [SerializeField] GameObject supplies;
    [SerializeField] Slider suppliesBar;
    [SerializeField] TextMeshProUGUI suppliesText;
    [SerializeField] Button add5SuppliesButton;
    [SerializeField] Button add50SuppliesButton;
    [SerializeField] Button addSuppliesToFillButton;
    [SerializeField] TextMeshProUGUI add5SuppliesButtonText;
    [SerializeField] TextMeshProUGUI add50SuppliesButtonText;
    [SerializeField] TextMeshProUGUI addSuppliesToFillButtonText;

    RoomObject room => Game.Selection.Room;
    List<GameObject> listItems = new List<GameObject>();

    private void OnEnable()
    {
        if (Game.Selection.Room == null)
        {
            UI.ShowRoomInfo(false);
            return;
        }
        supplies.SetActive(room.RoomUsesSupplies);

        closeButton.onClick.AddListener(OnCloseButtonPress);
        UpdateUI();

        addFloorButton.interactable = false;
        destroyRoomButton.interactable = false;
        addPlaceableButton.onClick.AddListener(OnAddPlaceableButtonPress);

        add5SuppliesButton.onClick.AddListener(() => room.BuySupplies(5));
        add50SuppliesButton.onClick.AddListener(() => room.BuySupplies(50));
        addSuppliesToFillButton.onClick.AddListener(() => room.BuySupplies(room.RoomSuppliesMaximum - room.RoomSuppliesCurrent));
        UpdateSuppliesUI();
    }

    public void UpdateUI()
    {
        roomNameText.text = room.Config.Name;

        if (listItems.Count > 0)
        {
            foreach (GameObject item in listItems)
            {
                Destroy(item);
            }
            listItems.Clear();
        }

        foreach (PlaceableObject placeable in room.Placeables)
        {
            UI_RoomInfo_Placeable placeableInfoItem = Instantiate(placeablePrefab, content);
            listItems.Add(placeableInfoItem.gameObject);
            placeableInfoItem.Initilize(placeable);
        }
    }
    public void UpdateSuppliesUI()
    {
        if (room.RoomUsesSupplies)
        {
            suppliesBar.value = room.RoomSuppliesCurrent / room.RoomSuppliesMaximum;
            suppliesText.text = $"{room.RoomSuppliesCurrent:0}/{room.RoomSuppliesMaximum:0}";
            add5SuppliesButtonText.text = $"+5 Supplies (${room.CostPerSupply * 5})";
            add50SuppliesButtonText.text = $"+50 Supplies (${room.CostPerSupply * 50})";
            addSuppliesToFillButtonText.text = $"+{room.RoomSuppliesMaximum - room.RoomSuppliesCurrent} Supplies (${room.CostPerSupply * room.RoomSuppliesMaximum - room.RoomSuppliesCurrent})";
        }
    }

    private void OnDisable()
    {
        closeButton.onClick.RemoveAllListeners();
        addPlaceableButton.onClick.RemoveAllListeners();

        add5SuppliesButton.onClick.RemoveAllListeners();
        add50SuppliesButton.onClick.RemoveAllListeners();
        addSuppliesToFillButton.onClick.RemoveAllListeners();
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        UI.MouseOverUI = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UI.MouseOverUI = false;
    }

    private void OnCloseButtonPress()
    {
        Game.Selection.DeselectRoom();
        UI.MouseOverUI = false;
    }

    private void OnAddPlaceableButtonPress()
    {
        UI.ShowPlaceablesMenu();
    }
}