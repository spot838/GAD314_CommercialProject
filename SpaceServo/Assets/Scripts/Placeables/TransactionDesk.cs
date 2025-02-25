using UnityEngine;

public class TransactionDesk : PlaceableObject
{
    [field: SerializeField] public Transform CustomerPosition {  get; private set; }
    [field: SerializeField] public Transform StaffPosition { get; private set; }
    [field: SerializeField] public float TransactionBaseTime { get; private set; } = 1.5f;
    [field: SerializeField] public Customer CurrentCustomer;
    [field: SerializeField] public StaffMember StaffMember;
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

    public override void SetPlaced()
    {
        base.SetPlaced();

        // for now we'll just spawn in the staff member when placement occurs
        StaffMember = Station.Staff.SpawnNew(StaffPosition);
        //StaffMember.transform.parent = StaffPosition;
        StaffMember.SetNewState(new SMS_SittingIdle(StaffMember));
    }

    public void BeginTransaction()
    {
        CurrentCustomer.SetNewState(new CS_CompletingTransaction(CurrentCustomer, this));
        StaffMember.SetNewState(new SMS_SittingTyping(StaffMember));
    }

    public void CompleteTransaction()
    {
        // apply customer stat changes here

        CurrentCustomer.HasBoughtFuel = true; // TODO: this will be replaced once fuel system is in

        CurrentCustomer.SetNewState(new CS_Idle(CurrentCustomer));
        StaffMember.SetNewState(new SMS_SittingIdle(StaffMember));

        CurrentCustomer = null;
    }
}


