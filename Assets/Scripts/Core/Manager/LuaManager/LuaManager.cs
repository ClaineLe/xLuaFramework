using System.IO;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
using Framework.Game;

namespace Framework
{
    namespace Core.Manager
    {
        public partial class ManagerName
        {
            public const string Lua = "LuaManager";
        }
        public partial class LuaManager : BaseManager<LuaManager>, IManager
        {
            private XLua.LuaEnv m_LuaEnv;

            private XLua.LuaFunction m_FrameworkStart;
            private XLua.LuaFunction m_FrameworkTick;
            private XLua.LuaFunction m_FrameworkRelease;

            private float lastGCTime = 0;
            private const float GCInterval = 1;//1 second 


			private string m_BaseLuaPath;
			private XLua.LuaFunction m_Require;
            public void Init()
            {
				this.m_assetBundleDic = new Dictionary<string, AssetBundle> ();
				m_BaseLuaPath = ResPathConst.BaseResPath;
				#if UNITY_EDITOR
					if (!AppConst.SimulateAssetBundleInEditor)
						m_BaseLuaPath += "lua/" + AppConst.LuaVersion.ToString () + "/";
				#endif
                this.m_LuaEnv = new XLua.LuaEnv();
                this.m_LuaEnv.AddBuildin("rapidjson", XLua.LuaDLL.Lua.LoadRapidJson);
                this.m_LuaEnv.AddBuildin("lpeg", XLua.LuaDLL.Lua.LoadLpeg);
                this.m_LuaEnv.AddBuildin("pb", XLua.LuaDLL.Lua.LoadLuaProfobuf);
                this.m_LuaEnv.AddLoader(CustomLoader);
				this.m_Require = m_LuaEnv.Global.Get<XLua.LuaFunction>("require");
            }

            public void Tick()
            {
                if (m_FrameworkTick != null)
                    m_FrameworkTick.Call();

                if (Time.time - lastGCTime > GCInterval)
                {
                    this.m_LuaEnv.Tick();
                    lastGCTime = Time.time;
                }
            }

            public void Release()
            {
                if (m_FrameworkRelease != null)
                    m_FrameworkRelease.Call();

                this.m_FrameworkStart = null;
                this.m_FrameworkTick = null;
                this.m_FrameworkRelease = null;

                this.m_LuaEnv = null;
            }

			public object[] LuaRequire(string luaPath)
            {
				return this.m_Require.Call (luaPath);
            }

			public XLua.LuaTable TblRequire(string luaPath){
				return this.m_Require.Call (luaPath)[0] as XLua.LuaTable;
			}

            public void HotFix(string hotfixStr)
            {
                this.m_LuaEnv.DoString(hotfixStr);
            }

            public byte[] CustomLoader(ref string filepath)
            {
				string luaPath = ResPathConst.FORMAT_LUAROOT + filepath.Replace('.',Path.DirectorySeparatorChar);

				string assetBundleName = AssetPathController.GetAssetBundleName (luaPath);
				string assetName = Path.GetFileNameWithoutExtension (luaPath);

				TextAsset txtAsset;
				#if UNITY_EDITOR
				if (AppConst.SimulateAssetBundleInEditor) {
					string[] assetPaths = UnityEditor.AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName (assetBundleName, assetName);
					if (assetPaths.Length == 0) {
						Debug.LogError ("There is no asset with name \"" + assetName + "\" in " + assetBundleName);
						return null;
					}
					txtAsset = UnityEditor.AssetDatabase.LoadMainAssetAtPath (assetPaths [0]) as TextAsset;
				} else
				#endif
				{
					if (!this.m_assetBundleDic.ContainsKey(assetBundleName)) {
						this.m_assetBundleDic [assetBundleName] = AssetBundle.LoadFromFile (m_BaseLuaPath + assetBundleName);
					}

					txtAsset = this.m_assetBundleDic [assetBundleName].LoadAsset<TextAsset> (assetName);

				}
				return txtAsset.bytes;
            }

			private Dictionary<string,AssetBundle> m_assetBundleDic;


            public bool StartUpLuaFramework()
            {
				XLua.LuaTable frameworkTable = this.TblRequire (ResPathConst.LUA_FRAMEWORK);
                m_FrameworkStart = frameworkTable.Get<XLua.LuaFunction>("Start");
                m_FrameworkTick = frameworkTable.Get<XLua.LuaFunction>("Tick");
                m_FrameworkRelease = frameworkTable.Get<XLua.LuaFunction>("Release");
				if (m_FrameworkStart != null) {
					m_FrameworkStart.Call(frameworkTable);
				}
                return true;
            }
        }
    }
}
