using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonMapGenerator : MonoBehaviour
{
    [Header("Options"), SerializeField] private int m_amountOfRooms;

    [SerializeField] private List<GameObject> m_roomPrefabs = new List<GameObject>();
    [SerializeField] private List<DungeonRoom> m_placedRooms = new List<DungeonRoom>(); // Not used yet, but might be useful to store
    [SerializeField] private DungeonRoom m_startingRoom;

    private void Awake()
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
        }
    }

    //TODO: Rotate Rooms to connect them
    // For now they will only connect connections pointing into the same direction
    private void ConnectRooms(DungeonRoom _existingRoom, DungeonRoom _newRoom)
    {
        foreach (DungeonRoomConnection existingRoomConnectionPoint in _existingRoom.m_connectionPoints)
        {
            if (existingRoomConnectionPoint.gameObject.activeSelf && !existingRoomConnectionPoint.Connected)
            {
                foreach (DungeonRoomConnection newRoomConnectionPoint in _newRoom.m_connectionPoints)
                {
                    if (newRoomConnectionPoint.gameObject.activeSelf && !newRoomConnectionPoint.Connected && 
                        existingRoomConnectionPoint.GetRoomConnectionDir == newRoomConnectionPoint.GetRoomConnectionDir)
                    {
                        // Assuming the rooms transform position is always at the center of the room
                        _newRoom.SetPositionRelativeToConnectionPosition(existingRoomConnectionPoint);
                        //TODO: make connections of new rooms Connected=false
                    }
                }
            }
        }
    }
}
