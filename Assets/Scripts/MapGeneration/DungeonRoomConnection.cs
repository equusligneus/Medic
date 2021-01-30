using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonRoomConnection : MonoBehaviour
{
    [SerializeField, Tooltip("Change only if not on the same axis as center")] private Vector3 m_roomConnectionDir;
    [Header("For debugging, don't change")]
    [HideInInspector] public bool Connected = false;

    public DungeonRoom ConnectedDungeonRoom; // Not used right now but might be useful

    public Vector3 GetRoomConnectionDir => m_roomConnectionDir;

    /// <summary>
    /// Sets the parents world position so that this world position is matching the one of the parameter
    /// </summary>
    /// <param name="_position">Position this connection should have in world space</param>
    public void SetRoomPositionFromConnectionPosition(Vector3 _position)
    {
        transform.parent.position = _position - (new Vector3(this.transform.localPosition.x * this.transform.parent.localScale.x ,this.transform.localPosition.y, this.transform.localPosition.z * this.transform.parent.localScale.z));
    }

    /// <summary>
    /// Rotates the room so that both connections are looking into opposite direction so they match each other
    /// </summary>
    /// <param name="_sourceTransform"></param>
    public void RotateRoomToMatch(Transform _sourceTransform)
    {
        Vector3 thisDir = (transform.position - this.transform.parent.position);
        Vector3 otherDir = ((_sourceTransform.position - _sourceTransform.parent.position) * -1);
        //if (thisDir != otherDir) // already looking into opposite directions
        //{
        //    transform.parent.rotation = Quaternion.LookRotation((_sourceTransform.position - _sourceTransform.parent.position) * -1, Vector3.up);
        //}
        this.transform.parent.LookAt(this.transform.parent.position + otherDir);

    }

    private void Awake()
    {
        if (m_roomConnectionDir == Vector3.zero)
            m_roomConnectionDir = transform.localPosition;
        m_roomConnectionDir.Normalize();
    }
}
