using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_DebugOutput : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Button backButton;
    [SerializeField] Button copyButton;
    [SerializeField] Button saveButton;
    [SerializeField] TMP_InputField outputField;

    private void OnEnable()
    {
        backButton.onClick.AddListener(OnBackButtonPress);
        copyButton.onClick.AddListener(OnCopyButtonPress);
        saveButton.onClick.AddListener(OnSaveButtonPress);

        outputField.text = "";
        foreach (string outputString in Game.Debug.Output)
        {
            outputField.text += outputString + "\n";
        }
    }

    private void OnDisable()
    {
        backButton.onClick.RemoveListener(OnBackButtonPress);
        copyButton.onClick.RemoveListener(OnCopyButtonPress);
        saveButton.onClick.RemoveListener(OnSaveButtonPress);
    }

    private void OnBackButtonPress()
    {
        UI.ShowDebugInfo(false);
    }

    private void OnCopyButtonPress()
    {
        GUIUtility.systemCopyBuffer = outputField.text;
    }

    private void OnSaveButtonPress()
    {
        // TODO: Output to text file placed on desktop
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
