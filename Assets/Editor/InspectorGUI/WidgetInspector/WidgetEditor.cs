using Framework.Core.Assistant;
using Framework.Core.Widget;
using Framework.Game;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Framework.Editor
{
    namespace Widget
    {
        public class WidgetEditor:UnityEditor.Editor
        {
            public const string basePrefabPath = "Assets/" + PathConst.ExportResDirPath + PathConst.ViewRoot_BasePath;
            public const string baseLuaFilePath = "Assets/" + PathConst.ExportResDirPath + PathConst.FORMAT_LUAROOT;

            public static void WidgetCommondInspector(Object target)
            {
                IWidget widget = target as IWidget;
                EditorGUILayout.BeginVertical(GUI.skin.FindStyle("IN GameObjectHeader"));
                EditorGUI.BeginChangeCheck();
                using (GUILayout.HorizontalScope hs_0 = new GUILayout.HorizontalScope())
                {
                    GUILayout.Label("【引用标签】", GUILayout.Width(100));
                    widget.RefName = EditorGUILayout.TextField(widget.RefName);
                }

                if (EditorGUI.EndChangeCheck())
                {
                    EditorUtility.SetDirty(target);

                    Transform parent = (target as UIBehaviour).transform;
                    MonoView monoView = null;
                    while (parent != null)
                    {
                        MonoView tmpMonoView = parent.GetComponent<MonoView>();
                        if (tmpMonoView != null)
                            monoView = tmpMonoView;
                        parent = parent.parent;
                    }
                    monoView.Refresh();
                }

                if (widget.ParentView != null)
                {
                    using (GUILayout.HorizontalScope hs_0 = new GUILayout.HorizontalScope())
                    {
                        GUILayout.Label("【父面板】", GUILayout.Width(100));
                        if (GUILayout.Button(Path.GetFileName(widget.ParentView.name + " (MonoView)"), GUI.skin.FindStyle("LargeTextField"),GUILayout.MinWidth(140)))
                        {
                            EditorGUIUtility.PingObject(widget.ParentView);
                        }
                    }
                }

                if (widget is MonoView)
                {
                    Object prefabAsset = PrefabUtility.GetPrefabParent(target);
                    DrawPrefabInfo(target, prefabAsset);
                    DrawLuaFileInfo(target, prefabAsset);
                }

                EditorGUILayout.EndVertical();

                EditorApplication.RepaintHierarchyWindow();
            }

            public static void DrawPrefabInfo(Object target, Object prefabAsset)
            {
                if (prefabAsset == null)
                {
                    using (GUILayout.HorizontalScope hs_0 = new GUILayout.HorizontalScope(GUI.skin.FindStyle("HelpBox")))
                    {
                        GUILayout.Label(string.Empty, GUI.skin.FindStyle("CN EntryErrorIcon"));
                        using (GUILayout.VerticalScope vs_1 = new GUILayout.VerticalScope(GUI.skin.FindStyle("HelpBox")))
                        {
                            string prefabName = target.name + ".prefab";
                            string fullPath = basePrefabPath + prefabName;
                            GUILayout.Label("【面板预制名字】" + prefabName, GUI.skin.FindStyle("HeaderLabel"));
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
                        GUILayout.Label("【面板预置物路径】", GUILayout.Width(100));
                        if (GUILayout.Button(prefabAsset.name + ".prefab", GUI.skin.FindStyle("LargeTextField")))
                        {
                            EditorGUIUtility.PingObject((prefabAsset as MonoView).gameObject.GetInstanceID());
                        }
                    }
                }
            }

            public static void DrawLuaFileInfoBase(string viewLuaFileName,string title)
            {
                string viewLuaFileFullPath = baseLuaFilePath + viewLuaFileName;

                FileInfo luaFileInfo = new FileInfo(viewLuaFileFullPath);
                if (luaFileInfo.Exists)
                {
                    using (GUILayout.HorizontalScope hs_0 = new GUILayout.HorizontalScope())
                    {
                        GUILayout.Label("【Lua脚本路径】", GUILayout.Width(100));
                        if (GUILayout.Button(Path.GetFileName(luaFileInfo.FullName), GUI.skin.FindStyle("LargeTextField")))
                        {
                            EditorGUIUtility.PingObject(AssetDatabase.LoadAssetAtPath<TextAsset>(viewLuaFileFullPath));
                        }
                    }
                }
                else
                {
                    using (GUILayout.HorizontalScope hs_0 = new GUILayout.HorizontalScope(GUI.skin.FindStyle("HelpBox")))
                    {
                        GUILayout.Label("", GUI.skin.FindStyle("CN EntryErrorIcon"));
                        using (GUILayout.VerticalScope vs_1 = new GUILayout.VerticalScope(GUI.skin.FindStyle("HelpBox")))
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

            public static void DrawLuaFileInfo(Object target, Object prefabAsset)
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
        }
    }
}