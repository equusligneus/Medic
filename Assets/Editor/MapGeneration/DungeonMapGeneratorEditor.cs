﻿#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(DungeonMapGenerator))]
public class DungeonMapGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        DungeonMapGenerator myTarget = (DungeonMapGenerator)target;

        if (GUILayout.Button("Create Dungeon"))
        {
            myTarget.GenerateDungeon();
        }

        if (GUILayout.Button("Remove All Rooms"))
        {
            myTarget.RemoveAllExistingRooms();
        }
    }
}
#endif