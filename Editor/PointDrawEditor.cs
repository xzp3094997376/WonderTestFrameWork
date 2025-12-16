using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
#if GTFramework
using GT.Main;
#endif
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class PointDrawEditor
{
    static  PointDrawEditor()
    {
        /*Selection.selectionChanged -= Update;
        Selection.selectionChanged += Update;*/
    }

    static void Update()
    {
        /*SceneView.duringSceneGui -= DrawSphere;
        SceneView.duringSceneGui += DrawSphere;*/
    }

    private static void DrawSphere(SceneView sc)
    {
#if GTFramework
        for (int i = 0; i < Selection.gameObjects.Length; i++)
        {
            GameObject go= Selection.gameObjects[i];
            if (go!=null&&go.GetComponent<PointObject>()!=null)
            {
                //Debug.Log("绘画物体");
                Vector3 center = go.transform.position;
                Handles.Label(center+new Vector3(0.2f,0,0),go.transform.name + " : " + go.transform.position);
        
                Handles.color = Color.green;
                Handles.SphereHandleCap(1, go.transform.position, Quaternion.identity, HandleUtility.GetHandleSize(go.transform.position), EventType.Repaint);

                /*Vector3 pos = go.transform.position;
                Quaternion qt = go.transform.rotation;
                Handles.TransformHandle(ref pos, ref qt);*/
                
            }
        }
#endif
        
       
    }
    private void OnDisable()
    {
        EditorApplication.update-= Update;
    }
}
