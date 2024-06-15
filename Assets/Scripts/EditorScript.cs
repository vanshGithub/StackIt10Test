using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class EditorScript : EditorWindow
{
    private GameObject[] prefabsToInstantiate = new GameObject[0];
    private GameObject parentObject;

    [MenuItem("Tools/Run Editor Script")]
    public static void ShowWindow()
    {
        GetWindow<EditorScript>("Prefab Instantiator");
    }

    private void OnGUI()
    {
        GUILayout.Label("Select the prefabs to instantiate in all scenes from Build Settings:", EditorStyles.boldLabel);

        int row = 0, column = 0;

        if (GUILayout.Button("Run Editor Script"))
        {          
            foreach (SnapPoint point in GameObject.Find("SnapPoints").GetComponentsInChildren<SnapPoint>())
            {
                point.column = column;
                point.row = row;

                row++;

                if (row == 6)
                {
                    row = 0;
                    column++;
                }

                EditorUtility.SetDirty(point);
            }
        }
    }
}