using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfinderAgent : MonoBehaviour
{
    public float MovementSpeed = 8.0f;
    public float RotationSpeed = 2.0f;
    public Vector3 MoveDir { get; private set; }

    private List<GridNode> path = null;

    private int index = 0;
    private GridNode nextNode = null;
    
    private GridNode targetGridNode;

    public bool stop = true;

    // Update is called once per frame
    void Update()
    {
        if (!stop)
        {
            MoveAgent();
        }
    }

    /// <summary>
    /// Move the Agent to the Position if he find a Path
    /// </summary>
    /// <param name="_target"></param>
    /// <returns>Did the Agent find a path</returns>
    public bool MoveTo(Vector3 _target)
    {
        bool returnError = false;
        if (GridManager.Instance.GridIsGenerate)
        {
            nextNode = GridManager.Instance.GetGridNode(transform.position);
            targetGridNode = GridManager.Instance.GetGridNode(_target);

            if (nextNode != null && targetGridNode != null)
            {
                path = Pathfinder.Instance.FindPath(nextNode, targetGridNode);

                if (path != null && path.Count > 0)
                {
                    if(path.Count > 1)
                    {
                        index = 1;
                    }
                    else
                    {
                        index = 0;
                    }
                    nextNode = path[index];

                    stop = false;
                    returnError = true;
                }
                else
                {
                    stop = true;
                    Debug.Log("Can't Find a Path");
                }

                GridManager.Instance.ClearTempNodedata();
            }
            else
            {
                Debug.Log("Start or endnode not found");
            }
        }
        else
        {
            Debug.Log("No Grid to move");
        }
        return returnError;
    }

    /// <summary>
    /// Move the Agent to the next Node in the Path
    /// If the Agent on this node it will be switcht
    /// </summary>
    private void MoveAgent()
    {
        if(nextNode != null)
        {
            if (0.1f > Vector3.Distance(nextNode.Position, transform.position))
            {
                index++;
                Debug.Log(path != null);
                if (index == path.Count)
                {
                    stop = true;
                    path = null;
                }
                else
                {
                    if (index < path.Count)
                    {
                        nextNode = path[index];
                    }
                }
            }

            if (!stop)
            {
                //Rotate the Agent to move Direction
                MoveDir = -(transform.position - nextNode.Position);
                Quaternion toRotation = Quaternion.LookRotation(MoveDir);
                transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, RotationSpeed * Time.deltaTime);
                //Move Agent
                transform.position += (nextNode.Position - transform.position).normalized * MovementSpeed * Time.deltaTime;
            }
        }
    }
}
