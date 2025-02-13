using UnityEngine;

// controls the camera

// TODO: paning when mouse is on screen edge
// TODO: paning on while holding right mouse button

public class CameraController : MonoBehaviour
{
    [SerializeField] Camera _camera;
    [SerializeField] float distanceToGround;
    [SerializeField] float angle;
    [SerializeField, Tooltip("Units/Second")] float panSpeed; 


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        KeyboardPan();
        SetCameraPosition();
    }

    private void SetCameraPosition()
    {
        float y = Mathf.Cos(angle) * distanceToGround; 
        float z = Mathf.Sin(angle) * distanceToGround;
        _camera.transform.localPosition = new Vector3(0, y, z);
        _camera.transform.localEulerAngles = new Vector3(angle, 0, 0);
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
