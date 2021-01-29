using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonMapGenerator : MonoBehaviour
{
    [Header("Options"), SerializeField] private int m_amountOfRooms;

    [SerializeField] private List<GameObject> m_roomPrefabs = new List<GameObject>();
    [SerializeField] private List<DungeonRoom> m_placedRooms = new List<DungeonRoom>(); // Not used yet, but might be useful to store
    [SerializeField] private DungeonRoom m_startingRoom;

    public void GenerateRooms()
    {
        // Remove existing rooms
        foreach (DungeonRoom room in m_placedRooms)
        {
            GameObject.Destroy(room.gameObject);
        }
        m_placedRooms.Clear();

        // Create new Dungeon

    }
}
