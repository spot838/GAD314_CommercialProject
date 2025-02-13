using UnityEngine;

public class Station : MonoBehaviour
{
    public static Station Instance;

    public static GameObject Object => Instance.gameObject;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }
}
