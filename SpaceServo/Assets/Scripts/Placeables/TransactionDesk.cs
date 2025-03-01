using UnityEngine;

public class TransactionDesk : PlaceableObject
{
    [field: SerializeField] public Transform CustomerPosition {  get; private set; }
    [field: SerializeField] public Transform StaffPosition { get; private set; }
    [field: SerializeField] public float TransactionBaseTime { get; private set; } = 1.5f;
    [field: SerializeField] public Customer CurrentCustomer;
    [field: SerializeField] public StaffMember StaffMember;
    float timer;

    public bool IsAvailable => StaffMember != null && CurrentCustomer == null;

    protected override void Update()
    {
        base.Update();
    }

    public override void SetPlaced()
    {
        base.SetPlaced();
    }

    public void HireStaffMember()
    {
        StaffMember = Station.Staff.SpawnNew(StaffPosition);
        StaffMember.SetNewState(new SMS_SittingIdle(StaffMember));
        UI.UpdateRoomInfo();
    }

    public void BeginTransaction()
    {
        CurrentCustomer.SetNewState(new CS_CompletingTransaction(CurrentCustomer, this));
        StaffMember.SetNewState(new SMS_SittingTyping(StaffMember));
    }

    public void CompleteTransaction()
    {
        // apply customer stat changes here

        if (Room.Config.Type == global::Room.EType.FuelPurchase) CurrentCustomer.Ship.BeginRefueling();

        CurrentCustomer.SetNewState(new CS_Idle(CurrentCustomer));
        StaffMember.SetNewState(new SMS_SittingIdle(StaffMember));

        CurrentCustomer = null;
    }
}


