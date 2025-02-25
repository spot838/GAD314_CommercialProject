public class StaffMember : Character
{
    public string Name { get; private set; }
    public int SomeStat { get; private set; } = 5;

    public void SetName(string newName)
    {
        Name = newName;
        name = newName;
    }

    private void OnDestroy()
    {
        Station.Staff.DespawnMember(this);
    }
}