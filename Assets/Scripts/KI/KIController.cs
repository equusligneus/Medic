using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KIController : MonoBehaviour
{
    public List<Transform> Waypoints = new List<Transform>();
    private int index = 0;
    public float range = 1.0f;
    private Vector3 currentGoalPosition;

    private CharacterController charContr;

    public float MovementSpeed = 2.0f;
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
        transform.LookAt(new Vector3(_goal.x, 0, _goal.z));
        Vector3 dir = MoveDirection(_goal);
        dir *= MovementSpeed * Time.deltaTime;
        dir = transform.TransformDirection(dir);

        charContr.Move(dir);
    }

    public Vector3 MoveDirection(Vector3 _goal)
    {
        return transform.InverseTransformDirection(-(transform.position - _goal).normalized);
    }

    public bool AtGoal(Vector3 _goal)
    {
        return Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(_goal.x, 0, _goal.z)) < range;
    }

    public void NextWaypoint()
    {
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