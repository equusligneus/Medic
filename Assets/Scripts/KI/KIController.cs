using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// If you have Question ask me GotrekGurrission
/// </summary>
public class KIController : MonoBehaviour
{
    [Header("Waypoints")]
    [SerializeField]
    private List<Transform> waypoints = new List<Transform>();

    private Animator animator;
    private CharacterController charContr;
    public PathfinderAgent Agent;
    public Enemy_Attack_Ability AttackAbility { get; private set; }

    public Vector3 currentTargetPosition;

    [Space(10)]
    [Header("KI Settings")]
    [Range(1, 10), Tooltip("Movmentspeed of the Enemy")]
    public float MovementSpeed = 2.0f;
    [Range(0, 10), Tooltip("Rotationspeed for the Lerp of the Lookat to the Target")]
    public float RotationSpeed = 2.0f;
    [Range(1, 20), Tooltip("The Viewdistance of the Enemy")]
    public float ViewRange = 4.0f;
    [Range(0,180), Tooltip("Viewangle of the Enemy")]
    public float ViewAngle = 45.0f;
    [Range(0, 2), Tooltip("The Range that must Reach to change the Waypoint")]
    public float TargetRange = 1.0f;
    public LayerMask BlockedLayer;

    public float InvincibilityTime = 5f;

    private int index = 0;
    private bool followPlayer = false;

    public float BreakTime = 6.0f;
    public bool CantReach = false;

    //public Transform Player;
    public Ref<bool> PlayerIsAlive;
    public Ref<bool> PlayerAwake;

    public Ref<Transform> Player;

    public bool IsStunned { get; private set; }

    public bool CanBeStunned
        => !IsStunned && !PlayerInViewSpace();

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        charContr = GetComponent<CharacterController>();
        Agent = GetComponent<PathfinderAgent>();
        AttackAbility = GetComponent<Enemy_Attack_Ability>();
        currentTargetPosition = waypoints[index].position;
    }
    
    // Update is called once per frame
    void Update()
    {

    }

    public void SetKiActive(bool _status)
    {
        animator.enabled = _status;
    }

    public void AddWaypoints(List<Transform> _waypoints)
    {
        waypoints = _waypoints;
        animator = GetComponent<Animator>();
        charContr = GetComponent<CharacterController>();
        Agent = GetComponent<PathfinderAgent>();
        AttackAbility = GetComponent<Enemy_Attack_Ability>();
        index = 0;
        currentTargetPosition = waypoints[index].position;
    }

    public void Move()
    {
        Vector3 dir = MoveDirection(currentTargetPosition);
        Quaternion toRotation = Quaternion.LookRotation(-(transform.position - currentTargetPosition));
        transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, RotationSpeed * Time.deltaTime);
        dir *= MovementSpeed * Time.deltaTime;
        dir = transform.TransformDirection(dir);

        charContr.Move(dir);
    }

    public Vector3 MoveDirection(Vector3 _goal)
    {
        return transform.InverseTransformDirection(-(transform.position - _goal).normalized);
    }

    public bool AtGoal()
    {
        return Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(currentTargetPosition.x, 0, currentTargetPosition.z)) < TargetRange;
    }

    public void NextWaypoint()
    {
        //Debug.Log("NextWaypoint");
        if (followPlayer)
        {
            currentTargetPosition = waypoints[index].position;
            followPlayer = false;
        }
        else
        {
            if (index < waypoints.Count - 1)
            {
                index++;
            }
            else
            {
                index = 0;
            }
            currentTargetPosition = waypoints[index].position;
        }
    }

    public bool PlayerInViewSpace()
    {
        if ((Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(Player.Get().position.x, 0, Player.Get().position.z)) < ViewRange) && PlayerAwake.Get() && PlayerIsAlive.Get())
        {
            if (Vector3.Angle(MoveDirection(Player.Get().position), Vector3.forward) < ViewAngle)
            {
                if(!Physics.Linecast(transform.position, Player.Get().position, BlockedLayer) && !CantReach)
                {
                    currentTargetPosition = Player.Get().position;
                    followPlayer = true;
                    return true;
                }
            }
        }    
        return false;
    }

    public void Stun()
	{
        IsStunned = true;
        Agent.stop = !Agent.stop;
        animator.SetBool("Stunned", true);
	}

    public void EndStun()
	{
        StartCoroutine(InvincibilityTimer());
    }

    private IEnumerator InvincibilityTimer()
	{
        yield return new WaitForSeconds(InvincibilityTime);
        IsStunned = false;
        Agent.stop = !Agent.stop;
        animator.SetBool("Stunned", false);
	}
}