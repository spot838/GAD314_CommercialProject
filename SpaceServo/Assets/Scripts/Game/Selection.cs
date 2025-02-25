using UnityEngine;

public class Selection : MonoBehaviour
{
    [field: SerializeField] public RoomObject Room { get; private set; }

    private bool selectionOverride => Game.FloorBuilder.IsPlacing || UI.IsRoomsMenuShowing || UI.IsPlaceablesMenuShowing || Game.PlaceableBuilder.IsPlacing;

    private void OnEnable()
    {
        Game.Input.OnPrimaryPress += OnPrimaryPress;
    }

    private void OnDisable()
    {
        Game.Input.OnPrimaryPress -= OnPrimaryPress;
    }

    public void SelectRoom(RoomObject room)
    {
        Room = room;
        UI.ShowRoomInfo();
    }

    public void DeselectRoom()
    {
        UI.ShowRoomInfo(false);
        Room = null;
    }

    private void OnPrimaryPress()
    {
        if (selectionOverride) return;

        if (Game.CameraController == null)
        {
            Debug.LogError("Game is missing Camera Controller reference");
            return;
        }
        if (UI.MouseOverUI)
        {
            //Debug.LogWarning("Mouse over UI");
            return;
        }

        //print("selection click");
        Ray ray = Game.CameraController.Camera.ScreenPointToRay(Game.Input.MousePosition);
        float rayCastDistance = Game.CameraController.DistanceToGround + 50;

        if (Physics.Raycast(ray, out RaycastHit hit, rayCastDistance, Game.RoomLayer))
        {
            //print("hit " + hit.collider.name);

            if (hit.collider.TryGetComponent<PlaceableObject>(out PlaceableObject placeableObject))
            {
                RoomObject room = placeableObject.GetComponentInParent<RoomObject>();
                if (room != null) SelectRoom(room);
            }

            if (hit.collider.TryGetComponent<FloorTile>(out FloorTile tile))
            {
                SelectRoom(tile.Room);
            }
        }
        else
        {
            //Debug.LogWarning("Did not select anything");
            if (Room != null)
            {
                DeselectRoom();
            }
        }
    }
}
