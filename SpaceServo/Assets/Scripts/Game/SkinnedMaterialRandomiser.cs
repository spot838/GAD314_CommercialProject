using UnityEngine;

public class SkinnedMaterialRandomiser : MonoBehaviour
{
    [SerializeField] SkinnedMeshRenderer meshRenderer;
    [SerializeField] Material[] materials;

    private void Start()
    {
        if (materials.Length == 0) return;
        meshRenderer.material = materials[Random.Range(0, materials.Length)];
    }
}
