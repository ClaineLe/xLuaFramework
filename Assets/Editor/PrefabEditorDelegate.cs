using Framework.Core.Assistant;
using UnityEditor;
using UnityEngine;

public class PrefabEditorDelegate
{
    [InitializeOnLoadMethod]
    static void StartInitializeOnLoadMethod()
    {
        // 注册Apply时的回调
        PrefabUtility.prefabInstanceUpdated = delegate (GameObject instance)
        {
            MonoView monoView = instance.GetComponent<MonoView>();
            if (instance != null && monoView != null)
            {
                EditorUtility.SetDirty(monoView);
            }
        };
    }
}
