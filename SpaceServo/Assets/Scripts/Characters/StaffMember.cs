public class StaffMember : Character
{
    private void OnDestroy()
    {
        Station.Staff.DespawnMember(this);
    }
}