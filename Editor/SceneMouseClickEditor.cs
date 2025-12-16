using UnityEditor;
using UnityEngine;

[InitializeOnLoad] // 确保编辑器启动时自动运行
public class SceneMouseClickEditor
{
    static SceneMouseClickEditor()
    {
        // 注册一个回调，在每一帧都检查Scene视图的鼠标事件
        SceneView.duringSceneGui += OnSceneGUI;
    }

    private static void OnSceneGUI(SceneView sceneView)
    {
        // 获取鼠标事件
        Event currentEvent = Event.current;

        // 判断是否是鼠标点击事件
        if (currentEvent.type == EventType.MouseDown&&currentEvent.shift)
        {
            // 获取鼠标点击的位置
            Vector2 mousePosition = currentEvent.mousePosition;

            // 转换为世界空间坐标
            Ray ray = HandleUtility.GUIPointToWorldRay(mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                // 你可以在这里处理击中的物体
                GameObject GOTarget =Selection.activeGameObject;
                if (GOTarget!=null)
                {
                    if (EditorUtility.DisplayDialog("创建物体","创建"+GOTarget.name,"确定","取消"))
                    {
                        GameObject obj = (GameObject)GameObject.Instantiate(GOTarget,GOTarget.transform.position,GOTarget.transform.rotation,GOTarget.transform.parent);
                        EditorUtility.SetDirty(obj);
                    }
                    else
                    {
                        Debug.Log("取消创建");
                    }
                   
                    Debug.Log("Clicked on: " + hit.collider.name);
                }
            }

            // 处理完后确保事件不继续传递（避免Unity默认行为）
            currentEvent.Use();
        }
    }
}
