using UnityEngine;

public class MaterialRandomiser : MonoBehaviour
{
    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] Material[] materials;

    private void Start()
    {
        if (materials.Length == 0) return;
        meshRenderer.material = materials[Random.Range(0, materials.Length)];
    }
}
