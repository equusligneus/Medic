using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DungeonRoom : MonoBehaviour
{
    [SerializeField] public List<DungeonRoomConnection> m_connectionPoints;

    /// <summary>
    /// True if all connections got rooms connected, otherwise false
    /// </summary>
    public bool AllConnected
    {
        get
        {
            return m_connectionPoints.All(connection => connection.Connected);
        }
    }
    
}
