using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// controlls placing floors in the station

// TODO: walls, cost, input error checking

public class StationFloorBuilder : MonoBehaviour
{
    [SerializeField] Vector2 tileSize = new Vector2(5,5);
    [SerializeField] MeshRenderer plane; // this is something for raycast to hit
    [SerializeField] RoomObject roomPrefab;
    [field: SerializeField] public Room[] RoomConfigs {  get; private set; }

    //bool placing;
    Vector3 topLeftPoint;
    Vector3 botttomRightPoint;
    FloorTile firstTile;
    [SerializeField] List<FloorTile> currentPlacement = new List<FloorTile>();

    RoomObject currentRoom;
    public bool IsPlacing => currentRoom != null;

    private void Update()
    {

        if (currentRoom == null || UI.MouseOverUI) return;

        if (!Game.Input.PrimaryButtonDown) // player is indicating where the floor starts
        {
            topLeftPoint = RoundToNearestGrid(GroundLocationUnderMouse);
            if (topLeftPoint.y >= 500) return;

            if (firstTile == null )
            {
                firstTile = Instantiate(currentRoom.Config.FloorTilePrefab, topLeftPoint, Quaternion.identity);
                firstTile.SwitchToBuildingMaterial();
            }

            firstTile.transform.position = topLeftPoint;
        }
        else // player has indicated where the floor starts already
        {
            Vector3 currentGrid = RoundToNearestGrid(GroundLocationUnderMouse);
            if (currentGrid.y >= 500) return;
            if (currentGrid != botttomRightPoint)
            {
                botttomRightPoint = currentGrid;

                foreach (FloorTile tile in currentPlacement.ToArray())
                {
                    //print("Destroying " + tile.transform.position.ToString());
                    Destroy(tile.gameObject);
                }
                currentPlacement.Clear();

                int columns;
                int rows;

                if (botttomRightPoint.x > topLeftPoint.x)
                    columns = (int)((botttomRightPoint.x - topLeftPoint.x) / tileSize.x);
                else
                    columns = (int)((topLeftPoint.x - botttomRightPoint.x) / tileSize.x);

                if (botttomRightPoint.z < topLeftPoint.z)
                    rows = (int)((topLeftPoint.z - botttomRightPoint.z) / tileSize.y);
                else
                    rows = (int)((botttomRightPoint.z - topLeftPoint.z) / tileSize.y);

                for (int currentCol = 0;  currentCol <= columns; currentCol++)
                {
                    for (int currentRow = 0;currentRow <= rows; currentRow++)
                    {
                        if (currentCol == 0 && currentRow == 0) continue;

                        Vector3 position = firstTile.transform.position;
                        if (botttomRightPoint.x > topLeftPoint.x) position.x += tileSize.x * currentCol;
                        else position.x -= tileSize.x * currentCol;
                        if (botttomRightPoint.z < topLeftPoint.z) position.z -= tileSize.y * currentRow;
                        else position.z += tileSize.y * currentRow;

                        FloorTile newTile = Instantiate(currentRoom.Config.FloorTilePrefab, position, Quaternion.identity);
                        newTile.SwitchToBuildingMaterial();
                        currentPlacement.Add(newTile);
                    }
                }
            }
        }
    }

    public void StartPlacingFloor(Room config)
    {
        //placing = true;
        Game.Input.OnPrimaryPress += PlaceFirstTile;
        Game.Input.OnSecondaryPress += CancelPlacement;

        currentRoom = Instantiate(roomPrefab, Vector3.zero, Quaternion.identity);
        currentRoom.Initialize(config);
        Game.Selection.SelectRoom(currentRoom);
    }

    private Vector3 GroundLocationUnderMouse
    {
        get
        {
            if (Game.CameraController == null)
            {
                Debug.LogError("Game is missing Camera Controller reference");
                return Vector3.zero;
            }

            Ray ray = Game.CameraController.Camera.ScreenPointToRay(Game.Input.MousePosition);
            float rayCastDistance = Game.CameraController.DistanceToGround + 50;

            if (Physics.Raycast(ray, out RaycastHit hit, rayCastDistance, Game.StationLayer))
            {
                return hit.point;
            }

            if (UI.MouseOverUI) Debug.LogWarning("Mouse over UI");
            //else Debug.LogWarning("RayCast fail, no ground position under mouse");
            return new Vector3(0, 500, 0);
        }
    }

    private Vector3 RoundToNearestGrid(Vector3 location)
    {
        float x = Mathf.Round(location.x / tileSize.x) * tileSize.x;
        float z = Mathf.Round(location.z / tileSize.y) * tileSize.y;
        return new Vector3(x, location.y, z);
    }

    private void PlaceFirstTile()
    {
        Game.Input.OnPrimaryPress -= PlaceFirstTile;
        Game.Input.OnPrimaryRelease += CompletePlacement;
        botttomRightPoint = topLeftPoint;
    }

    private void CompletePlacement()
    {
        Game.Input.OnPrimaryRelease -= CompletePlacement;


        if (PlacementIsValid)
        {
            Game.Input.OnSecondaryPress -= CancelPlacement;

            if (Station.NavMeshSurface == null)
            {
                firstTile.NavMeshSurface.enabled = true;
                Station.SetNavMeshSurface(firstTile.NavMeshSurface);
                Station.NavMeshSurface.BuildNavMesh();
            }
            else
                Station.NavMeshSurface.BuildNavMesh();

            firstTile.SwitchToBuitMaterial();
            //firstTile.transform.parent = Station.Instance.transform;
            currentRoom.AddFloorTile(firstTile);
            firstTile = null;

            foreach (FloorTile tile in currentPlacement)
            {
                tile.SwitchToBuitMaterial();
                //tile.transform.parent = Station.Instance.transform;
                currentRoom.AddFloorTile(tile);
            }
            currentPlacement.Clear();

            Station.AddRoom(currentRoom);
            //Game.Selection.DeselectRoom();
            currentRoom = null;
            //placing = false;
        }

        else
        {
            if (firstTile != null)
            {
                Destroy(firstTile.gameObject);
                firstTile = null;
            }

            foreach (FloorTile tile in currentPlacement.ToArray())
            {
                Destroy(tile.gameObject);
            }
            currentPlacement.Clear();

            Game.Input.OnPrimaryPress += PlaceFirstTile;
        }
        
    }

