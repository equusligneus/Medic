using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DungeonRoomConnection : MonoBehaviour
{
    [SerializeField, Tooltip("Change only if not on the same axis as center")] private Vector3 m_roomConnectionDir;
    [Header("For debugging, don't change")]
    [HideInInspector] public bool Connected = false;

    public DungeonRoom ConnectedDungeonRoom; // Not used right now but might be useful

    //debug
    private Vector3 pos;

    public Vector3 GetRoomConnectionDir => m_roomConnectionDir;

    /// <summary>
    /// Sets the parents world position so that this world position is matching the one of the parameter
    /// </summary>
    /// <param name="_position">Position this connection should have in world space</param>
    public void SetRoomPositionFromConnectionPosition(Vector3 _position)
    {
        //debug
        pos = _position;
        //TODO: Scale
        Vector3 thisCenterToConnection = new Vector3(transform.position.x - transform.parent.position.x, transform.parent.position.y, transform.position.z - transform.parent.position.z);
        transform.parent.position = _position + thisCenterToConnection;
    }

    /// <summary>
    /// Rotates the room so that both connections are looking into opposite direction so they match each other
    /// </summary>
    /// <param name="_sourceConnection">The rotation to match</param>
    public void RotateRoomToMatch(DungeonRoomConnection _sourceConnection)
    {
        //int i = 0;

        //Vector3 thisDir = (transform.position - this.transform.parent.position);
        //Vector3 otherDir = ((_sourceConnection.transform.position - _sourceConnection.transform.parent.position));
        //// in 90 degree
        //while (otherDir != (thisDir * -1) && i < 10)
        //{
        //    thisDir = (transform.position - this.transform.parent.position);
        //    otherDir = ((_sourceConnection.transform.position - _sourceConnection.transform.parent.position));
        //    this.transform.parent.RotateAround(this.transform.position, Vector3.up, 90);
        //    i++;
        //}
        //if (i == 10)
        //    Debug.LogWarning("This TBH");


        Vector3 thisDir = (transform.position - this.transform.parent.position);
        Vector3 otherDir = ((_sourceConnection.transform.position - _sourceConnection.transform.parent.position));

        float sourceAngle = Vector3.Angle(otherDir, thisDir);
        Debug.Log(sourceAngle);
        this.transform.parent.eulerAngles = new Vector3(0, sourceAngle, 0);
        if (this.transform.position != _sourceConnection.transform.position)
            this.transform.parent.eulerAngles = new Vector3(0, sourceAngle - 180, 0);
    }

    private void Awake()
    {
        if (m_roomConnectionDir == Vector3.zero)
            m_roomConnectionDir = transform.localPosition;
        m_roomConnectionDir.Normalize();
    }

    void OnDrawGizmosSelected()
    {
        Vector3 thisCenterToConnection = new Vector3(transform.position.x - transform.parent.position.x, transform.parent.position.y, transform.position.z - transform.parent.position.z);
        Gizmos.DrawLine(pos, pos - thisCenterToConnection);
    }
}
