using System.Collections.Generic;
using UnityEngine;

public class ShipManager : MonoBehaviour
{
    [field: SerializeField] public float FlightLevel { get; private set; } = 10;
    [SerializeField] Ship[] shipPrefabs;
    [SerializeField] Vector3 spawnPoint;
    [field: SerializeField] public Vector3 DeparturePoint;

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

        Ship newShip = Instantiate(shipPrefab, spawnPoint, Quaternion.identity);
        newShip.Initilize(targetLandingPad);
        ships.Add(newShip);
        newShip.transform.parent = transform;
    }

    private Ship shipPrefab { get { return shipPrefabs[Random.Range(0, shipPrefabs.Length)]; } }

    public void ShipDespawn(Ship ship)
    {
        ships.Remove(ship);
    }
}
