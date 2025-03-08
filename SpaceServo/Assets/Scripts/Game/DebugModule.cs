using System.Collections.Generic;
using UnityEngine;

public class DebugModule : MonoBehaviour
{
    public int MoneySpent = 0;
    public int MoneyEarned = 0;
    public int Customers { get; private set; } = 0;
    public Dictionary<Placeable, int> PlaceablesBuilt { get; set; } = new Dictionary<Placeable, int>();
    public Dictionary<Room, int> RoomsBuilt { get; set; } = new Dictionary<Room, int>();
    public Dictionary<RoomObject, int> RoomCosts { get; set; } = new Dictionary<RoomObject, int>();

    private void OnEnable()
    {
        Station.CustomerManager.OnCustomerDeparted += IncrementCustomers;
    }

    private void OnDisable()
    {
        Station.CustomerManager.OnCustomerDeparted -= IncrementCustomers;
    }

    private void IncrementCustomers(Customer customer)
    {
        Customers++;
    }

    public string[] Output
    {
        get
        {
            List<string> strings = new List<string>();

            strings.Add("===DEBUG_START===");
            strings.Add("PlaySessionLength: " + (Time.time/60).ToString("f2") + " minutes");
            strings.Add("StationRating: " + Station.Rating.Value.ToString("f0") + " / " + Station.Rating.MAX_RATING);

            strings.Add("\n ===Placeables===");
            int placeablesTotal = 0;
            int placeablesSpendTotal = 0;
            foreach (KeyValuePair<Placeable, int> placeableBuilt in PlaceablesBuilt)
            {
                strings.Add(placeableBuilt.Key.Name + ": " + placeableBuilt.Value + " $" + (placeableBuilt.Key.Cost * placeableBuilt.Value));
                placeablesTotal += placeableBuilt.Value;
                placeablesSpendTotal += placeableBuilt.Key.Cost * placeableBuilt.Value;
            }
            strings.Add("TOTAL_BUILT: " + placeablesTotal);
            strings.Add("TOATL_COST: $" + placeablesSpendTotal);

            strings.Add("\n ===Rooms===");
            int roomsTotal = 0;
            foreach (KeyValuePair<Room, int> roomBuilt in RoomsBuilt)
            {
                strings.Add(roomBuilt.Key.Name + ": " + roomBuilt.Value);
                roomsTotal += roomBuilt.Value;
            }
            strings.Add("TOTAL_BUILT: " + roomsTotal);
            

            strings.Add("\n ===Room Costs===");
            int roomCostTotal = 0;
            foreach (KeyValuePair<RoomObject, int> roomBuilt in RoomCosts)
            {
                strings.Add(roomBuilt.Key.Config.Name + ": $" + roomBuilt.Value);
                roomCostTotal += roomBuilt.Value;
            }
            strings.Add("TOATL_COST: $" + roomCostTotal);

            strings.Add("\n ===Money===");
            strings.Add("Starting: $" + ((Station.Money.Amount + MoneySpent) - MoneyEarned));
            strings.Add("Earned: $" + MoneyEarned);
            strings.Add("Spent: $" + MoneySpent);
            strings.Add("Current Balance: $" + Station.Money.Amount);

            strings.Add("\n ===Customers===");
            int onStation = 0;
            foreach (LandingPad pad in Station.LandingPads)
            {
                if (pad.CurrentShip != null && pad.CurrentShip.Customer != null) onStation++;
            }
            strings.Add("OnStation: " + onStation);
            strings.Add("Departed: " + Customers);
            
            strings.Add("\n ===DEBUG_END===");
            return strings.ToArray();
        }
    }
}
