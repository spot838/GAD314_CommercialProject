using UnityEngine;

// this manages the player's station money supply
public class StationMoney : MonoBehaviour
{
    [field: SerializeField] public int Amount { get; private set; } = 5000;

    private void Start()
    {
        UI.UpdateMoneyText();
    }

    public void Add(int amount)
    {
        this.Amount += amount;
        UI.UpdateMoneyText();
    }

    public void Remove(int amount)
    {
        this.Amount = Mathf.Clamp(this.Amount - amount, 0, int.MaxValue);
        UI.UpdateMoneyText();
    }

    public bool CanAfford(int amount)
    {
        return this.Amount - amount >= 0;
    }
}
