using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonRoomConnection : MonoBehaviour
{
    [SerializeField] private Vector3 m_roomConnectionDir;
    public bool Connected = false;

    public Vector3 GetRoomConnectionDir => m_roomConnectionDir;
}
