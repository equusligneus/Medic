using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfinderAgent : MonoBehaviour
{
    public float MovementSpeed = 8.0f;

    private List<GridNode> path = null;

    private int index = 0;
    private GridNode nextNode;

    [SerializeField]
    private Vector3 targetPosition;
    private GridNode targetGridNode;

    public Transform debugGoal;

    public bool stop;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!stop)
        {
            MoveAgent();
        }
    }

    [ContextMenu("Move to Position")]
    public void MoveTooTargetPosition()
    {
        MoveTo(debugGoal.position);
    }

    public void MoveTo(Vector3 _target)
    {
        nextNode = GridManager.Instance.GetGridNode(transform.position);
        targetGridNode = GridManager.Instance.GetGridNode(_target);

        if (nextNode != null && targetGridNode != null)
        {
            path = Pathfinder.Instance.FindPath(nextNode, targetGridNode);

            if (path != null && path.Count > 0)
            {
                index = 0;
                nextNode = path[index];

                stop = false;
            }
            else
            {
                Debug.Log("Can't Find a Path");
            }

            GridManager.Instance.ClearTempNodedata();
        }
        else
        {
            Debug.Log("Start or endnode not found");
        }
    }

    public void MoveAgent()
    {
        if(0.1f > Vector3.Distance(nextNode.Position, transform.position))
        {
            index++;
            if(index == path.Count)
            {
                stop = true;
                path = null;
            }
            else
            {
                if(index < path.Count)
                {
                    nextNode = path[index];
                }
            }
        }
        else
        {
            transform.position += (nextNode.Position - transform.position).normalized * MovementSpeed * Time.deltaTime;
        }
    }
}
