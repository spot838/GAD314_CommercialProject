using UnityEngine;
using UnityEngine.UI;

public class UI_LandingPadRefuelIndicator : MonoBehaviour
{
    LandingPad landingPad;
    [SerializeField] Image circle;

    private void Awake()
    {
        landingPad = GetComponentInParent<LandingPad>();
    }

    private void Update()
    {
        if (circle == null || landingPad == null || landingPad.CurrentShip == null) return;

        circle.fillAmount = landingPad.CurrentShip.Fuel.Percentage;
    }
}
