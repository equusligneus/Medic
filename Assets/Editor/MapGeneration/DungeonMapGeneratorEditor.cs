//using UnityEngine;
//using System.Collections;
//using UnityEditor;

//[CustomEditor(typeof(DungeonMapGenerator))]
//public class DungeonMapGeneratorEditor : Editor
//{
//    public override void OnInspectorGUI()
//    {
//        DrawDefaultInspector();

//        DungeonMapGenerator myTarget = (DungeonMapGenerator)target;

//        if (Application.isPlaying && GUILayout.Button("Create Room"))
//        {
//            myTarget.currentRoom = myTarget.AddRoom(myTarget.currentRoom);
//        }
//    }
//}