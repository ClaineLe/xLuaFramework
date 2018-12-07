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
        public class WidgetInspector : UnityEditor.Editor
        {
            public const string basePrefabPath = "Assets/" + PathConst.ExportResDirPath + PathConst.ViewRoot_BasePath;
            public const string baseLuaFilePath = "Assets/" + PathConst.ExportResDirPath + PathConst.FORMAT_LUAROOT;

            public static void DrawHeaderGUI(Object target)
            {
                IWidget widget = target as IWidget;
                using (GUILayout.VerticalScope vs = new GUILayout.VerticalScope("IN GameObjectHeader"))
                {
                    if (widget is MonoView)
                    {
                        Object prefabAsset = PrefabUtility.GetPrefabParent(target);
                        DrawPrefabInfo(target, prefabAsset);
                        DrawLuaFileInfo(target, prefabAsset);
                    }

                    if (widget.ParentView != null || !(widget is MonoView))
                    {
                        GUILayout.Space(5);
                        using (GUILayout.VerticalScope v1s = new GUILayout.VerticalScope("HelpBox"))
                        {
                            using (EditorGUI.ChangeCheckScope ccs = new EditorGUI.ChangeCheckScope())
                            {
                                using (GUILayout.HorizontalScope hs_0 = new GUILayout.HorizontalScope())
                                {
                                    GUILayout.Label("【引用标签】", GUILayout.Width(100));
                                    widget.RefName = EditorGUILayout.TextField(widget.RefName);
                                }

                                if (ccs.changed)
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
                            }
                            using (GUILayout.HorizontalScope hs_0 = new GUILayout.HorizontalScope())
                            {
                                GUILayout.Label("【父面板】", GUILayout.Width(100));

                                if (widget.ParentView)
                                {
                                    if (GUILayout.Button(widget.ParentView.name + " (MonoView)", "LargeTextField", GUILayout.MinWidth(140)))
                                    {
                                        EditorGUIUtility.PingObject(widget.ParentView);
                                    }
                                }
                                else
                                {
                                    GUILayout.Label("无父物体", "LargeTextField", GUILayout.MinWidth(140));

                                }
                            }
                        }
                    }
                }
                EditorApplication.RepaintHierarchyWindow();
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
        }
    }
}