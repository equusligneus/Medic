using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DungeonRoomConnection : MonoBehaviour
{
    [Header("Set connection room direction:")]
    [SerializeField] public Vector3 m_roomConnectionRotation;
    [HideInInspector] public bool Connected = false;
    [HideInInspector] public DungeonRoom ConnectedDungeonRoom; // Not used right now but might be useful
    [SerializeField, Tooltip("Technically don't need to be in single door rooms")] private Collider doorTrigger;

    /// <summary>
    /// Finds the Vector3 from one point to another
    /// </summary>
    /// <param name="_start">Starting point</param>
    /// <param name="_end">End point</param>
    /// <returns>Vector3 from start to end</returns>
    public static Vector3 VecFromToPoint(Vector3 _start, Vector3 _end)
    {
        return _end - _start;
    }

    private void Awake()
    {
        if (m_roomConnectionRotation == Vector3.zero)
            m_roomConnectionRotation = transform.localPosition;
    }

    public bool DoorMayOpen
    {
        get
        {
            if (doorTrigger == null)
                return false;
            return doorTrigger.enabled;
        }
        set
        {
            if (doorTrigger == null)
                return;
            doorTrigger.enabled = value;
        }
    }
}
