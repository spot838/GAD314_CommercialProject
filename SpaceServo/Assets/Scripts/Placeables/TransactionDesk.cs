using UnityEngine;

public class TransactionDesk : PlaceableObject
{
    [field: SerializeField] public Transform CustomerPosition {  get; private set; }
    [SerializeField] float transactionBaseTime = 0.5f;
    [field: SerializeField] public Customer CurrentCustomer;
    float timer;

    public bool IsAvailable => CurrentCustomer == null;

    protected override void Update()
    {
        base.Update();

        if (CurrentCustomer != null)
        {
            if (!CurrentCustomer.IsMoving && CurrentCustomer.HasArrivedAtNavMeshDestination && !CurrentCustomer.KeepIdle)
            {
                //print("begining transaction");
                CurrentCustomer.KeepIdle = true;
                timer = transactionBaseTime;
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

            
        }
    }
}
