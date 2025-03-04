using System;
using System.Collections.Generic;
using UnityEngine;

// controlls placing floors in the station

// TODO: walls, cost, input error checking

public class StationFloorBuilder : MonoBehaviour
{
    [field: SerializeField] public Vector2 TileSize = new Vector2(5,5);
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
    private bool placingDoor;
    private FloorTile possibleDoorTile;
    private FloorTile possibleOtherDoorTile; // TODO: preview doorway building iteration

    private void Update()
    {

        if (currentRoom == null || UI.MouseOverUI) return;

        if (placingDoor) PlacingDoorUpdate();

        else PlacingFloorUpdate();
    }

    private void PlacingDoorUpdate()
    {
        FloorTile floorTileUnderMouse = Station.TileAtLocation(RoundToNearestGrid(GroundLocationUnderMouse));

        if (floorTileUnderMouse != null && floorTileUnderMouse.Room == currentRoom)
        {
            if (floorTileUnderMouse != possibleDoorTile)
            {
                if (possibleDoorTile != null)
                {
                    possibleDoorTile.SwitchToBuitMaterial();
                    possibleDoorTile = null;
                }
                possibleDoorTile = floorTileUnderMouse;
                possibleDoorTile.SwitchToBuildingMaterial();
            }

            
        }
        else if (possibleDoorTile != null)
        {
            possibleDoorTile.SwitchToBuitMaterial();
            possibleDoorTile = null;
        }
    }

