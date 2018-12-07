using UnityEngine;
using UnityEditor;
using Framework.Core.Widget;
using Framework.Game;
using Framework.Core.Assistant;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.IO;

namespace Framework.Editor
{
	namespace Widget
	{
		[CustomEditor(typeof(MonoView))]
		public class MonoViewInspector : UnityEditor.Editor
		{
			private MonoView m_Target{
				get{
					return target as MonoView;
				}
			}

            private bool showSubViewList = true;
            private bool showWidgetList = true;

            public const string basePrefabPath = "Assets/" + PathConst.ExportResDirPath + PathConst.ViewRoot_BasePath;
            public const string baseLuaFilePath = "Assets/" + PathConst.ExportResDirPath + PathConst.FORMAT_LUAROOT;

            private List<UIBehaviour> _WidgetList {
                get {
                    return m_Target._widgets.FindAll(a => !(a is MonoView));
                }
            }

            private List<UIBehaviour> _SubViewList {
                get {
                    return m_Target._widgets.FindAll(a => (a is MonoView));
                }
            }

            

            private void FoldOutPageList(string title, List<UIBehaviour> list, bool canSelect,ref bool foldoutState)
            {
                if (list.Count <= 0)
                    return;
                foldoutState = EditorGUILayout.Foldout(foldoutState, title);
                if (foldoutState)
                {
                    using (EditorGUI.DisabledScope ds = new EditorGUI.DisabledScope(canSelect))
                    {
                        for (int i = 0; i < list.Count; i++)
                        {
                            IWidget widget = list[i].GetComponent<IWidget>();
                            using (GUILayout.HorizontalScope hs_0 = new GUILayout.HorizontalScope())
                            {
                                GUILayout.Label(widget.RefName, GUILayout.Width(100));
                                if (GUILayout.Button((widget as UIBehaviour).name + string.Format(" ({0})", widget.GetType().Name), "LargeTextField"))
                                {
                                    EditorGUIUtility.PingObject(list[i].GetInstanceID());
                                }
                            }
                        }
                    }
                }
            }
            protected override void OnHeaderGUI()
            {
                WidgetInspector.DrawHeaderGUI(target);
                using (GUILayout.VerticalScope vs = new GUILayout.VerticalScope("IN GameObjectHeader"))
                {
                    Object prefabAsset = PrefabUtility.GetPrefabParent(target);
                    DrawPrefabInfo(target, prefabAsset);
                    DrawLuaFileInfo(target, prefabAsset);
                }
            }
            public override void OnInspectorGUI()
			{
                base.DrawHeader();
                if (m_Target._widgets != null && m_Target._widgets.Count > 0)
                {
                    EditorGUILayout.Space();
                    FoldOutPageList("【子面板】", _SubViewList, m_Target.ParentView, ref showSubViewList);
                    FoldOutPageList("【控件】", _WidgetList, m_Target.ParentView, ref showWidgetList);
                }
            }

            private static void DrawPrefabInfo(Object target, Object prefabAsset)
            {
                if (prefabAsset == null)
                {
                    using (GUILayout.HorizontalScope hs_0 = new GUILayout.HorizontalScope("HelpBox"))
                    {
                        GUILayout.Label(string.Empty, "CN EntryErrorIcon");
                        using (GUILayout.VerticalScope vs_1 = new GUILayout.VerticalScope("HelpBox"))
                        {
                            string prefabName = target.name + ".prefab";
                            string fullPath = basePrefabPath + prefabName;
                            GUILayout.Label("【面板预制名字】" + prefabName, "HeaderLabel");
                            GUILayout.Label("【目标路径预览】" + fullPath);
                            if (GUILayout.Button("创建面板Prefab"))
                            {
                                GameObject prefab = (target as UIBehaviour).gameObject;
                                PrefabUtility.CreatePrefab(fullPath, prefab, ReplacePrefabOptions.ConnectToPrefab);

                                Selection.activeObject = target;
                                EditorGUIUtility.PingObject(target);
                            }
                        }
                    }
                }
                else
                {
                    using (GUILayout.HorizontalScope hs_0 = new GUILayout.HorizontalScope())
                    {
                        GUILayout.Label("【预置物路径】", GUILayout.Width(100));
                        if (GUILayout.Button(prefabAsset.name + ".prefab", "LargeTextField"))
                        {
                            EditorGUIUtility.PingObject((prefabAsset as MonoView).gameObject.GetInstanceID());
                        }
                    }
                }
            }

