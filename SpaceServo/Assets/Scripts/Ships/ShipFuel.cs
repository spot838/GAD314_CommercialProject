using UnityEngine;

public class ShipFuel : MonoBehaviour
{
    [field: SerializeField] public float Amount { get; private set; }
    [field: SerializeField] public float MaxAmount { get; private set; }
    [SerializeField] Vector2 initialRange;

    private void Start()
    {
        Amount = Random.Range(initialRange.x, initialRange.y);
    }

    public void Refuel(float refuelAmount)
    {
        Amount = Mathf.Clamp(Amount + refuelAmount, 0, MaxAmount);
    }

    public bool HasRefueled => Amount == MaxAmount;

    public float Percentage => Amount / MaxAmount;
}