    private void PlacingFloorUpdate()
    {
        if (!Game.Input.PrimaryButtonDown) // player is indicating where the floor starts
        {
            topLeftPoint = RoundToNearestGrid(GroundLocationUnderMouse);
            if (topLeftPoint.y >= 500) return;

            if (firstTile == null)
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
                    columns = (int)((botttomRightPoint.x - topLeftPoint.x) / TileSize.x);
                else
                    columns = (int)((topLeftPoint.x - botttomRightPoint.x) / TileSize.x);

                if (botttomRightPoint.z < topLeftPoint.z)
                    rows = (int)((topLeftPoint.z - botttomRightPoint.z) / TileSize.y);
                else
                    rows = (int)((botttomRightPoint.z - topLeftPoint.z) / TileSize.y);

                for (int currentCol = 0; currentCol <= columns; currentCol++)
                {
                    for (int currentRow = 0; currentRow <= rows; currentRow++)
                    {
                        if (currentCol == 0 && currentRow == 0) continue;

                        Vector3 position = firstTile.transform.position;
                        if (botttomRightPoint.x > topLeftPoint.x) position.x += TileSize.x * currentCol;
                        else position.x -= TileSize.x * currentCol;
                        if (botttomRightPoint.z < topLeftPoint.z) position.z -= TileSize.y * currentRow;
                        else position.z += TileSize.y * currentRow;

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

    public Vector3 RoundToNearestGrid(Vector3 location)
    {
        float x = Mathf.Round(location.x / TileSize.x) * TileSize.x;
        float z = Mathf.Round(location.z / TileSize.y) * TileSize.y;
        return new Vector3(x, location.y, z);
    }

    public Vector3 RoundToNearestHalfGrid(Vector3 location)
    {
        float x = Mathf.Round(location.x / (TileSize.x / 2)) * (TileSize.x/2);
        float z = Mathf.Round(location.z / (TileSize.y / 2)) * (TileSize.y/2);
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


        if (PlacementIsValid && canAffordPlacement)
        {
            //Game.Input.OnSecondaryPress -= CancelPlacement;

            Station.Money.Remove(costOfPlacement);

            firstTile.SwitchToBuitMaterial();
            currentRoom.AddFloorTile(firstTile);
            firstTile = null;
            foreach (FloorTile tile in currentPlacement)
            {
                tile.SwitchToBuitMaterial();
                currentRoom.AddFloorTile(tile);
            }
            currentPlacement.Clear();
            Station.AddRoom(currentRoom);
            currentRoom.AddWalls();
            if (Station.Rooms.Count > 1)
            {
                placingDoor = true;
                Game.Input.OnPrimaryPress += AddDoorToPlacement;
            }
            else
            {
                BuildNavMesh();
                currentRoom = null;
                Game.Input.OnSecondaryPress -= CancelPlacement;
            }

            //AddWallsToCurrentRoom();

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

    private void AddDoorToPlacement()
    {
        FloorTile otherRoomTile = possibleDoorTile.OtherRoomTile();
        if (otherRoomTile != null)
        {
            possibleDoorTile.MakeDoorTile();
            otherRoomTile.MakeDoorTile();
            possibleDoorTile.AddWalls();
            possibleDoorTile.SwitchToBuitMaterial();
            possibleDoorTile = null;

            placingDoor = false;
            Game.Input.OnPrimaryPress -= AddDoorToPlacement;

            
            BuildNavMesh();
            currentRoom = null;
            Game.Input.OnSecondaryPress -= CancelPlacement;
        }
    }

    private void BuildNavMesh()
    {
        if (Station.NavMeshSurface == null)
        {
            //firstTile.NavMeshSurface.enabled = true;
            currentRoom.Floor[0].NavMeshSurface.enabled = true;
            Station.SetNavMeshSurface(currentRoom.Floor[0].NavMeshSurface);
            Station.NavMeshSurface.BuildNavMesh();
        }
        else
            Station.NavMeshSurface.BuildNavMesh();
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
        up.z += TileSize.y;
        FloorTile upTile = Station.TileAtLocation(up);
        if (upTile != null && upTile.Room != null)
            list.Add(upTile.Room);

        Vector3 down = tile.transform.position;
        down.z -= TileSize.y;
        FloorTile downTile = Station.TileAtLocation(down);
        if (downTile != null && downTile.Room != null)
            list.Add(downTile.Room);

        Vector3 left = tile.transform.position;
        left.x -= TileSize.x;
        FloorTile leftTile = Station.TileAtLocation(left);
        if (leftTile != null && leftTile.Room != null)
            list.Add(leftTile.Room);

        Vector3 right = tile.transform.position;
        right.x += TileSize.x;
        FloorTile rightTile = Station.TileAtLocation(right);
        if (rightTile != null && rightTile.Room != null)
            list.Add(rightTile.Room);


        return list;
    }

    private List<RoomObject> RoomsTouchingRoom(RoomObject room)
    {
        List<RoomObject> list = new List<RoomObject>();

        foreach(FloorTile tile in room.Floor)
        {
            foreach (RoomObject otherRoom in RoomsTouchingTile(tile))
            {
                if (!list.Contains(room)) list.Add(room);
            }
        }

        if (list.Contains(room)) list.Remove(room);
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

    private void AddWallsToCurrentRoom()
    {
        foreach (FloorTile tile in currentRoom.Floor)
        {
            AddWallsToTile(tile);
        }

        /*foreach (RoomObject room in RoomsTouchingRoom(currentRoom))
        {
            foreach (FloorTile tile in room.Floor)
            {
                AddWallsToTile(tile);
            }
        }*/
    }

    private void AddWallsToTile(FloorTile tile)
    {
        tile.RemoveWalls();

        Vector3 up = tile.transform.position;
        up.z += TileSize.y;
        FloorTile upTile = Station.TileAtLocation(up);
        if (upTile == null)
            tile.AddTopWall();
        else AddWallsToTileSingle(upTile);

            Vector3 down = tile.transform.position;
        down.z -= TileSize.y;
        FloorTile downTile = Station.TileAtLocation(down);
        if (downTile == null)
            tile.AddBottomWall();
        else AddWallsToTileSingle(downTile);

            Vector3 left = tile.transform.position;
        left.x -= TileSize.x;
        FloorTile leftTile = Station.TileAtLocation(left);
        if (leftTile == null)
            tile.AddLeftWall();
        else AddWallsToTileSingle(leftTile);

        Vector3 right = tile.transform.position;
        right.x += TileSize.x;
        FloorTile rightTile = Station.TileAtLocation(right);
        if (rightTile == null)
            tile.AddRightWall();
        else AddWallsToTileSingle(rightTile);

        tile.ApplyPillers();
    }

    private void AddWallsToTileSingle(FloorTile tile)
    {
        tile.RemoveWalls();

        Vector3 up = tile.transform.position;
        up.z += TileSize.y;
        FloorTile upTile = Station.TileAtLocation(up);
        if (upTile == null)
            tile.AddTopWall();

        Vector3 down = tile.transform.position;
        down.z -= TileSize.y;
        FloorTile downTile = Station.TileAtLocation(down);
        if (downTile == null)
            tile.AddBottomWall();

        Vector3 left = tile.transform.position;
        left.x -= TileSize.x;
        FloorTile leftTile = Station.TileAtLocation(left);
        if (leftTile == null)
            tile.AddLeftWall();

        Vector3 right = tile.transform.position;
        right.x += TileSize.x;
        FloorTile rightTile = Station.TileAtLocation(right);
        if (rightTile == null)
            tile.AddRightWall();

        tile.ApplyPillers();
    }

    private bool canAffordPlacement
    {
        get
        {
            if (currentRoom == null || firstTile == null) return false;
            return Station.Money.CanAfford(costOfPlacement);
        }
    }

    private int costOfPlacement
    {
        get
        {
            if (firstTile == null) return 0;
            return currentRoom.Config.Cost(1 + currentPlacement.Count);
        }
    }
}
