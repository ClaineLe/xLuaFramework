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
        if (obj != null && obj.transform.root.name == "ViewRoot")
        {
            if (obj.name == "Root" && Event.current.type == EventType.DragExited)
            {
                for (int i = 0; i < obj.transform.childCount;i++)
                {
                    MonoView tmpMonoView = obj.transform.GetChild(i).GetComponent<MonoView>();
                    if (tmpMonoView != null && tmpMonoView.IsChangeInHierarchy())
                    {
                        tmpMonoView.Refresh();
                    }
                }
            }
            Rect rectCheck = new Rect(selectionRect);
			rectCheck.x += rectCheck.width - 20;
			rectCheck.width = 18;
			obj.SetActive(GUI.Toggle(rectCheck, obj.activeSelf, string.Empty));

            Framework.Core.Widget.IWidget widget = obj.GetComponent<Framework.Core.Widget.IWidget>();
            if (widget != null)
            {
                Rect rectRefName = new Rect(selectionRect);
                rectRefName.x += rectRefName.width - 130;
                rectRefName.width = 100;
                GUI.Label(rectRefName, widget.RefName);
            }


            MonoView monoView = widget as MonoView;
			if (monoView != null) {
                Rect rect = new Rect(selectionRect);
				rect.x += rect.width - 34;
				rect.width = 16;
				if (monoView.ParentView != null) {
					GUI.Label (rect,"S",guiStyle_SubView);
				} else {
                    GUI.Label (rect,"V",guiStyle_View);
				}
			}

		}
	}
}
// EndScript //
