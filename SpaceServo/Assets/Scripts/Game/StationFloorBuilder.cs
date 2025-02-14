using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// controlls placing floors in the station

// TODO: walls, cost, input error checking

public class StationFloorBuilder : MonoBehaviour
{
    [SerializeField] FloorTile floorTilePrefab;
    [SerializeField] Vector2 tileSize = new Vector2(5,5);
    [SerializeField] MeshRenderer plane; // this is something for raycast to hit

    bool placing;
    Vector3 topLeftPoint;
    Vector3 botttomRightPoint;
    FloorTile firstTile;
    [SerializeField] List<FloorTile> currentPlacement = new List<FloorTile>();

    private void Update()
    {
        if (!placing || UI.MouseOverUI) return;

        if (!Game.Input.PrimaryButtonDown) // player is indicating where the floor starts
        {
            topLeftPoint = RoundToNearestGrid(GroundLocationUnderMouse);
            if (topLeftPoint.y >= 500) return;

            if (firstTile == null )
            {
                firstTile = Instantiate(floorTilePrefab, topLeftPoint, Quaternion.identity);
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
                    rows = (int)((topLeftPoint.z - botttomRightPoint.z) / tileSize.y);

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

                        FloorTile newTile = Instantiate(floorTilePrefab, position, Quaternion.identity);
                        newTile.SwitchToBuildingMaterial();
                        currentPlacement.Add(newTile);
                    }
                }
            }
        }
    }

    public void StartPlacingFloor()
    {
        placing = true;
        Game.Input.OnPrimaryPress += PlaceFirstTile;
        Game.Input.OnSecondaryPress += CancelPlacement;
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
        firstTile.transform.parent = Station.Instance.transform;
        firstTile = null;

        foreach(FloorTile tile in currentPlacement)
        {
            tile.SwitchToBuitMaterial();
            tile.transform.parent = Station.Instance.transform;
        }
            
        currentPlacement.Clear();

        placing = false;
    }

    private void CancelPlacement()
    {
        Game.Input.OnSecondaryPress -= CancelPlacement;

        Destroy(firstTile.gameObject);
        firstTile = null;

        if (Game.Input.PrimaryButtonDown)
            Game.Input.OnPrimaryRelease -= CompletePlacement;
        else
            Game.Input.OnPrimaryPress -= PlaceFirstTile;

        foreach (FloorTile tile in currentPlacement.ToArray())
            Destroy(tile.gameObject);
        currentPlacement.Clear();

        placing = false;
    }
}
