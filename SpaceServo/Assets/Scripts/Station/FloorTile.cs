using Unity.AI.Navigation;
using UnityEngine;

public class FloorTile : MonoBehaviour
{
    [SerializeField] Material building; // the material used while the player is placing the floor
    [SerializeField] Material built; // the material once the player has confirmed placement, what it normally looks like
    [SerializeField] MeshRenderer[] meshes;
    [field: SerializeField] public NavMeshSurface NavMeshSurface {  get; private set; }
    public RoomObject Room {  get; private set; }

    public void SetRoom(RoomObject room)
    {
        this.Room = room;
    }


    public void SwitchToBuildingMaterial()
    {
        foreach (MeshRenderer meshRenderer in meshes)
        {
            meshRenderer.material = building;
        }
    }

    public void SwitchToBuitMaterial()
    {
        foreach (MeshRenderer meshRenderer in meshes)
        {
            meshRenderer.material = built;
        }
    }

    private void OnDestroy()
    {
        Room?.RemoveTile(this);
    }
}
