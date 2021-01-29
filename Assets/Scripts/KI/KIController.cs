using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KIController : MonoBehaviour
{
    public List<Transform> Waypoints = new List<Transform>();
    private int index = 0;
    public float range = 0.0f;
    private Vector3 currentGoalPosition;

    private CharacterController charContr;

    public float MovementSpeed = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        charContr = GetComponent<CharacterController>();
        currentGoalPosition = Waypoints[index].position;
    }

    // Update is called once per frame
    void Update()
    {
        if (AtGoal(currentGoalPosition))
        {
            NextWaypoint();
        }

        Move(currentGoalPosition);
    }

    public void Move(Vector3 _goal)
    {
        charContr.Move((MoveDirection(_goal) * MovementSpeed * Time.deltaTime));
    }

    public Vector3 MoveDirection(Vector3 _goal)
    {
        return -(transform.position - _goal).normalized;
    }

    public bool AtGoal(Vector3 _goal)
    {
        return Vector3.Distance(transform.position, _goal) < range;
    }

    public void NextWaypoint()
    {
        Debug.Log(index + " / " + Waypoints[index]);
        if(index < Waypoints.Count-1)
        {
            index++;
        }
        else
        {
            index = 0;
        }
        currentGoalPosition = Waypoints[index].position;
    }
}
