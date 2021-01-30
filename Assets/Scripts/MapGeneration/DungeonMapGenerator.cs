using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class DungeonMapGenerator : MonoBehaviour
{
    [Header("Options")]
    [SerializeField, Tooltip("Amount of rooms to generate (excluding starting room)")] private int m_targetRoomAmount; // favors building rooms in one direction (when there are no intersections), if only 1 connection left it will add more rooms until target reached
    [SerializeField] private DungeonRoom m_startingRoom;

    [Header("Debug")]
    [SerializeField] private List<GameObject> m_roomPrefabs = new List<GameObject>();
    [SerializeField, Space(15f)] private List<DungeonRoom> m_placedRooms;

    //debug
    public DungeonRoom currentRoom;

    public void Awake()
    {
        GenerateDungeon();  
    }

    //Will only search if rooms got default scale
    public bool RoomAlreadySetHere(Vector3 _here)
    {
        foreach (DungeonRoom dungeonRoom in m_placedRooms)
        {
            if (dungeonRoom.transform.position == _here)
                return true;
        }

        return false;
    }

    public void RemoveAllExistingRooms()
    {
        List<DungeonRoom> allRooms = FindObjectsOfType<DungeonRoom>().ToList();
        foreach (DungeonRoom room in allRooms)
        {
            if (room != m_startingRoom)
                GameObject.DestroyImmediate(room.gameObject);
        }

        foreach (DungeonRoomConnection connection in m_startingRoom.m_connectionPoints)
        {
            connection.Connected = false;
        }

        currentRoom = m_startingRoom;

        m_placedRooms.Clear();
    }

    public void GenerateDungeon()
    {
        RemoveAllExistingRooms();
        // Create new Dungeon
        List<DungeonRoom> roomsWithOpenConnection = new List<DungeonRoom> { m_startingRoom };
        //DungeonRoom currentRoom;
        while (roomsWithOpenConnection.Count > 0)
        {
            // use any room, sequence doesn't matter
            currentRoom = roomsWithOpenConnection[0];
            // create a new room
            DungeonRoom nextRoom;
            // if placedRooms + doors that aren't connected yet == targetRoomAmount, close all remaining doors, which will add the remaining rooms so placedRooms.Count == targetRoomAmount
            if (m_placedRooms.Count + roomsWithOpenConnection.Count >= m_targetRoomAmount) // Close off all remaining doors with dead end rooms
            {
                nextRoom = AddRoom(currentRoom, GetRoomPrefabWithDeadEnd());
            }
            // if only one open connection left and targetRoomAmount not reached add another notDeadEnd room
            else if (roomsWithOpenConnection.Count == 1 && m_placedRooms.Count - 1 < m_targetRoomAmount)
            {
                nextRoom = AddRoom(currentRoom, GetRoomPrefabWithConnections());
            }
            else
            {
                nextRoom = AddRoom(currentRoom, m_roomPrefabs);
            }

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

    public DungeonRoom AddRoom(DungeonRoom _startingRoom, List<GameObject> _roomPrefabs)
    {
        // Get random room of the prefabs
        DungeonRoom nextRoom = Instantiate(_roomPrefabs[Random.Range(0, _roomPrefabs.Count)]).GetComponent<DungeonRoom>();
        m_placedRooms.Add(nextRoom);
        bool stopGeneration = ConnectRooms(_startingRoom, nextRoom);
        return stopGeneration ? null : nextRoom;
    }

    private List<GameObject> GetRoomPrefabWithConnections()
    {
        // All rooms with at least 2 connections, where one of it will be connected immediately, leaving 1 free connection
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
        // All rooms with at least 1 connection, where there is no connection unoccupied after the room has been connected
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
