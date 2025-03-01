using UnityEngine;

public class Refueling : MonoBehaviour
{
    LandingPad landingPad;
    [SerializeField] float refuelRate = 10;

    ShipFuel ship;

    private void Awake()
    {
        landingPad = GetComponentInParent<LandingPad>();
    }

    private void Update()
    {
        if (IsRefueling)
        {
            ship.Refuel(refuelRate * Time.deltaTime);

            if (ship.HasRefueled)
            {
                if (landingPad != null) landingPad.Indicator.gameObject.SetActive(false);
                ship = null;
            }
        }
    }

    public void BeginRefueling(ShipFuel shipFuel)
    {
        ship = shipFuel;
    }

    public bool IsRefueling => ship != null;
}
