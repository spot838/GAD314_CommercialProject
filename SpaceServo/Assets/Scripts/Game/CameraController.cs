using UnityEngine;

// controls the camera

// TODO: paning when mouse is on screen edge
// TODO: paning on while holding right mouse button

public class CameraController : MonoBehaviour
{
    [field: SerializeField] public Camera Camera { get; private set; }
    [field: SerializeField] public float DistanceToGround { get; private set; }
    [SerializeField] float angle;
    [SerializeField, Tooltip("Units/Second")] float panSpeed; 

    void Update()
    {
        KeyboardPan();
        SetCameraPosition();
    }

    private void SetCameraPosition()
    {
        float y = Mathf.Cos(angle) * DistanceToGround; 
        float z = Mathf.Sin(angle) * DistanceToGround;
        Camera.transform.localPosition = new Vector3(0, y, z);
        Camera.transform.localEulerAngles = new Vector3(angle, 0, 0);
    }

    private void KeyboardPan()
    {
        if (Game.Input.CameraMove.magnitude > 0)
        {
            Vector3 position = transform.position;
            position.x += Game.Input.CameraMove.x * panSpeed * Time.deltaTime;
            position.z += Game.Input.CameraMove.y * panSpeed * Time.deltaTime;
            transform.position = position;
        }
    }
}
