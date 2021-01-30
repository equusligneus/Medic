using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DungeonRoomConnection : MonoBehaviour
{
    [Header("For debugging, don't change")]
    [SerializeField, Tooltip("Change only if not on the same axis as center, which will not work right now")] private Vector3 m_roomConnectionVector;
    [HideInInspector] public bool Connected = false;
    [HideInInspector] public DungeonRoom ConnectedDungeonRoom; // Not used right now but might be useful

    // debug
    [HideInInspector] public DungeonRoomConnection temp;

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

    /// <summary>
    /// Sets the parents world position so that this world position is matching the one of the parameter
    /// </summary>
    /// <param name="_sourceConnection"></param>
    public void SetRoomPositionFromConnectionPosition(DungeonRoomConnection _sourceConnection)
    {
        Vector3 sourceDir = VecFromToPoint(_sourceConnection.transform.parent.position,
            _sourceConnection.transform.position);
        //TODO: Scale
        transform.parent.position = _sourceConnection.transform.parent.position + sourceDir +
            sourceDir.normalized * sourceDir.magnitude;
    }

    /// <summary>
    /// Rotates the room so that both connections are looking into opposite direction so they match each other
    /// </summary>
    /// <param name="_sourceConnection">The rotation to match</param>
    public void RotateRoomToMatch(DungeonRoomConnection _sourceConnection)
    {
        temp = _sourceConnection;
        Vector3 dirSource = _sourceConnection.transform.position - _sourceConnection.transform.parent.position;
        // Rotate until it fits
        if (this.transform.position != _sourceConnection.transform.position)
        {
            this.transform.parent.RotateAround(this.transform.parent.position, Vector3.up, 90);
        }
        if (this.transform.position != _sourceConnection.transform.position)
        {
            this.transform.parent.RotateAround(this.transform.parent.position, Vector3.up, 90);
        }
        if (this.transform.position != _sourceConnection.transform.position)
        {
            this.transform.parent.RotateAround(this.transform.parent.position, Vector3.up, 90);
        }
    }

    private void Awake()
    {
        if (m_roomConnectionVector == Vector3.zero)
            m_roomConnectionVector = transform.localPosition;
    }
}
