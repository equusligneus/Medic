using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DungeonRoomConnection : MonoBehaviour
{
    [SerializeField, Tooltip("Change only if not on the same axis as center")] private Vector3 m_roomConnectionVector;
    [Header("For debugging, don't change")]
    public bool Connected = false;

    public DungeonRoom ConnectedDungeonRoom; // Not used right now but might be useful

    // debug
    public DungeonRoomConnection temp;

    public static Vector3 VecFromToPoint(Vector3 _start, Vector3 _end)
    {
        return _end - _start;
    }

    public Vector3 GetRoomConnectionVector => m_roomConnectionVector;

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


        //int i = 0;

        //Vector3 thisDir = this.m_roomConnectionVector;
        //Vector3 otherDir = _sourceConnection.m_roomConnectionVector;
        //// in 90 degree
        //while (otherDir != (thisDir * -1) && i < 10)
        //{
        //    thisDir = (transform.position - this.transform.parent.position);
        //    otherDir = ((_sourceConnection.transform.position - _sourceConnection.transform.parent.position));
        //    this.transform.parent.RotateAround(this.transform.parent.position, Vector3.up, 90);
        //    i++;
        //}
        //if (i == 10)
        //    Debug.LogWarning("This TBH");


        //Vector3 thisDir = (transform.position - this.transform.parent.position);
        //Vector3 otherDir = ((_sourceConnection.transform.position - _sourceConnection.transform.parent.position));

        //float sourceAngle = Vector3.Angle(otherDir, thisDir);
        //Debug.Log(sourceAngle);
        //this.transform.parent.eulerAngles = new Vector3(0, sourceAngle, 0);
        //if (this.transform.position != _sourceConnection.transform.position)
        //    this.transform.parent.eulerAngles = new Vector3(0, sourceAngle - 180, 0);
    }

    private void Awake()
    {
        if (m_roomConnectionVector == Vector3.zero)
            m_roomConnectionVector = transform.localPosition;
    }
}
