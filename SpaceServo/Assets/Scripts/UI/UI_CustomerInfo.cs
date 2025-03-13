using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_CustomerInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Button closeButton;
    [SerializeField] TMP_Text customerName;
    [SerializeField] TMP_Text customerStatus;
    [SerializeField] TMP_Text shipStatus;
    [SerializeField] Slider ratingSlider;
    [SerializeField] RawImage customerFootage;

    Customer customer => Game.Selection.Customer;

    private void OnEnable()
    {
        if (customer == null)
        {
            UI.ShowCustomerInfo(false);
            return;
        }

        closeButton.onClick.AddListener(OnCloseButtonPress);

        customerName.text = customer.name;
    }

    private void OnDisable()
    {
        closeButton.onClick.RemoveListener(OnCloseButtonPress);
    }

    private void Update()
    {
        if (customer == null) return;

        UpdateRating();
        UpdateCustomerStatus();
        UpdateShipStatus();
    }

    private void OnCloseButtonPress()
    {
        Game.Selection.DeselectCustomer();
    }

    private void UpdateRating()
    {
        ratingSlider.value = customer.Info.Satisfaction / Station.Rating.MAX_RATING;
    }

    private void UpdateCustomerStatus()
    {
        customerStatus.text = customer.State.Status();
    }

    private void UpdateShipStatus()
    {
        if (customer.Ship.Fuel.HasRefueled)
        {
            shipStatus.text = "Refuel Compelete";
        }
        else if (!customer.Ship.LandingPad.IsRefueling)
        {
            shipStatus.text = "Awaiting Refuel";
        }
        else
        {
            shipStatus.text = (customer.Ship.Fuel.Percentage * 100).ToString("f0") + "% refueled";
        }
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
