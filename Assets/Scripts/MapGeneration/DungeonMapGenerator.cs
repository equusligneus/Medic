using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonMapGenerator : MonoBehaviour
{
    [Header("Options"), SerializeField] private int m_amountOfRooms;

    [SerializeField] private DungeonRoom m_startingRoom;
    [SerializeField] private List<GameObject> m_roomPrefabs = new List<GameObject>();
    [SerializeField, Space(15f)] private List<DungeonRoom> m_placedRooms = new List<DungeonRoom>();

    private void Start()
    {
        GenerateRooms();
    }

    public void GenerateRooms()
    {
        // Remove existing rooms
        foreach (DungeonRoom room in m_placedRooms)
        {
            if (room != null)
                GameObject.DestroyImmediate(room.gameObject);
        }
        m_placedRooms.Clear();

        // Create new Dungeon
        DungeonRoom currentRoom = m_startingRoom;

        DungeonRoom nextRoom;
        for (int i = 0; i < m_amountOfRooms; i++)
        {
            // Get random room of the prefabs
            nextRoom = Instantiate(m_roomPrefabs[Random.Range(0, m_roomPrefabs.Count)].GetComponent<DungeonRoom>());
            m_placedRooms.Add(nextRoom);
            ConnectRooms(currentRoom, nextRoom);
            currentRoom = nextRoom;
        }
    }

    //TODO: Rotate Rooms to connect them
    // For now they will only connect connections pointing into the same direction
    private void ConnectRooms(DungeonRoom _existingRoom, DungeonRoom _newRoom)
    {
        // Get random active connection
        List<DungeonRoomConnection> activeConnections = new List<DungeonRoomConnection>();
        foreach (DungeonRoomConnection roomConnection in _existingRoom.m_connectionPoints)
        {
            if (roomConnection.gameObject.activeSelf && !roomConnection.Connected)
                activeConnections.Add(roomConnection);
        }
        DungeonRoomConnection existingRoomConnection = activeConnections[Random.Range(0, activeConnections.Count)];
        Debug.Log(existingRoomConnection, existingRoomConnection);

        // New room connection point that is available
        foreach (DungeonRoomConnection newRoomConnection in _newRoom.m_connectionPoints)
        {
            if (existingRoomConnection.gameObject.activeSelf && newRoomConnection.gameObject.activeSelf &&
                !newRoomConnection.Connected && !existingRoomConnection.Connected)
            {
                Debug.Log(newRoomConnection, newRoomConnection);
                // Rotate room to match connections (doors)
                newRoomConnection.RotateRoomToMatch(existingRoomConnection.transform);

                if (existingRoomConnection.GetRoomConnectionDir == newRoomConnection.GetRoomConnectionDir * -1) // Just to be safe
                {
                    // Set new room so both connections are on top of each other
                    newRoomConnection.SetRoomPositionFromConnectionPosition(existingRoomConnection.transform.position);
                    // Give the rooms some information
                    existingRoomConnection.Connected = true;
                    newRoomConnection.Connected = true;
                    existingRoomConnection.ConnectedDungeonRoom = _newRoom;
                    newRoomConnection.ConnectedDungeonRoom = _existingRoom;
                    break;
                }
            }
        }
    }
}
