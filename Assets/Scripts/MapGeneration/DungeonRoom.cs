using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonRoom : MonoBehaviour
{
    [SerializeField] public List<DungeonRoomConnection> m_connectionPoints;

    public void SetPositionRelativeToConnectionPosition(DungeonRoomConnection _connection)
    {
        Vector3 connectionOffset = _connection.GetRoomConnectionDir;
        connectionOffset = new Vector3(
            connectionOffset.x * _connection.transform.localPosition.x *transform.localScale.x,
            connectionOffset.y * _connection.transform.localPosition.y *transform.localScale.y,
            connectionOffset.z * _connection.transform.localPosition.z *transform.localScale.z);
        this.transform.position = _connection.transform.position + connectionOffset;
    }
}
