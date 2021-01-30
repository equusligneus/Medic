using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public struct minMaxMap
{
    public int maxX;
    public int minX;
    public int maxZ;
    public int minZ;

    public minMaxMap(Vector3 _position)
    {
        maxX = (int) _position.x + 5;
        minX = (int) _position.x - 5;
        maxZ = (int) _position.z + 5;
        minZ = (int) _position.z - 5;
    }

    public void SetMinMax(Vector3 _position)
    {
        if (_position.x < minX) minX = (int) _position.x - 5;
        if (_position.x > maxX) maxX = (int) _position.x + 5;
        if (_position.z < minZ) minZ = (int) _position.z - 5;
        if (_position.z > maxZ) maxZ = (int) _position.z + 5;
    }

    public Vector3 GetCenter
    {
        get
        {
            return new Vector3(minX + maxX, 0, minZ + maxZ) / 2;
        }
    }
}

public class DungeonMapGenerator : MonoBehaviour
{
    [Header("Options")]
    [SerializeField, Tooltip("Amount of rooms to generate (excluding starting room)")] private int m_targetRoomAmount; // favors building rooms in one direction (when there are no intersections), if only 1 connection left it will add more rooms until target reached
    [SerializeField] private DungeonRoom m_startingRoom;
    [SerializeField] private GameObject m_doorPrefab;

    [SerializeField] private List<GameObject> m_roomPrefabs = new List<GameObject>();
    [Header("Debug")]
    [SerializeField, Space(15f)] private List<DungeonRoom> m_placedRooms;

    [HideInInspector] public Vector3 Center;

    /*[HideInInspector] */public minMaxMap MapSize;

    //debug
    public DungeonRoom currentRoom;

    public void Awake()
    {
        MapSize = new minMaxMap(m_startingRoom.transform.position);
        MapSize.SetMinMax(m_startingRoom.transform.position);

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

        m_placedRooms.Clear();
    }

    public void GenerateDungeon()
    {
        currentRoom = m_startingRoom;
        RemoveAllExistingRooms();
        // Create new Dungeon
        List<DungeonRoom> roomsWithOpenConnection = new List<DungeonRoom> { m_startingRoom };
        //DungeonRoom currentRoom;
        while (roomsWithOpenConnection.Count > 0)
        {
            // use any room, sequence doesn't matter
            currentRoom = roomsWithOpenConnection[0];
            // create a new room
            DungeonRoom nextRoom = null;
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
            if (nextRoom != null && currentRoom.AllConnected)
            {
                roomsWithOpenConnection.Remove(currentRoom);
            }

            if (currentRoom.AllConnected)
                roomsWithOpenConnection.Remove(currentRoom);

            // no room with unconnected door left -> break
            if (roomsWithOpenConnection.Count == 0) break;
        }
    }

    private void PlaceDoor(DungeonRoomConnection _connectionToPlace)
    {
        _connectionToPlace.Connected = true;
        //TODO: place door here
        GameObject door = Instantiate(m_doorPrefab, _connectionToPlace.transform.parent);

        //Debug.Log(door, door);
    }

    public DungeonRoom AddRoom(DungeonRoom _startingRoom, List<GameObject> _roomPrefabs)
    {
        bool allowedToPlaceHere = true;
        // See if no collision - expensive but works (hopefully) and is only called once on generation
        if (GetActiveConnections(_startingRoom).Count > 0)
        {
            Vector3 nextRoomPos = _startingRoom.transform.position + DungeonRoomConnection.VecFromToPoint(
                    _startingRoom.transform.position,
                    GetActiveConnections(_startingRoom)[0].transform.position) *
                2; // 2 times distance from center to connection = nextRoom normal size center
            foreach (DungeonRoom placedRoom in m_placedRooms)
            {
                // Check for overlap at nextRoomPos
                if (placedRoom.transform.position == nextRoomPos)
                {
                    // Overlap detected here; placedRoom was there first; GetActiveConnections(_startingRoom)[0] is where the door needs to be placed
                    PlaceDoor(GetActiveConnections(_startingRoom)[0]);
                    allowedToPlaceHere = false;
                    //return null;
                }

                //if (Vector3.Distance(placedRoom.transform.position, _startingRoom.transform.position) <= 10)
                //{
                //    //
                //    Debug.Log("Don't place here");
                //}
            }

            // Get random room of the prefabs

            if (allowedToPlaceHere)
            {
                DungeonRoom nextRoom = Instantiate(_roomPrefabs[Random.Range(0, _roomPrefabs.Count)])
                    .GetComponent<DungeonRoom>();
                m_placedRooms.Add(nextRoom);
                bool stopGeneration = ConnectRooms(_startingRoom, nextRoom);
                return stopGeneration ? null : nextRoom;
            }
            else
            {
                //Debug.Log(_startingRoom);
            }
        }
        else
        {
            Debug.Log(_startingRoom.m_connectionPoints);
        }

        return null;
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

    private List<DungeonRoomConnection> GetActiveConnections(DungeonRoom _room)
    {
        List<DungeonRoomConnection> activeConnections = new List<DungeonRoomConnection>();
        foreach (DungeonRoomConnection roomConnection in _room.m_connectionPoints)
        {
            if (roomConnection.gameObject.activeSelf && !roomConnection.Connected)
                activeConnections.Add(roomConnection);
        }

        return activeConnections;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_existingRoom"></param>
    /// <param name="_newRoom"></param>
    /// <returns>True if all connection got connected</returns>
    private bool ConnectRooms(DungeonRoom _existingRoom, DungeonRoom _newRoom, bool _largeRoomAllowed = false)
    {
        // Get first active connection
        List<DungeonRoomConnection> activeConnections = GetActiveConnections(_existingRoom);

        if (activeConnections.Count == 0)
            return true;
        //DungeonRoomConnection existingRoomConnection = activeConnections[Random.Range(0, activeConnections.Count)];
        DungeonRoomConnection existingRoomConnection = activeConnections[0];
        //Debug.Log(existingRoomConnection, existingRoomConnection);

        // New room connection point that is available
        foreach (DungeonRoomConnection newRoomConnection in _newRoom.m_connectionPoints)
        {
            if (existingRoomConnection.gameObject.activeSelf && newRoomConnection.gameObject.activeSelf &&
                !newRoomConnection.Connected && !existingRoomConnection.Connected)
            {
                //Debug.Log(newRoomConnection, newRoomConnection);
                // Set new room so both connections are on top of each other
                newRoomConnection.SetRoomPositionFromConnectionPosition(existingRoomConnection);
                // Rotate room to match connections (doors)
                newRoomConnection.RotateRoomToMatch(existingRoomConnection);
                // Give the rooms some information
                existingRoomConnection.Connected = true;
                newRoomConnection.Connected = true;
                existingRoomConnection.ConnectedDungeonRoom = _newRoom;
                newRoomConnection.ConnectedDungeonRoom = _existingRoom;

                MapSize.SetMinMax(_newRoom.transform.position);
                return false;
            }
        }

        return true;
    }
}
