using NUnit.Framework.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_TopBar : MonoBehaviour
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
}
