using UnityEngine;

// where the ships land and customers will spawn out of

public class LandingPad : PlaceableObject
{
    [field: SerializeField] public Transform CustomerSpawnPoint {  get; private set; }
    [field: SerializeField] public float LandingLevel {  get; private set; }
    [field: SerializeField] public Ship CurrentShip;

    public bool IsAvailable => CurrentShip == null;

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
        Destroy(CurrentShip.gameObject);
        base.OnDestroy();
    }
}
