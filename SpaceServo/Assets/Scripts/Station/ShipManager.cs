using System.Collections.Generic;
using UnityEngine;

public class ShipManager : MonoBehaviour
{
    [field: SerializeField] public float FlightLevel { get; private set; } = 10;
    [SerializeField] Ship[] shipPrefabs;
    [SerializeField] float spawnDepartureDistance = 500.0f;

    List<Ship> ships = new List<Ship>();

    private void Update()
    {
        if (TryGetAvailableLandingPad(out LandingPad landingPad))
        {
            SpawnNewShip(landingPad);
        }
    }

    private bool TryGetAvailableLandingPad(out LandingPad landingPad)
    {
        foreach (LandingPad pad in Station.LandingPads)
        {
            if (pad.IsAvailable)
            {
                landingPad = pad;
                return true;
            }
        }

        landingPad = null;
        return false;
    }

    private void SpawnNewShip(LandingPad targetLandingPad)
    {
        if (shipPrefabs.Length == 0) return;

        Ship newShip = Instantiate(shipPrefab, RandomPoint(targetLandingPad.transform.position), Quaternion.identity);
        newShip.Initilize(targetLandingPad);
        ships.Add(newShip);
        newShip.transform.parent = transform;
    }

    private Ship shipPrefab { get { return shipPrefabs[Random.Range(0, shipPrefabs.Length)]; } }

    public void ShipDespawn(Ship ship)
    {
        ships.Remove(ship);
    }

    public Vector3 RandomPoint(Vector3 origin)
    {
        float angle = Random.Range(0.0f, 360.0f);
        float x = origin.x + spawnDepartureDistance * Mathf.Cos(angle * Mathf.Deg2Rad);
        float z = origin.z + spawnDepartureDistance * Mathf.Sin(angle * Mathf.Deg2Rad);

        return new Vector3( x, 20.0f, z);
    }
}
