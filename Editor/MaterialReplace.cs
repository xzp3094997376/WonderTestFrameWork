using UnityEditor;
using UnityEngine;

public class MaterialReplacerWindow : EditorWindow
{
    private GameObject selectedGameObject;
    private Material originalMaterial;
    private Material replacementMaterial;
    private bool replaceAllChildren = false;

    [MenuItem("Tools/Material Replacer")]
    static void Init()
    {
        GetWindow<MaterialReplacerWindow>("Material Replacer");
    }

    void OnGUI()
    {
        GUILayout.Label("Material Replacer", EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();
        selectedGameObject = EditorGUILayout.ObjectField("Selected GameObject", selectedGameObject, typeof(GameObject), true) as GameObject;
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        originalMaterial = EditorGUILayout.ObjectField("Original Material", originalMaterial, typeof(Material), false) as Material;
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        replacementMaterial = EditorGUILayout.ObjectField("Replacement Material", replacementMaterial, typeof(Material), false) as Material;
        EditorGUILayout.EndHorizontal();

        replaceAllChildren = EditorGUILayout.Toggle("Replace All Children", replaceAllChildren);

        if (GUILayout.Button("Replace Material"))
        {
            ReplaceSelectedMaterial(originalMaterial, replacementMaterial);
        }
    }

    private void ReplaceSelectedMaterial(Material originalMaterial, Material replacementMaterial)
    {
        if (selectedGameObject == null || originalMaterial == null || replacementMaterial == null)
        {
            Debug.LogWarning("Please select a valid game object and both original and replacement materials.");
            return;
        }

        MeshRenderer[] renderers = replaceAllChildren ?
            selectedGameObject.GetComponentsInChildren<MeshRenderer>(true) :
            selectedGameObject.GetComponents<MeshRenderer>();

        foreach (MeshRenderer renderer in renderers)
        {
            Material[] sharedMaterials = renderer.sharedMaterials;
            for (int i = 0; i < sharedMaterials.Length; i++)
            {
                if (sharedMaterials[i] == originalMaterial)
                {
                    sharedMaterials[i] = replacementMaterial;
                }
            }
            renderer.sharedMaterials = sharedMaterials;
        }

        Debug.Log($"Material replaced on {renderers.Length} renderers.");
    }
}
