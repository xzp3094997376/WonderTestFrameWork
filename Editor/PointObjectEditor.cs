using System.Collections;
using System.Collections.Generic;

#if GTFramework
using GT.Main;
#endif

using UnityEditor;
using UnityEngine;

#if GTFramework
[CustomEditor(typeof(PointObject))]
public class PointObjectEditor: Editor
{
    private GameObject last;
    
    
    
    void OnSceneGUI1()
    {
        // get the chosen game object
        PointObject t = target as PointObject;

        if (t == null)
        {
            Debug.Log("No Point 选中");
            return;
        }

        if (last==t.transform.parent)
        {
            Debug.Log("no changge");//
            return;
        }
        last = t.transform.parent.gameObject;

        GameObject par= t.transform.parent.gameObject;
        Transform[] transforms= par.GetComponentsInChildren<Transform>(true);

        GUIStyle guiStyle= new GUIStyle();
        guiStyle.normal.background =null;
        guiStyle.normal.textColor = Color.green;
        guiStyle.fontSize = 20;
        
        
      
        
        
        for (int i = 0; i < transforms.Length; i++)
        {
            Vector3 center = transforms[i].position;
            Handles.Label(center + new Vector3(0.5f, 1f, 0),
                transforms[i].transform.name, guiStyle);
        
            Handles.color = Color.green;
            Handles.SphereHandleCap(1, transforms[i].transform.position, Quaternion.identity, HandleUtility.GetHandleSize(transforms[i].position), EventType.Repaint);


            if (transforms[i]==t.transform)
            {
                Vector3 pos = transforms[i].position;
                Quaternion qt = transforms[i].rotation;
                Handles.PositionHandle(pos, qt);
            }
            
        }
        // grab the center of the parent
      
    }
}
#endif
