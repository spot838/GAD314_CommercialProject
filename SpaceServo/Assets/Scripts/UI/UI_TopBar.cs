using NUnit.Framework.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_TopBar : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] TMP_Text moneyText;
    [SerializeField] TMP_Text ratingText;
    [SerializeField] Button buildFloorButton;
    [SerializeField] Button placeObjectButton;

    private void OnEnable()
    {
        buildFloorButton.onClick.AddListener(OnBuildFloorButtonPress);
        placeObjectButton.onClick.AddListener(OnPlaceObjectButtonPress);
    }

    private void OnDisable()
    {
        buildFloorButton.onClick.RemoveListener(OnBuildFloorButtonPress);
        placeObjectButton.onClick.RemoveListener(OnPlaceObjectButtonPress);
    }

    private void OnBuildFloorButtonPress()
    {
        Debug.Log("Build Floor button pressed");

        Game.FloorBuilder.StartPlacingFloor();
    }

    private void OnPlaceObjectButtonPress()
    {
        Debug.Log("Place Object button pressed");
    }

    public void UpdateMoneyText()
    {
        moneyText.text = "$" + Game.Money.Amount.ToString();
    }

    public void UpdateRaitingText()
    {
        ratingText.text = Game.Rating.Raiting.ToString();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UI.MouseOverUI = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UI.MouseOverUI = false;
    }
}
