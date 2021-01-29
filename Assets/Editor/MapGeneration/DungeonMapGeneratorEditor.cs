#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(DungeonMapGenerator))]
public class DungeonMapGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        //DungeonMapGenerator myTarget = (DungeonMapGenerator)target;

        //if (Application.isPlaying && GUILayout.Button("Generate new Dungeon"))
        //{
        //    // For some reason doesn't work in editor, i guess its overriding the startRoom
        //    myTarget.GenerateRooms();
        //}
    }
}
#endif