    private void CancelPlacement()
    {
        Game.Input.OnSecondaryPress -= CancelPlacement;

        if (firstTile != null)
        {
            Destroy(firstTile.gameObject);
            firstTile = null;
        }

        if (Game.Input.PrimaryButtonDown)
            Game.Input.OnPrimaryRelease -= CompletePlacement;
        else
            Game.Input.OnPrimaryPress -= PlaceFirstTile;

        foreach (FloorTile tile in currentPlacement.ToArray())
            Destroy(tile.gameObject);
        currentPlacement.Clear();

        Game.Selection.DeselectRoom();
        Destroy(currentRoom.gameObject);
        currentRoom = null;
        //placing = false;
    }

    private List<RoomObject> RoomsTouchingTile(FloorTile tile)
    {
        List<RoomObject> list = new List<RoomObject>();

        Vector3 up = tile.transform.position;
        up.z += tileSize.y;
        FloorTile upTile = Station.TileAtLocation(up);
        if (upTile != null && upTile.Room != null)
            list.Add(upTile.Room);

        Vector3 down = tile.transform.position;
        down.z -= tileSize.y;
        FloorTile downTile = Station.TileAtLocation(down);
        if (downTile != null && downTile.Room != null)
            list.Add(downTile.Room);

        Vector3 left = tile.transform.position;
        left.x -= tileSize.x;
        FloorTile leftTile = Station.TileAtLocation(left);
        if (leftTile != null && leftTile.Room != null)
            list.Add(leftTile.Room);

        Vector3 right = tile.transform.position;
        right.x += tileSize.x;
        FloorTile rightTile = Station.TileAtLocation(right);
        if (rightTile != null && rightTile.Room != null)
            list.Add(rightTile.Room);

        return list;
    }

    private List<RoomObject> roomsTouchingPlacement
    {
        get
        {
            List<RoomObject> list = new List<RoomObject>();

            foreach (RoomObject room in RoomsTouchingTile(firstTile).ToArray())
            {
                if (!list.Contains(room)) list.Add(room);
            }

            foreach (FloorTile tile in currentPlacement)
            {
                foreach (RoomObject room in RoomsTouchingTile(tile).ToArray())
                {
                    if (!list.Contains(room)) list.Add(room);
                }
            }

            return list;
        }
    }

    private bool PlacementIsValid
    {
        get
        {
            if (firstTile == null || currentPlacement.Count == 0) return false;

            // TODO: add size requirement

            // check current Placement is not overlapping
            if (Station.TileAtLocation(firstTile.transform.position))
            {
                Debug.LogWarning("First tile overlapping");
                return false;
            }

            foreach (FloorTile tile in currentPlacement.ToArray())
            {
                if (Station.TileAtLocation(tile.transform.position))
                {
                    Debug.LogWarning("Placement overlapping");
                    return false;
                }
                    
            }


            // check that room is touching hallway
            if (Station.HasRooms) // if the station has rooms check that current placement is touching a hallway or the currently-being-placed hallway is touching another room
            {
                if (currentRoom.Config.Type != Room.EType.Hallway) // not placing a hallway
                {
                    bool containsAHallway = false;

                    foreach (RoomObject room in roomsTouchingPlacement)
                    {
                        if (room.Config.Type == Room.EType.Hallway)
                        {
                            containsAHallway = true;
                        }
                    }

                    if (!containsAHallway)
                    {
                        Debug.LogWarning("Placement not touching hallway");
                        return false;
                    }
                }
                else // placing a hallway
                {
                    bool touchingSomeOtherRoom = false;

                    foreach (RoomObject room in roomsTouchingPlacement)
                    {
                        if (room.Config.Type != Room.EType.Hallway)
                            touchingSomeOtherRoom = true;
                    }

                    if (!touchingSomeOtherRoom)
                    {
                        Debug.LogWarning("Hallway not touching any rooms");
                        return false;
                    }
                }
            }

            return true;
        }
    }

    private void AddWallsToPlacement()
    {

    }

    private void AddWallsToTile(FloorTile tile)
    {
        tile.RemoveWalls();

        Vector3 up = tile.transform.position;
        up.z += tileSize.y;
        FloorTile upTile = Station.TileAtLocation(up);
        if (upTile == null)
            tile.AddTopWall();

        Vector3 down = tile.transform.position;
        down.z -= tileSize.y;
        FloorTile downTile = Station.TileAtLocation(down);
        if (downTile == null)
            tile.AddBottomWall();

        Vector3 left = tile.transform.position;
        left.x -= tileSize.x;
        FloorTile leftTile = Station.TileAtLocation(left);
        if (leftTile == null)
            tile.AddLeftWall();

        Vector3 right = tile.transform.position;
        right.x += tileSize.x;
        FloorTile rightTile = Station.TileAtLocation(right);
        if (rightTile == null)
            tile.AddRightWall();

        tile.ApplyPillers();
    }
}
