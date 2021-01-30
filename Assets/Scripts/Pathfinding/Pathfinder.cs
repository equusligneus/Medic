using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    public static Pathfinder Instance;
    public const int MOVE_STRAIGHT_COST = 10;
    public const int MOVE_DIAGONAL_COST = 14;

    private GridNode[,] grid;

    private List<GridNode> Path = null;
    private List<GridNode> OpenList = null;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    public List<GridNode> FindPath(GridNode _startNode, GridNode _endNode)
    {
        OpenList = null;
        Path = null;
        grid = GridManager.Instance.grid;

        if (grid == null)
        {
            return null;
        }

        List<GridNode> openList = new List<GridNode>();
        List<GridNode> closedList = new List<GridNode>();

        openList.Add(_startNode);
        _startNode.gCost = 0;
        _startNode.hCost = CalculateDistanceCost(_startNode, _endNode);
        _startNode.CalculateFCost();

        while(openList.Count > 0)
        {
            GridNode currentNode = GetLowestFCostNode(openList);
            if(currentNode == _endNode)
            {
                return CalculatePath(currentNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach (GridNode node in GetNodeNeigbours(currentNode))
            {
                if (closedList.Contains(node)) continue;

                int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, node);
                if(tentativeGCost < node.gCost)
                {
                    node.cameFrom = currentNode;
                    node.gCost = tentativeGCost;
                    node.hCost = CalculateDistanceCost(node, _endNode);
                    node.CalculateFCost();

                    if (!openList.Contains(node))
                    {
                        openList.Add(node);
                    }
                }
            }
        }
        OpenList = closedList;
        return null;
    }

    public List<GridNode> CalculatePath(GridNode _endNode)
    {
        List<GridNode> path = new List<GridNode>();
        path.Add(_endNode);
        GridNode currentNode = _endNode;

        while(currentNode.cameFrom != null)
        {
            path.Add(currentNode.cameFrom);
            currentNode = currentNode.cameFrom;
        }
        path.Reverse();
        Path = path;
        return path;
    }

    public List<GridNode> GetNodeNeigbours(GridNode _node)
    {
        List<GridNode> neighbours = new List<GridNode>();

        if (_node.GridPosition.x > 0)
        {
            neighbours.Add(grid[_node.GridPosition.x - 1, _node.GridPosition.y]);
        }

        if (_node.GridPosition.y > 0)
        {
            neighbours.Add(grid[_node.GridPosition.x, _node.GridPosition.y - 1]);
        }

        if (_node.GridPosition.x < GridManager.Instance.Size2.x -1)
        {
            neighbours.Add(grid[_node.GridPosition.x + 1, _node.GridPosition.y]);
        }

        if (_node.GridPosition.y < GridManager.Instance.Size2.y - 1)
        {
            neighbours.Add(grid[_node.GridPosition.x, _node.GridPosition.y + 1]);
        }

        if (_node.GridPosition.x > 0 && _node.GridPosition.y > 0)
        {
            neighbours.Add(grid[_node.GridPosition.x - 1, _node.GridPosition.y - 1]);
        }

        if (_node.GridPosition.x > 0 && _node.GridPosition.y < GridManager.Instance.Size2.y -1)
        {
            neighbours.Add(grid[_node.GridPosition.x - 1, _node.GridPosition.y + 1]);
        }

        if (_node.GridPosition.x < GridManager.Instance.Size2.x -1 && _node.GridPosition.y > 0)
        {
            neighbours.Add(grid[_node.GridPosition.x + 1, _node.GridPosition.y - 1]);
        }

        if (_node.GridPosition.x < GridManager.Instance.Size2.x -1 && _node.GridPosition.y < GridManager.Instance.Size2.y -1)
        {
            neighbours.Add(grid[_node.GridPosition.x + 1, _node.GridPosition.y + 1]);
        }

        return neighbours;
    }

    public int CalculateDistanceCost(GridNode _a, GridNode _b)
    {
        int xDistance = Mathf.Abs(_a.GridPosition.x - _b.GridPosition.x);
        int zDistance = Mathf.Abs(_a.GridPosition.y - _b.GridPosition.y);
        int remaining = Mathf.Abs(xDistance - zDistance);
        int temp = MOVE_DIAGONAL_COST * Mathf.Min(xDistance, zDistance) + MOVE_STRAIGHT_COST * remaining;
        //Debug.Log(temp);
        return temp;
        //return (int)Vector2.Distance(_a.GridPosition, _b.GridPosition);
    }

    public GridNode GetLowestFCostNode(List<GridNode> _openList)
    {
        GridNode returnNode = _openList[0];
        foreach(GridNode node in _openList)
        {
            if(node.fCost < returnNode.fCost)
            {
                returnNode = node;
            }
        }
        return returnNode;
    }

    private void OnDrawGizmos()
    {
        if(Path != null)
        {
            foreach (GridNode node in Path)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(node.Position, 0.2f);
            }
        }

        if(OpenList != null)
        {
            foreach(GridNode node in OpenList)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawCube(node.Position, new Vector3(0.18f, 0.18f, 0.18f));
            }
        }

        if(GridManager.Instance.grid != null)
        {
            grid = GridManager.Instance.grid;
            Gizmos.color = Color.black;
            foreach (GridNode node in GridManager.Instance.grid)
            {
                foreach (GridNode node2 in GetNodeNeigbours(node))
                {
                    
                    Gizmos.DrawLine(node.Position, node2.Position);
                }
            }
        }
    }
}
