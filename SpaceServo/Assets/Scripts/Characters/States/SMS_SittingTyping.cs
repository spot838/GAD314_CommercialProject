using UnityEngine;

public class SMS_SittingTyping : StaffMemberState
{
    float crossFadeTime = 0.1f;

    public SMS_SittingTyping(StaffMember staffMember) : base(staffMember)
    {
        this.staffMember = staffMember;
    }

    public override void StateStart()
    {
        staffMember.Animator.CrossFade("Sitting Typing", crossFadeTime);
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
        return "Working";
    }
}
