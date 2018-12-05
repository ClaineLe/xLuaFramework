using UnityEngine;
using UnityEngine.Networking;
using UnityEditor;
using System.Linq;
using System;
using Framework.Core.Assistant;


[InitializeOnLoad]
public class HierachyIconEditor
{
	private static GUIStyle guiStyle_View;
	private static GUIStyle guiStyle_SubView;
	static HierachyIconEditor()
	{
		guiStyle_View = new GUIStyle ();
		guiStyle_View.normal.textColor = Color.red;
		guiStyle_SubView = new GUIStyle ();
		guiStyle_SubView.normal.textColor = Color.blue;
		EditorApplication.hierarchyWindowItemOnGUI = null;
		EditorApplication.hierarchyWindowItemOnGUI += HierarchWindowOnGui;
    }


    // 绘制Hiercrch
    static void HierarchWindowOnGui(int instanceId, Rect selectionRect)
	{
		// 通过ID获得Obj
		GameObject obj = EditorUtility.InstanceIDToObject(instanceId) as GameObject;
		if (obj != null) {
			Rect rectCheck = new Rect(selectionRect);
			rectCheck.x += rectCheck.width - 20;
			rectCheck.width = 18;
			obj.SetActive(GUI.Toggle(rectCheck, obj.activeSelf, string.Empty));
			MonoView monoView = obj.GetComponent<MonoView> ();

			if (monoView != null) {
                Rect rect = new Rect(selectionRect);
				rect.x += rect.width - 34;
				rect.width = 16;
				if (monoView.ParentView != null) {
					obj.LockChild ();
					GUI.Label (rect,"S",guiStyle_SubView);
				} else {
					obj.UnLockChild ();
                    GUI.Label (rect,"V",guiStyle_View);
				}
			}

			Framework.Core.Widget.IWidget widget = obj.GetComponent<Framework.Core.Widget.IWidget>();
			if(widget != null){
				Rect rectRefName = new Rect(selectionRect);
				rectRefName.x += rectRefName.width - 130;
				rectRefName.width = 100;
				GUI.Label (rectRefName,widget.RefName);
			}
		}
	}
}
public static class ExtensionMethods
{
	public static bool HasComponent<T>(this GameObject go, bool checkChildren) where T : Component
	{
		if (!checkChildren)
		{
			return go.GetComponent<T>();
		}
		else
		{
			return go.GetComponentsInChildren<T>().FirstOrDefault() != null;
		}
	}


	#if UNITY_EDITOR
	public static void LockChild(this GameObject go){
		foreach (Transform child in go.GetComponentsInChildren<Transform>()) {
			if (!child.Equals (go.transform)) {
				child.gameObject.hideFlags = HideFlags.HideInHierarchy;
			}
		}
	}

	public static void UnLockChild(this GameObject go){
		foreach (Transform child in go.GetComponentsInChildren<Transform>()) {
			if (!child.Equals (go.transform)) {
				child.gameObject.hideFlags = HideFlags.None;
			}
		}
	}
	#endif

}
// EndScript //
