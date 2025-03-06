using UnityEngine;

public class SMS_SittingIdle : StaffMemberState
{
    float crossFadeTime = 0.1f;

    public SMS_SittingIdle(StaffMember staffMember) : base(staffMember)
    {
        this.staffMember = staffMember;
    }

    public override void StateStart()
    {
        staffMember.Animator.CrossFade("Sitting Idle", crossFadeTime);
        staffMember.NavMeshAgent.enabled = false;
        staffMember.Rigidbody.useGravity = false;
        staffMember.transform.localPosition = Vector3.zero;
        staffMember.transform.localEulerAngles = Vector3.zero;
    }

    public override void StateTick()
    {
    }

    public override void StateEnd()
    {
    }

    public override string Status()
    {
        return "Idle";
    }
}
