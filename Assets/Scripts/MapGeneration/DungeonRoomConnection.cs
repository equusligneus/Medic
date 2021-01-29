using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonRoomConnection : MonoBehaviour
{
    [SerializeField] private Vector3 m_roomConnectionDir; // Should always be normalized
    public bool Connected = false;

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
}
