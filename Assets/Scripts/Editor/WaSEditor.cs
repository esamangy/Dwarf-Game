using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof(StrengthAndWeakness))]
public class WaSEditor : Editor{
    public override void OnInspectorGUI(){
        // MapGenerator mapGen = (MapGenerator)target;

        // if(DrawDefaultInspector()){
        //     if(mapGen.autoUpdate){
        //         mapGen.DrawMapInEditor();
        //     }
        // }

        if(GUILayout.Button("Update Table")){
            //mapGen.DrawMapInEditor();
        }
    }
}
