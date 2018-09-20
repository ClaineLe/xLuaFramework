using System.Collections.Generic;
using Framework.Editor.Utility;
using Framework.Game;
using UnityEditor;
using UnityEngine;

namespace Framework.Editor
{
    namespace AssetBundle
    {
        public class AssetBundleWnd : EditorWindow
        {
            protected class FuncItem
            {
                public string title;
                public float titleWidth = 120.0f;
                public System.Action onDraw;
                public static FuncItem Create(string title, System.Action onDraw, float titleWidth = 120.0f)
                {
                    FuncItem item = new FuncItem();
                    item.title = title;
                    item.titleWidth = titleWidth;
                    item.onDraw = onDraw;
                    return item;
                }
            }

            private static FuncItem[] _funcItemitems;
            private FuncItem[] m_FuncItemitems
            {
                get
                {
                    if (_funcItemitems == null)
                    {
                        _funcItemitems = new FuncItem[]{
                    FuncItem.Create("系统信息",     OnDrawSystemInfo),
                    FuncItem.Create("ab包名管理",   OnDrawAssetBundleName),
                    FuncItem.Create("构建配置",     OnDrawOptions),
                    FuncItem.Create("Lua包管理",    OnDrawLuaPackage),
                    FuncItem.Create("构建资源包",    OnDraResourcePackage),
                    FuncItem.Create("构建安装包",    OnDrawSetupPackage),
                };
                    }
                    return _funcItemitems;
                }
            }

            List<eChannel> Channel = new List<eChannel>() { eChannel.Official, eChannel.AppleAppStore, eChannel.GooglePlayStore, eChannel.Local };
            string[] targetsName = new[] { "官网渠道", "苹果商店", "谷歌商店", "本地测试" };
            private int channelIndex = 0;
            private bool overrideBundle = false;
            private bool buildAndCopy = true;
            private bool releaseModel = false;
            private int svnVersion = 0;

            private void OnEnable()
            {
                
                base.titleContent = new GUIContent("构建资源包工具");

                RefreshSvnVersion();

                //channelIndex = Channel.FindIndex(a => a == (eChannel)ClientConfig.Channel);

#if RELEASE
		releaseModel = true;
#else
                releaseModel = false;
#endif

            }

            void OnGUI()
            {
                if (EditorApplication.isCompiling)
                {
                    GUILayout.Label("代码编译中...");
                }
                else
                {
                    DrawWindow();
                }
            }

            void DrawWindow()
            {
                for (int i = 0; i < m_FuncItemitems.Length; i++)
                {
                    using (GUILayout.VerticalScope vs0 = new GUILayout.VerticalScope("HelpBox"))
                    {
                        using (GUILayout.HorizontalScope hs0 = new GUILayout.HorizontalScope())
                        {
                            GUILayout.Label("【" + m_FuncItemitems[i].title + "】 ", GUILayout.Width(m_FuncItemitems[i].titleWidth));
                            m_FuncItemitems[i].onDraw();
                        }
                        GUILayout.Space(10);
                    }
                }
            }
            #region 系统信息
            void OnDrawSystemInfo()
            {
                using (GUILayout.VerticalScope vs0 = new GUILayout.VerticalScope())
                {

#if UNITY_ANDROID
			GUILayout.Label ("当前系统:Android");
#elif UNITY_IOS
			GUILayout.Label ("当前系统:iOS");
#else
                    GUILayout.Label("当前系统:Win");
#endif
                    GUILayout.Label("服务器地址:" + AppConfig.FileServerIP);
                    GUILayout.Space(10);
                    GUILayout.Label("AppVersion:" + AppConfig.AppVersion);
                    GUILayout.Label("ResVersion:" + AppConfig.ResVersion);
                    GUILayout.Label("LuaVersion:" + AppConfig.LuaVersion);
                    using (GUILayout.HorizontalScope hs = new GUILayout.HorizontalScope())
                    {
                        GUILayout.Label("SvnVersion:" + svnVersion);
                        if (GUILayout.Button("更新SVN版本号"))
                        {
                            RefreshSvnVersion();
                        }
                    }
                }
            }
            #endregion

            #region 构建配置
            void OnDrawOptions()
            {
                /*
                if (GUILayout.Button("刷新构建配置", GUILayout.Width(100), GUILayout.Height(30)))
                {
                    PlatformDefinesMacros.RemoveAllDefineMacros();

                    PlatformDefinesMacros.EditorDefine(true, (Channel[channelIndex]).ToString());
                    if (releaseModel)
                    {
                        PlatformDefinesMacros.EditorDefine(true, "RELEASE");
                    }
                }*/

                using (GUILayout.VerticalScope vs0 = new GUILayout.VerticalScope())
                {
                    channelIndex = GUILayout.Toolbar(channelIndex, targetsName, GUILayout.Width(250));
                    releaseModel = GUILayout.Toggle(releaseModel, "发布模式");
                }
            }
            #endregion

            #region ab名字管理
            void OnDrawAssetBundleName()
            {
                if (GUILayout.Button("清理", GUILayout.Width(100), GUILayout.Height(30)))
                {
                    AssetBundleMark.Clean();
                    EditorUtility.ClearProgressBar();
                }
                GUILayout.Space(10);
                if (GUILayout.Button("完整构建", GUILayout.Width(100), GUILayout.Height(30)))
                {
                    AssetBundleMark.MarkAssets();
                    AssetBundleMark.MarkLua();
                    EditorUtility.ClearProgressBar();
                }
                GUILayout.Space(10);
                if (GUILayout.Button("构建Lua", GUILayout.Width(100), GUILayout.Height(30)))
                {
                    AssetBundleMark.MarkLua();
                    EditorUtility.ClearProgressBar();
                }
                GUILayout.Space(10);
                if (GUILayout.Button("构建资源", GUILayout.Width(100), GUILayout.Height(30)))
                {
                    AssetBundleMark.MarkAssets();
                    EditorUtility.ClearProgressBar();
                }
            }
            #endregion

            #region 导出lua包
            void OnDrawLuaPackage()
            {
                if (GUILayout.Button("导出lua包", GUILayout.Width(100), GUILayout.Height(30)))
                {
                    AssetBundleMark.MarkLua();
                    AssetBundleBuild.BuildLua();
                    EditorUtility.ClearProgressBar();
                }
            }
            #endregion

            #region 构建资源包
            void OnDraResourcePackage()
            {
                if (GUILayout.Button("构建资源", GUILayout.Width(100), GUILayout.Height(30)))
                {
                    AssetBundleBuild.BuildAssetBundle();
                    EditorUtility.ClearProgressBar();
                }
                overrideBundle = GUILayout.Toggle(overrideBundle, "OverRide Bundle");
            }
            #endregion

            #region 构建安装包
            void OnDrawSetupPackage()
            {
                if (GUILayout.Button("构建应用", GUILayout.Width(100), GUILayout.Height(30)))
                {
                    AssetBundleBuild.BuildPlayer();
                    EditorUtility.ClearProgressBar();
                }

                using (GUILayout.HorizontalScope hs = new GUILayout.HorizontalScope())
                {
                    using (GUILayout.VerticalScope vs0 = new GUILayout.VerticalScope())
                    {
                        buildAndCopy = GUILayout.Toggle(buildAndCopy, "Build && Copy To StreamingAssets");
                    }
                }
            }
            #endregion
            public void RefreshSvnVersion()
            {
                svnVersion = SvnUtility.GetSvnVersion(Application.dataPath);
            }
        }
    }
}