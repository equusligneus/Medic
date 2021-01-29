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

    public Transform Player;
    public float ViewRange = 4.0f;
    public float ViewAngle = 45.0f;

    public float RotationSpeed = 20.0f;
    // Start is called before the first frame update
    void Start()
    {
        charContr = GetComponent<CharacterController>();
        currentGoalPosition = Waypoints[index].position;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerInViewSpace())
        {

        }

        if (AtGoal(currentGoalPosition))
        {
            NextWaypoint();
        }

        Move(currentGoalPosition);
    }

    public void Move(Vector3 _goal)
    {
        Vector3 dir = MoveDirection(_goal);
        Quaternion toRotation = Quaternion.LookRotation(-(transform.position - _goal));
        transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, RotationSpeed * Time.deltaTime);
        //transform.LookAt(new Vector3(_goal.x, 0, _goal.z));
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

    public bool PlayerInViewSpace()
    {
        if(Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(Player.position.x, 0, Player.position.z)) < ViewRange)
        {
            if (Vector3.Angle(MoveDirection(Player.position), Vector3.forward) < ViewAngle)
            {
                currentGoalPosition = Player.position;
                return true;
            }
            else
            {
                currentGoalPosition = Waypoints[index].position;
                return false;
            }
        }
        else
        {
            currentGoalPosition = Waypoints[index].position;
            return false;
        }
    }
}