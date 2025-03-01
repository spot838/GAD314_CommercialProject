using Unity.AI.Navigation;
using UnityEngine;

public class FloorTile : MonoBehaviour
{
    [SerializeField] Material building; // the material used while the player is placing the floor
    [SerializeField] Material built; // the material once the player has confirmed placement, what it normally looks like
    [SerializeField] MeshRenderer[] meshes;
    [SerializeField] GameObject wallTop;
    [SerializeField] GameObject wallBottom;
    [SerializeField] GameObject wallLeft;
    [SerializeField] GameObject wallRight;
    [SerializeField] GameObject pillerTopLeft;
    [SerializeField] GameObject pillerTopRight;
    [SerializeField] GameObject pillerBottomLeft;
    [SerializeField] GameObject pillerBottomRight;

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

    public void RemoveWalls()
    {
        wallTop.SetActive(false);
        wallBottom.SetActive(false);
        wallLeft.SetActive(false);
        wallRight.SetActive(false);

        pillerTopLeft.SetActive(false);
        pillerBottomLeft.SetActive(false);
        pillerBottomRight.SetActive(false);
        pillerTopRight.SetActive(false);
    }

    public void AddTopWall()
    {
        wallTop.SetActive(true);
    }

    public void AddBottomWall()
    {
        wallBottom.SetActive(true);
    }

    public void AddLeftWall()
    {
        wallLeft.SetActive(true);
    }

    public void AddRightWall()
    {
        wallRight.SetActive(true);
    }

    public void ApplyPillers()
    {
        if (wallTop.activeSelf && wallLeft.activeSelf)
        {
            pillerTopLeft.SetActive(true);
        }

        if (wallBottom.activeSelf && wallLeft.activeSelf)
        {
            pillerBottomLeft.SetActive(true);
        }

        if (wallBottom.activeSelf && wallRight.activeSelf)
        {
            pillerBottomRight.SetActive(true);
        }

        if (wallTop.activeSelf && wallRight.activeSelf)
        {
            pillerTopRight.SetActive(true);
        }
    }
}
