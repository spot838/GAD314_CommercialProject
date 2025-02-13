using UnityEngine;

// this is the physical placeable object, this contains functionality of the placeable object, use this as the mother class for the different placeable objects, this script itself just holds the functionality to place it in the world first how it works after placement should be in a child script

public class PlaceableObject : MonoBehaviour
{
    [SerializeField] Material validPlacementMat;
    [SerializeField] Material invalidPlacementMat;
    [SerializeField] Material placedMat;
    [SerializeField] MeshRenderer[] meshes;

    public bool IsPlaced;

    public bool HasValidPlacement;

    private void Start()
    {
        SetInvalidPlacementMaterial();
    }

    private void Update()
    {
        if (!IsPlaced)
        {
            if (ValidPlacement) SetValidPlacementMaterial();
            else SetInvalidPlacementMaterial();
        }
    }

    public void SetValidPlacementMaterial()
    {
        foreach (MeshRenderer mesh in meshes)
        {
            mesh.material = validPlacementMat;
        }
        HasValidPlacement = true;
    }

    public void SetInvalidPlacementMaterial()
    {
        foreach (MeshRenderer mesh in meshes)
        {
            mesh.material = invalidPlacementMat;
        }
        HasValidPlacement = false;
    }

    public void SetPlaced()
    {
        foreach (MeshRenderer mesh in meshes)
        {
            mesh.material = placedMat;
        }
        IsPlaced = true;
    }

    private bool ValidPlacement
    {
        get
        {
            Vector3 origin = transform.position;
            origin.y += 5;
            Vector3 direction = new Vector3(0, -1, 0);
            RaycastHit[] hits = Physics.RaycastAll(origin, direction, 10);
            //print(hits.Length + " hits");
            foreach (RaycastHit hit in hits)
            {
                //print("hit " + hit.collider.name);
                FloorTile floorTile = hit.collider.GetComponent<FloorTile>();
                if (floorTile != null) return true;
            }
            
            return false;
        }
    }
}
