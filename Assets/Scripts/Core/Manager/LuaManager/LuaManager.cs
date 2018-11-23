using System.IO;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
using Framework.Game;
using Framework.Core.Assistant;

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

			private List<string> m_cSharpAddTable;
            private XLua.LuaFunction m_Require;
			private SyncLoader m_Loader;
            public void Init()
            {
				m_Loader = SyncLoader.Create();
				m_cSharpAddTable = new List<string> ();
                this.m_LuaEnv = new XLua.LuaEnv();
                this.m_LuaEnv.AddBuildin("rapidjson", XLua.LuaDLL.Lua.LoadRapidJson);
                this.m_LuaEnv.AddBuildin("lpeg", XLua.LuaDLL.Lua.LoadLpeg);
                this.m_LuaEnv.AddBuildin("pb", XLua.LuaDLL.Lua.LoadLuaProfobuf);
                this.m_LuaEnv.AddLoader(CustomLoader);
				this.m_Require = m_LuaEnv.Global.Get<XLua.LuaFunction>("require");
            }


			public void DisposeGlobalLuaTable(string cfgName){
				XLua.LuaTable CfgTable;
				m_LuaEnv.Global.Get<string, XLua.LuaTable>(cfgName, out CfgTable);
				if (CfgTable != null)
				{
					CfgTable.Dispose();
				}
			} 

			public void SetGlobalLuaTable(string cfgName, XLua.LuaTable cfgTable){
				if (!m_cSharpAddTable.Exists (a => a.Equals (cfgName)))
					m_cSharpAddTable.Add (cfgName);
				m_LuaEnv.Global.Set<string, XLua.LuaTable>(cfgName, cfgTable);
			}

			private void ClearGlobalLuaTable(){
				for (int i = 0; i < m_cSharpAddTable.Count; i++)
					DisposeGlobalLuaTable (m_cSharpAddTable[i]);
				m_cSharpAddTable = new List<string> ();
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
				this.ClearGlobalLuaTable ();
				this.m_Loader.Dispose ();

                if (m_FrameworkRelease != null)
                    m_FrameworkRelease.Call();

                this.m_FrameworkStart = null;
                this.m_FrameworkTick = null;
                this.m_FrameworkRelease = null;

                this.m_LuaEnv = null;
            }

			public XLua.LuaTable CreatLuaTable(){
				return this.m_LuaEnv.NewTable ();
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
				string luaPath = PathConst.FORMAT_LUAROOT + filepath.Replace('.',Path.DirectorySeparatorChar);
				TextAsset txtAsset = this.m_Loader.LoadAsset<TextAsset>(luaPath);
				return txtAsset.bytes;
            }

            public bool StartUpLuaFramework()
            {
				XLua.LuaTable frameworkTable = this.TblRequire (PathConst.LUA_FRAMEWORK);
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
