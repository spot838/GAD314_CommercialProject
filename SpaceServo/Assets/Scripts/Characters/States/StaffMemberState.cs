using UnityEngine;

public abstract class StaffMemberState : CharacterState
{
    protected StaffMember staffMember;

    public StaffMemberState(StaffMember staffMember)
    {
        this.staffMember = staffMember;
    }
}
