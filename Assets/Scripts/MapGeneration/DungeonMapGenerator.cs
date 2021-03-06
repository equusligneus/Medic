﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public struct MinMaxMap
{
    public int maxX;
    public int minX;
    public int maxZ;
    public int minZ;

    public MinMaxMap(Vector3 _position)
    {
        maxX = (int)_position.x + 5;
        minX = (int)_position.x - 5;
        maxZ = (int)_position.z + 5;
        minZ = (int)_position.z - 5;
    }

    public void SetMinMax(Vector3 _position)
    {
        if (_position.x < minX) minX = (int)_position.x - 5;
        if (_position.x > maxX) maxX = (int)_position.x + 5;
        if (_position.z < minZ) minZ = (int)_position.z - 5;
        if (_position.z > maxZ) maxZ = (int)_position.z + 5;
    }

    public int GetWidth()
    {
        return (int)Mathf.Abs(minX + maxX) + 40;
    }

    public int GetHeight()
    {
        return (int)Mathf.Abs(minZ + maxZ) + 40;
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
    [SerializeField, Tooltip("Amount of rooms to generate (excluding starting room)")] private int m_targetRoomAmount;

    public static MinMaxMap MapSize;
    [SerializeField] private DungeonRoom m_startDungeonRoom;
    [SerializeField] private List<GameObject> m_roomPrefabs = new List<GameObject>();

    [Header("Debug")]
    [SerializeField, Space(15f)] private List<DungeonRoom> m_placedRooms = new List<DungeonRoom>();

    public DungeonRoom m_currentRoom;

    private void Awake()
    {
        MapSize = new MinMaxMap(m_startDungeonRoom != null ? m_startDungeonRoom.transform.position : Vector3.zero);
    }

    public void SetStartRoom(DungeonRoom _startRoom)
    {
        m_startDungeonRoom = _startRoom;
    }

    #region Helper
    /// <summary>
    /// Moves parent of child2 so child1 and child2 position match
    /// </summary>
    /// <param name="_child1">child to stay</param>
    /// <param name="_child2">child being moved</param>
    private void MoveParentsByChildren(Transform _child1, Transform _child2)
    {
        Vector3 finalPos = _child1.position;
        Vector3 parent2Offset = _child2.parent.position - _child2.position;
        _child2.parent.position = finalPos + parent2Offset;
    }

    /// <summary>
    /// Rotate connection2 parent so connection2 is the inverse to connection1 in world space
    /// </summary>
    /// <param name="_connection1">Connection to stay</param>
    /// <param name="_connection2">Connection which s parent will be rotated</param>
    private void RotateParentToMatchDirOfConnections(DungeonRoomConnection _connection1, DungeonRoomConnection _connection2)
    {
        Vector3 dir1 = _connection1.transform.TransformDirection(_connection1.m_roomConnectionRotation);
        Vector3 dir2 = _connection2.transform.TransformDirection(_connection2.m_roomConnectionRotation);
        float angle = Vector3.SignedAngle(dir1, dir2, Vector3.up);

        _connection2.transform.parent.RotateAround(_connection1.transform.position, Vector3.up, -angle + 180);
    }

    public void RemoveAllExistingRooms()
    {
        List<DungeonRoom> allRooms = FindObjectsOfType<DungeonRoom>().ToList();
        foreach (DungeonRoom room in allRooms)
        {
            if (room != m_startDungeonRoom)
                GameObject.Destroy(room.gameObject);
        }
        foreach (DungeonRoomConnection connection in m_startDungeonRoom.m_connectionPoints)
        {
            connection.Connected = false;
        }
        m_placedRooms.Clear();
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
    #endregion

    [ContextMenu("Generate Dungeon")]
    public void GenerateDungeon(int _targetAmountOfRooms = 0)
    {
        // When no room size set, use the inspector one
        if (_targetAmountOfRooms == 0)
        {
            _targetAmountOfRooms = m_targetRoomAmount;
        }

        RemoveAllExistingRooms();
        m_currentRoom = m_startDungeonRoom;

        // Create new Dungeon
        List<DungeonRoom> roomsWithOpenConnection = new List<DungeonRoom> { m_currentRoom };

        int notWhileTrue = 1000;
        while (notWhileTrue-- > 0 && roomsWithOpenConnection.Count > 0)
        {
            m_currentRoom = roomsWithOpenConnection[0];
            DungeonRoom nextRoom = null;

            if (m_placedRooms.Count + roomsWithOpenConnection.Count >= _targetAmountOfRooms)
            {
                nextRoom = AddRoom(m_currentRoom, GetRoomPrefabWithDeadEnd());
            }
            else if (roomsWithOpenConnection.Count == 1 && m_placedRooms.Count - 1 < _targetAmountOfRooms)
            {
                nextRoom = AddRoom(m_currentRoom, GetRoomPrefabWithConnections());
            }
            else
            {
                nextRoom = AddRoom(m_currentRoom, m_roomPrefabs);
            }

            if (nextRoom == null)
            {
                if (m_currentRoom.AllConnected)
                {
                    roomsWithOpenConnection.Remove(m_currentRoom);
                }
                continue;
            }

            if (!nextRoom.AllConnected && !roomsWithOpenConnection.Contains(nextRoom))
            {
                roomsWithOpenConnection.Add(nextRoom);
            }

            if (m_currentRoom.AllConnected)
            {
                roomsWithOpenConnection.Remove(m_currentRoom);
            }
        }
    }

    private DungeonRoom AddRoom(DungeonRoom _startRoom, List<GameObject> _roomPrefabs)
    {
        if (GetActiveConnections(_startRoom).Count > 0)
        {
            // TODO: Check if allowed to place here
            DungeonRoom nextRoom = Instantiate(_roomPrefabs[Random.Range(0, _roomPrefabs.Count)])
                .GetComponent<DungeonRoom>();
            nextRoom = ConnectRooms(_startRoom, nextRoom);
            if (nextRoom != null)
                m_placedRooms.Add(nextRoom);
            return nextRoom;
        }
        else
        {
            Debug.Log("This shouldn't happen");
            return null;
        }
    }

    private DungeonRoom ConnectRooms(DungeonRoom _startRoom, DungeonRoom _nextRoom)
    {
        bool isCollision = false;

        DungeonRoomConnection startRoomConnection = GetActiveConnections(_startRoom)[0];

        DungeonRoomConnection nextRoomConnection = GetActiveConnections(_nextRoom)[Random.Range(0, GetActiveConnections(_nextRoom).Count)];

        MoveParentsByChildren(startRoomConnection.transform, nextRoomConnection.transform);

        RotateParentToMatchDirOfConnections(startRoomConnection, nextRoomConnection);

        // Collision check
        foreach (DungeonRoom placedRoom in m_placedRooms)
        {
            if (_nextRoom != placedRoom &&
                // Normal room size compare center to other rooms
                (!_nextRoom.LargeRoomX && !_nextRoom.LargeRoomZ && Vector3.Distance(_nextRoom.transform.position, placedRoom.transform.position) < 9.9f) ||
                // LargeRoomX compare center, -x, +x to other rooms
                (Vector3.Distance(_nextRoom.transform.position, placedRoom.transform.position) < 9.9f &&
                 (Vector3.Distance(_nextRoom.transform.position + new Vector3(3, 0, 0), placedRoom.transform.position) < 9.9f &&
                 Vector3.Distance(_nextRoom.transform.position + new Vector3(-3, 0, 0), placedRoom.transform.position) < 9.9f) &&
                 // LargeRoomZ compare center, -z, +z to other rooms
                 (Vector3.Distance(_nextRoom.transform.position + new Vector3(0, 0, 3), placedRoom.transform.position) < 9.9f &&
                 Vector3.Distance(_nextRoom.transform.position + new Vector3(0, 0, -3), placedRoom.transform.position) < 9.9f)
                ))
                // TODO: Collision
                isCollision = true;
            else
            {
                Debug.LogWarning("This shouldn't happen", placedRoom);
            }
        }

        // infos
        if (!isCollision)
        {
            startRoomConnection.Connected = true;
            nextRoomConnection.Connected = true;
            startRoomConnection.ConnectedDungeonRoom = _nextRoom;
            nextRoomConnection.ConnectedDungeonRoom = _startRoom;

            MapSize.SetMinMax(_nextRoom.transform.position);
            return _nextRoom;
        }
        else
        {
            PlaceDoor(startRoomConnection);
            m_placedRooms.Remove(_nextRoom);
            Debug.Log("Overlap avoided here", _startRoom);
            DestroyImmediate(_nextRoom.gameObject);
            return null;
        }
    }

    private void PlaceDoor(DungeonRoomConnection _connectionToPlace)
    {
        ////Debug.Log(_connectionToPlace.m_roomConnectionRotation);
        _connectionToPlace.Connected = true;
        //GameObject door = Instantiate(m_doorPrefab, _connectionToPlace.transform.parent);

        //door.transform.position = _connectionToPlace.transform.position;

        _connectionToPlace.DoorMayOpen = false;
    }
}
