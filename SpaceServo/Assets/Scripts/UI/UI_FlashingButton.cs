using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class UI_FlashingButton : MonoBehaviour
{
    [SerializeField] Color flashingColor = Color.yellow;
    [SerializeField] float flashLength = 1.0f;
    Image buttonImage;
    RawImage buttonRawImage;
    Color baseColor;

    float lastChange;
    bool flashing;
    float flashingProgress => Time.time - lastChange;

    private void Awake()
    {
        if (buttonImage == null) buttonImage = GetComponent<Image>();
        if (buttonRawImage == null) buttonRawImage = GetComponent<RawImage>();
        if (buttonImage == null && buttonRawImage == null) Debug.LogError(name + " missing Image to change color");
    }

    private void Start()
    {
        if (buttonRawImage != null) baseColor = buttonRawImage.color;
        else if (buttonImage != null) baseColor = buttonImage.color;
    }

    private void Update()
    {
        if (flashing)
        {
            if (Time.time > lastChange + flashLength)
                ToggleFlash();
        }
    }

    public void StartFlashing()
    {
        flashing = true;
        ToggleFlash();
    }

    public void StopFlashing()
    {
        flashing = false;
        if (buttonRawImage != null) buttonRawImage.color = baseColor;
        else if (buttonImage != null) buttonImage.color = baseColor;
    }

    private void ToggleFlash()
    {
        lastChange = Time.time;

        if (buttonRawImage != null)
        {
            if (buttonRawImage.color == baseColor)
                buttonRawImage.color = flashingColor;

            else
                buttonRawImage.color = baseColor;
        }

        else if (buttonImage != null)
        {
            if (buttonImage.color == baseColor)
                buttonImage.color = flashingColor;

            else
                buttonImage.color = baseColor;
        }
    }
}
