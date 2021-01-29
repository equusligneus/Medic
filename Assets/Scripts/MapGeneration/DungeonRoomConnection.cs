using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonRoomConnection : MonoBehaviour
{
    [SerializeField] private Vector3 m_roomConnectionDir; // Should always be normalized
    public bool Connected = false;

    public List<DungeonRoom> ConnectedDungeonRooms = new List<DungeonRoom>();

    public Vector3 GetRoomConnectionDir => m_roomConnectionDir;
    public Transform GetRoomTransform => this.transform.parent;
}
