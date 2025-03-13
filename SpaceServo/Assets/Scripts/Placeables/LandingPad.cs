using UnityEngine;

// where the ships land and customers will spawn out of

public class LandingPad : PlaceableObject
{
    [field: SerializeField] public Transform CustomerSpawnPoint {  get; private set; }
    [field: SerializeField] public float LandingLevel {  get; private set; }
    [field: SerializeField] public Ship CurrentShip;
    [field: SerializeField] public Refueling Refueling { get; private set; }
    [field: SerializeField] public UI_LandingPadRefuelIndicator Indicator;

    public bool IsAvailable => CurrentShip == null;
    public bool IsRefueling => Refueling.IsRefueling;

    public Vector3 ArivalPosition
    {
        get
        {
            Vector3 position = transform.position;
            position.y = Station.ShipManager.FlightLevel;
            return position;
        }
    }

    protected override void OnDestroy()
    {
        if (CurrentShip != null) Destroy(CurrentShip.gameObject);
        base.OnDestroy();
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);


        if (IsPlaced && CurrentShip != null && CurrentShip.Customer != null && other.TryGetComponent<Customer>(out Customer customer))
        {
            if (CurrentShip.Customer == customer && customer.HasRefueled)
            {
                customer.DestoryCustomer();
            }
        }
    }
}
