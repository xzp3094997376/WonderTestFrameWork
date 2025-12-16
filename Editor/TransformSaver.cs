using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.Rendering;

[InitializeOnLoad]
public class TransformSaver
{
    [System.Serializable]
    public struct TransformData
    {
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;
        public string objPath;

        public TransformData(Vector3 pos, Quaternion rot, Vector3 scl,string _path)
        {
            position = pos;
            rotation = rot;
            scale = scl;
            objPath = _path;
        }
    }
    
    private static Dictionary<string, TransformData> savedTransforms = new Dictionary<string, TransformData>();

    static TransformSaver()
    {
        EditorApplication.playModeStateChanged += LogPlayModeState;
    }

    private static void LogPlayModeState(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.ExitingPlayMode)
        {
            foreach (var kvp in savedTransforms)
            {
                EditorPrefs.SetFloat(kvp.Key + "PosX", kvp.Value.position.x);
                EditorPrefs.SetFloat(kvp.Key + "PosY", kvp.Value.position.y);
                EditorPrefs.SetFloat(kvp.Key + "PosZ", kvp.Value.position.z);

                EditorPrefs.SetFloat(kvp.Key + "RotX", kvp.Value.rotation.x);
                EditorPrefs.SetFloat(kvp.Key + "RotY", kvp.Value.rotation.y);
                EditorPrefs.SetFloat(kvp.Key + "RotZ", kvp.Value.rotation.z);
                EditorPrefs.SetFloat(kvp.Key + "RotW", kvp.Value.rotation.w);

                EditorPrefs.SetFloat(kvp.Key + "ScaleX", kvp.Value.scale.x);
                EditorPrefs.SetFloat(kvp.Key + "ScaleY", kvp.Value.scale.y);
                EditorPrefs.SetFloat(kvp.Key + "ScaleZ", kvp.Value.scale.z);
            }
        }
        else if (state == PlayModeStateChange.EnteredEditMode)
        {
            foreach (var kvp in savedTransforms)
            {
                GameObject go = GameObject.Find(kvp.Key);
                if (go==null)
                {
                    go = FindGameObjectByPath(kvp.Value.objPath);
                }
                if (go != null)
                {
                    float posX = EditorPrefs.GetFloat(kvp.Key + "PosX", 0);
                    float posY = EditorPrefs.GetFloat(kvp.Key + "PosY", 0);
                    float posZ = EditorPrefs.GetFloat(kvp.Key + "PosZ", 0);
                    go.transform.position = new Vector3(posX, posY, posZ);

                    float rotX = EditorPrefs.GetFloat(kvp.Key + "RotX", 0);
                    float rotY = EditorPrefs.GetFloat(kvp.Key + "RotY", 0);
                    float rotZ = EditorPrefs.GetFloat(kvp.Key + "RotZ", 0);
                    float rotW = EditorPrefs.GetFloat(kvp.Key + "RotW", 1);
                    go.transform.rotation = new Quaternion(rotX, rotY, rotZ, rotW);

                    float scaleX = EditorPrefs.GetFloat(kvp.Key + "ScaleX", 1);
                    float scaleY = EditorPrefs.GetFloat(kvp.Key + "ScaleY", 1);
                    float scaleZ = EditorPrefs.GetFloat(kvp.Key + "ScaleZ", 1);
                    go.transform.localScale = new Vector3(scaleX, scaleY, scaleZ);
                    
                    Debug.Log($"set transform for {go.name}: Position: {go.transform.position}, Rotation: {go.transform.rotation}, Scale: {go.transform.localScale}");
                }
            }
        }
    }

    [MenuItem("快捷键/Save Transform %&s")] // Ctrl + alt + S
    private static void SaveSelectedTransforms()
    {
        foreach (GameObject go in Selection.gameObjects)
        {
            SaveTransform(go);
        }
    }

    private static void SaveTransform(GameObject go)
    {
        if (go != null)
        {
            // 保存 GameObject 的变换信息
            savedTransforms[go.name] = new TransformData(go.transform.position, go.transform.rotation, go.transform.localScale,GetGameObjectPath(go));
            Debug.Log($"Saved transform for {go.name}: Position: {go.transform.position}, Rotation: {go.transform.rotation}, Scale: {go.transform.localScale}");
        }
    }
    
    public static string GetGameObjectPath(GameObject obj)
    {
        string path = obj.name;
        Transform parentTransform = obj.transform.parent;

        while (parentTransform != null)
        {
            path = parentTransform.name + "/" + path;
            parentTransform = parentTransform.parent;
        }
        Debug.Log(path);
        return path;
    }
    
    public static GameObject FindGameObjectByPath(string path)
    {
        // 从场景的根物体开始查找
        Transform targetTransform = null;
        
        // 将路径按“/”分割成各个部分
        string[] pathParts = path.Split('/');
        
        // 开始查找路径中的每一层
        targetTransform = GameObject.Find(pathParts[0]).transform;
       string childPath= path.Substring(pathParts[0].Length + 1);

       targetTransform = targetTransform.Find(childPath);
       if (targetTransform == null)
       {
           Debug.LogWarning("找不到路径中的物体: " + childPath);
           return null;
       }
        /*for (int i = 1; i < pathParts.Length; i++)
        {
            // 查找子物体
            targetTransform = targetTransform.Find(pathParts[i]);
            
            // 如果找不到任何物体，返回null
            if (targetTransform == null)
            {
                Debug.LogWarning("找不到路径中的物体: " + pathParts[i]);
                return null;
            }
        }*/

        return targetTransform.gameObject;
    }

}