            private static void DrawLuaFileInfoBase(string viewLuaFileName, string title)
            {
                string viewLuaFileFullPath = baseLuaFilePath + viewLuaFileName;

                FileInfo luaFileInfo = new FileInfo(viewLuaFileFullPath);
                if (luaFileInfo.Exists)
                {
                    using (GUILayout.HorizontalScope hs_0 = new GUILayout.HorizontalScope())
                    {
                        GUILayout.Label("【Lua脚本路径】", GUILayout.Width(100));
                        if (GUILayout.Button(Path.GetFileName(luaFileInfo.FullName), "LargeTextField"))
                        {
                            EditorGUIUtility.PingObject(AssetDatabase.LoadAssetAtPath<TextAsset>(viewLuaFileFullPath));
                        }
                    }
                }
                else
                {
                    using (GUILayout.HorizontalScope hs_0 = new GUILayout.HorizontalScope("HelpBox"))
                    {
                        GUILayout.Label("", "CN EntryErrorIcon");
                        using (GUILayout.VerticalScope vs_1 = new GUILayout.VerticalScope("HelpBox"))
                        {
                            GUILayout.Label(string.Format("【{0}目录】", title) + baseLuaFilePath);
                            GUILayout.Label(string.Format("【{0}名字】", title) + Path.GetFileName(viewLuaFileName));
                            GUILayout.Label(string.Format("【{0}路径】", title) + viewLuaFileName);

                            if (GUILayout.Button("创建Lua脚本"))
                            {
                                if (!luaFileInfo.Directory.Exists)
                                    luaFileInfo.Directory.Create();

                                File.WriteAllText(viewLuaFileFullPath, viewLuaFileFullPath);
                                AssetDatabase.Refresh();
                            }
                        }
                    }
                }
            }

            private static void DrawLuaFileInfo(Object target, Object prefabAsset)
            {
                if (prefabAsset == null)
                {
                    return;
                }

                string viewLuaName = prefabAsset.name;
                string viewLuaFileName = string.Format(PathConst.FORMAT_VIEW_NAME, viewLuaName, viewLuaName).Replace('.', '/') + ".txt";
                string pLuaFileName = string.Format(PathConst.FORMAT_PRESENDER_NAME, viewLuaName, viewLuaName).Replace('.', '/') + ".txt";

                DrawLuaFileInfoBase(viewLuaFileName, "View ");
                DrawLuaFileInfoBase(pLuaFileName, "Presender ");
            }




            public static void EditorRefreshWidgets(MonoView target, bool rootMonoView = true)
            {
                if (rootMonoView)
                {
                    target.ParentView = null;
                    target.refName = string.Empty;
                    EditorSetChildsInHierarchy(target.transform, true);
                }

                if (target._widgets == null)
                    target._widgets = new List<UIBehaviour>();
                else
                    target._widgets.Clear();

                List<IWidget> widgets = EditorGetChildWidget(target.transform);
                for (int i = 0; i < widgets.Count; i++)
                {
                    widgets[i].ParentView = target;
                    if (widgets[i] is MonoView)
                    {
                        EditorRefreshWidgets((widgets[i] as MonoView),false);
                    }
                    if (!string.IsNullOrEmpty(widgets[i].RefName))
                        target._widgets.Add(widgets[i] as UIBehaviour);
                }
            }

            private static void EditorSetChildsInHierarchy(Transform dst, bool show)
            {
                Transform[] childGos = dst.GetComponentsInChildren<Transform>();
                for (int i = 0; i < childGos.Length; i++)
                {
                    if (childGos[i] != dst)
                    {
                        childGos[i].hideFlags = show ? HideFlags.None : HideFlags.HideInHierarchy;
                    }
                }
            }

            private static List<IWidget> EditorGetChildWidget(Transform monoView)
            {
                List<IWidget> widgetList = new List<IWidget>();
                int cnt = monoView.childCount;
                for (int i = 0; i < cnt; i++)
                {
                    Transform child = monoView.GetChild(i);
                    UIBehaviour subMonoView = child.GetComponent<UIBehaviour>();
                    if (subMonoView is MonoView)
                    {
                        EditorSetChildsInHierarchy(subMonoView.transform, false);
                        widgetList.Add(subMonoView as IWidget);
                    }
                    else
                    {
                        if (subMonoView is IWidget)
                        {
                            widgetList.Add(subMonoView.GetComponent<IWidget>());
                        }
                        widgetList.AddRange(EditorGetChildWidget(child));
                    }
                }
                return widgetList;
            }

            public static bool EditorIsChangeInHierarchy(MonoView monoView)
            {
                string curSiblingPath = EditorGetSiblingPathInHierarchy(monoView.transform);
                int curChildCnt = EditorGetAllChildCount(monoView.transform);
                if (monoView.EditorSiblingPath != curSiblingPath || monoView.EditorChildCnt != curChildCnt)
                    return true;

                monoView.EditorSiblingPath = curSiblingPath;
                monoView.EditorChildCnt = curChildCnt;
                return false;
            }

            private static int EditorGetAllChildCount(Transform transform)
            {
                return transform.GetComponentsInChildren<Transform>().Length - 1;
            }
            private static string EditorGetSiblingPathInHierarchy(Transform transform)
            {
                string path = string.Empty;
                Transform parent = transform;
                while (parent != null)
                {
                    path += parent.GetSiblingIndex();
                    parent = parent.parent;
                }
                return path;
            }
        }
	}
}