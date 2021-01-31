using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    [SerializeField]
    public Vector2Int Size;
    public Vector2Int Size2;
    [SerializeField]
    private float NodeSize;

    public GridNode[,] grid { get; private set; }

    private RaycastHit hit;
    [SerializeField]
    private LayerMask RaycastMask;
    [SerializeField]
    private LayerMask SphercastMask;

    public bool StartInAwake = false;
    public bool GridIsGenerate = false;
    public bool ShowGizmo = false;

    private void Awake()
    {
        Instance = this;
        if (StartInAwake)
        {
            GenerateGrid();
        }
    }

    [ContextMenu("Generate Grid")]
    public void GenerateGrid()
    {
        Vector2Int gridSize = new Vector2Int((int)(Size.x / NodeSize), (int)(Size.y / NodeSize));
        grid = new GridNode[gridSize.x, gridSize.y];
        Size2 = gridSize;

        float startX = transform.position.x + -(((gridSize.x * NodeSize) / 2) + (NodeSize / 2));
        float startZ = transform.position.z + -(((gridSize.y * NodeSize) / 2) + (NodeSize / 2));

        for(int x = 0; x < gridSize.x; x++)
        {
            startX += NodeSize;
            for(int z = 0; z < gridSize.y; z++)
            {
                startZ += NodeSize;

                Vector3 worldPos = new Vector3(startX,transform.position.y + 50, startZ);

                grid[x, z] = new GridNode(x, z);

                if(Physics.Raycast(worldPos, Vector3.down, out hit, 100, RaycastMask))
                {
                    worldPos.y = (hit.point.y > 0.01f) ? hit.point.y : 0.0f;

                    grid[x, z].Walkable = (!Physics.CheckSphere(worldPos, NodeSize * 0.9f, SphercastMask)) ? true : false;
                }
                else
                {
                    worldPos.y = transform.position.y;
                }
                grid[x, z].Position = worldPos;
            }
            startZ = -(((gridSize.y * NodeSize) / 2) + (NodeSize / 2));
        }

        GridIsGenerate = true;
    }

    public GridNode GetGridNode(Vector3 _pos)
    {
        //GridNode nearestNode = null;
        //foreach(GridNode node in grid)
        //{
        //    if(Vector3.Distance(node.Position, _pos) < (NodeSize * 0.5f))
        //    {
        //        nearestNode = node;
        //        break;
        //    }
        //}
        //return nearestNode;
        GridNode tempNode = null;
        foreach (GridNode node in grid)
        {
            if ((Mathf.Abs(node.Position.x - _pos.x) <= (1 * 0.5f)) && (Mathf.Abs(node.Position.z - _pos.z) <= (1 * 0.5f)))
            //if (Vector3.Distance(node.Position, _position) < ((1 * 0.5f) + 0.001f))
            {
                tempNode = node;
                break;
            }
        }
        return tempNode;
    }

    public void SetWalkableInside(Collider _collider, bool _walkable)
    {
        if(grid != null)
        {
            foreach (GridNode node in grid)
            {
                if (_collider.bounds.Contains(node.Position))
                {
                    node.Walkable = _walkable;
                }
            }
        }
    }

    public void ClearTempNodedata()
    {
        foreach(GridNode node in grid)
        {
            node.ResetTempData();
        }
    }

    private void OnDrawGizmos()
    {
        if(grid != null && ShowGizmo)
        {
            foreach(GridNode node in grid)
            {
                if (node == null) continue;
                if (node.Walkable)
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawWireCube(node.Position, new Vector3(NodeSize - 0.1f, 0.2f, NodeSize - 0.1f));
                }
                else
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawWireCube(node.Position, new Vector3(NodeSize - 0.1f, 0.2f, NodeSize - 0.1f));
                }
            }
        }
    }
}


public class GridNode
{
    /// <summary>
    /// World Positon from the Node
    /// </summary>
    public Vector3 Position;

    /// <summary>
    /// Grid Position from the Node
    /// </summary>
    public Vector2Int GridPosition;

    public int gCost = int.MaxValue;
    public int hCost;
    public int fCost;

    public GridNode cameFrom = null;

    public bool Walkable = false;

    public GridNode(int _gridPosX, int _gridPosY)
    {
        GridPosition = new Vector2Int(_gridPosX, _gridPosY);
    }

    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }

    /// <summary>
    /// Clear temp node Data
    /// </summary>
    public void ResetTempData()
    {
        cameFrom = null;
        gCost = int.MaxValue;
    }
}