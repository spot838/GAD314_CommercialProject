using UnityEngine;

// this manages the player's station money supply
public class StationMoney : MonoBehaviour
{
    [field: SerializeField] public int Amount { get; private set; } = 5000;

    private void Start()
    {
        UI.UpdateMoneyText();
    }
}
