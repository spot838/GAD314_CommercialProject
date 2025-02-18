using UnityEngine;

public class TransactionDesk : PlaceableObject
{
    [field: SerializeField] public Transform CustomerPosition {  get; private set; }
    [field: SerializeField] public float TransactionBaseTime { get; private set; } = 1.5f;
    [field: SerializeField] public Customer CurrentCustomer;
    float timer;

    public bool IsAvailable => CurrentCustomer == null;

    protected override void Update()
    {
        base.Update();

        /*if (CurrentCustomer != null)
        {
            if (!CurrentCustomer.IsMoving && CurrentCustomer.ArrivedAtDestination && !CurrentCustomer.KeepIdle)
            {
                //print("begining transaction");
                CurrentCustomer.KeepIdle = true;
                timer = TransactionBaseTime;
                CurrentCustomer.transform.rotation = CustomerPosition.rotation;
            }

            else if (CurrentCustomer.KeepIdle)
            {
                if (timer >= 0)
                {
                    timer -= Time.deltaTime;
                    //print(timer);
                }
                else
                {
                    CurrentCustomer.CompleteFuelPurchase();
                    CurrentCustomer = null;
                    //print("transaction complete");
                }
            }

            
        }*/
    }
}
