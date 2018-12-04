using UnityEngine;
using UnityEngine.Networking;
using UnityEditor;
using System.Linq;
using System;
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

			if (obj.IsView ()) {
				Rect rect = new Rect(selectionRect);
				rect.x += rect.width - 34;
				rect.width = 16;
				if (obj.IsSubView ()) {
					if (obj.transform.parent.gameObject.IsSubView ()) {
						GameObject.DestroyImmediate (obj);
						return;
					}
					GUI.Label (rect,"S",guiStyle_SubView);
					obj.LockChild ();
					if (obj.IsSubViewed ()) {
						Framework.Core.Widget.SubView subView = obj.GetComponent<Framework.Core.Widget.SubView> ();
						subView.InitSubView ();
					} else {
						Framework.Core.Widget.SubView subView = obj.AddComponent<Framework.Core.Widget.SubView> ();
					}
				} else {
					obj.UnLockChild ();
					Framework.Core.Widget.SubView subView = obj.GetComponent<Framework.Core.Widget.SubView> ();
					if (subView != null) {
						GameObject.DestroyImmediate (subView);
					}
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

	public static bool IsView(this GameObject go){
		return go.CompareTag ("View");
	}

	public static bool IsSubViewed(this GameObject go){
		return go.GetComponent<Framework.Core.Widget.SubView>();
	}

	public static bool IsSubView(this GameObject go){
		if (!IsView (go))
			return false;
		
		Transform parent = go.transform.parent;
		while (!parent.gameObject.IsView ()) {
			if (parent.parent == null)
				return false;
			else
				parent = parent.parent;
		}
		return true;
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
