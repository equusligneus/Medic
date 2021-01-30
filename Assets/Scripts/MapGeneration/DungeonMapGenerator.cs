using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonMapGenerator : MonoBehaviour
{
    [Header("Options")]
    [SerializeField] private int m_targetRoomAmount;
    [SerializeField] private DungeonRoom m_startingRoom;
    [SerializeField] private List<GameObject> m_roomPrefabs = new List<GameObject>();
    [SerializeField, Space(15f)] private List<DungeonRoom> m_placedRooms = new List<DungeonRoom>();

    //debug
    public DungeonRoom currentRoom;

    private void Start()
    {
        GenerateDungeon();
    }

    public void GenerateDungeon()
    {
        // Remove existing rooms - for debug
        //foreach (DungeonRoom room in m_placedRooms)
        //{
        //    if (room != null)
        //        GameObject.Destroy(room.gameObject);
        //}
        //m_placedRooms.Clear();

        // Create new Dungeon
        List<DungeonRoom> roomsWithOpenConnection = new List<DungeonRoom> { m_startingRoom };
        //for (int i = 0; i < m_amountOfRooms; i++)
        //DungeonRoom currentRoom;
        while (roomsWithOpenConnection.Count > 0)
        {
            // use any room, sequence doesn't matter
            currentRoom = roomsWithOpenConnection[0];
            // create a new room
            DungeonRoom nextRoom = AddRoom(currentRoom, m_roomPrefabs[Random.Range(0, m_roomPrefabs.Count)]);
            // when nextRoom got connections add it to list of open connections
            if (nextRoom != null && !nextRoom.AllConnected && !roomsWithOpenConnection.Contains(nextRoom)) // second might be useless
            {
                roomsWithOpenConnection.Add(nextRoom);
            }
            // when all connections occupied remove from list
            if (currentRoom.AllConnected && roomsWithOpenConnection.Contains(currentRoom))
            {
                roomsWithOpenConnection.Remove(currentRoom);
            }

            // no room with unconnected door left -> break
            if (roomsWithOpenConnection.Count == 0) break;
        }
    }

    public DungeonRoom AddRoom(DungeonRoom _startingRoom, GameObject _roomPrefab)
    {
        // Get random room of the prefabs
        DungeonRoom nextRoom = Instantiate(_roomPrefab).GetComponent<DungeonRoom>();
        m_placedRooms.Add(nextRoom);
        bool stopGeneration = ConnectRooms(_startingRoom, nextRoom);
        return stopGeneration ? null : nextRoom;
    }

    private List<GameObject> GetRoomPrefabWithConnections()
    {
        List<GameObject> prefabRoomsWithConnections = new List<GameObject>();
        foreach (GameObject roomPrefab in m_roomPrefabs)
        {
            if (roomPrefab.GetComponent<DungeonRoom>().m_connectionPoints.Count > 1)
            {
                prefabRoomsWithConnections.Add(roomPrefab);
            }
        }

        return prefabRoomsWithConnections;
    }

    private List<GameObject> GetRoomPrefabWithDeadEnd()
    {
        List<GameObject> prefabRoomsWithConnections = new List<GameObject>();
        foreach (GameObject roomPrefab in m_roomPrefabs)
        {
            if (roomPrefab.GetComponent<DungeonRoom>().m_connectionPoints.Count == 1)
            {
                prefabRoomsWithConnections.Add(roomPrefab);
            }
        }

        return prefabRoomsWithConnections;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_existingRoom"></param>
    /// <param name="_newRoom"></param>
    /// <returns>True if all connection got connected</returns>
    private bool ConnectRooms(DungeonRoom _existingRoom, DungeonRoom _newRoom)
    {
        // Get random active connection
        List<DungeonRoomConnection> activeConnections = new List<DungeonRoomConnection>();
        foreach (DungeonRoomConnection roomConnection in _existingRoom.m_connectionPoints)
        {
            if (roomConnection.gameObject.activeSelf && !roomConnection.Connected)
                activeConnections.Add(roomConnection);
        }

        if (activeConnections.Count == 0)
            return true;
        DungeonRoomConnection existingRoomConnection = activeConnections[Random.Range(0, activeConnections.Count)];
        Debug.Log(existingRoomConnection, existingRoomConnection);

        // New room connection point that is available
        foreach (DungeonRoomConnection newRoomConnection in _newRoom.m_connectionPoints)
        {
            if (existingRoomConnection.gameObject.activeSelf && newRoomConnection.gameObject.activeSelf &&
                !newRoomConnection.Connected && !existingRoomConnection.Connected)
            {
                Debug.Log(newRoomConnection, newRoomConnection);
                // Set new room so both connections are on top of each other
                newRoomConnection.SetRoomPositionFromConnectionPosition(existingRoomConnection);
                // Rotate room to match connections (doors)
                newRoomConnection.RotateRoomToMatch(existingRoomConnection);
                // Give the rooms some information
                existingRoomConnection.Connected = true;
                newRoomConnection.Connected = true;
                existingRoomConnection.ConnectedDungeonRoom = _newRoom;
                newRoomConnection.ConnectedDungeonRoom = _existingRoom;
                return false;
            }
        }

        return true;
    }
}
