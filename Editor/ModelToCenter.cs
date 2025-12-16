using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;

public class ModelToCenter {
    [MenuItem("GameObject/Tool/Model/模型中心点复位")]
    static void Test()
    {
        Transform parent = Selection.activeGameObject.transform;
        Vector3 postion = parent.position;
        Quaternion rotation = parent.rotation;
        Vector3 scale = parent.localScale;
        parent.position = Vector3.zero;
        parent.rotation = Quaternion.Euler(Vector3.zero);
        parent.localScale = Vector3.one;//父物体归零


        Vector3 center = Vector3.zero;
        Renderer[] renders = parent.GetComponentsInChildren<Renderer>();
        foreach (Renderer child in renders)
        {
            center += child.bounds.center;//  
        }
        center /= parent.GetComponentsInChildren<Transform>().Length;
        Bounds bounds = new Bounds(center, Vector3.zero);
        foreach (Renderer child in renders)//计算（归零的父物体下）子物体位置
        {
            bounds.Encapsulate(child.bounds);
        }

        Debug.LogError(renders.Length);


        parent.position = postion; //父物体复原位置
        parent.rotation = rotation;
        parent.localScale = scale;

        foreach (Transform t in parent)//将子物体放在复原的的父物体下面
        {
            t.position = t.position - bounds.center;//0+向量差值
        }
        parent.transform.position = bounds.center + parent.position;//center+相对位置

  

    }

    /// <summary>
    /// Ctrl+Shift+C 复制组件
    /// </summary>
    static  List<Type> _coms = new List<Type>();
    [MenuItem("GameObject/Tool/拷贝所有组件")]
    static void MyCopyComponent()
    {
        _coms.Clear();
        GameObject go= Selection.activeGameObject;
        Component[] _components= go.GetComponents<Component>();
        Debug.Log(_components.Length);
        foreach (var _component in _components)
        {
            Debug.Log(_component);
            if (_component is Transform)
            {
                continue;
            }
            _coms.Add(_component.GetType());
        }
    }
    
    /// <summary>
    /// Ctrl+Shift+V 选中物体粘贴组件
    /// </summary>
    [MenuItem("GameObject/Tool/粘贴所有组件")]
    static void MyPasteComponent()
    {
        GameObject go= Selection.activeGameObject;
        foreach (Type _type in _coms)
        {
            Debug.Log(_type);
           
            if (go.GetComponent(_type)==null)
            {
                go.AddComponent(_type);
            }
        }
    }
    
    static Vector3 worldPos;
    [MenuItem("CONTEXT/Transform/拷贝世界坐标")]
    static void CopyWorldPos()
    {
        GameObject seleObj = Selection.activeGameObject;
        worldPos = seleObj.transform.position;
    }
    [MenuItem("CONTEXT/Transform/粘贴世界坐标")]
    static void PasteWorldPos()
    {
        GameObject seleObj = Selection.activeGameObject;
        seleObj.transform.position = worldPos;
    }
}
