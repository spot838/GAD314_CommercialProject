using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_DebugOutput : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Button backButton;

    private void OnEnable()
    {
        backButton.onClick.AddListener(OnBackButtonPress);
    }

    private void OnDisable()
    {
        backButton.onClick.RemoveListener(OnBackButtonPress);
    }

    private void OnBackButtonPress()
    {
        UI.ShowDebugInfo(false);
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
