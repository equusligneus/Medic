using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(DungeonRoomConnection))]
public class DungeonRoomConnectionEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        DungeonRoomConnection myTarget = (DungeonRoomConnection)target;

        if (Application.isPlaying && GUILayout.Button("RotateRoom"))
        {
            myTarget.RotateRoomToMatch(myTarget.temp);
        }
    }
}