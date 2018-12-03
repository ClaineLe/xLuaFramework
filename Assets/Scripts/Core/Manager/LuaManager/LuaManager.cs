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

			private const string LUA_CONFIG_TABLE_NAME = "C";
			private const string CONFIG_XLS_LIST_PATH = "#Xls/xls_list.txt";
			private const string CONFIG_PATH_FORMAT = "#Xls/{0}.csv";

            private XLua.LuaEnv m_LuaEnv;

            private XLua.LuaFunction m_FrameworkStart;
            private XLua.LuaFunction m_FrameworkTick;
            private XLua.LuaFunction m_FrameworkRelease;

            private float lastGCTime = 0;
            private const float GCInterval = 1;//1 second 

			private List<string> m_cSharpAddTable;
            private XLua.LuaFunction m_Require;
			private SyncLoader m_CfgLoader;
			private SyncLoader m_LuaLoader;
            public void Init()
            {
				m_LuaLoader = SyncLoader.Create();
				m_CfgLoader = SyncLoader.Create();
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
				this.m_LuaLoader.Dispose ();
				this.m_CfgLoader.Dispose ();

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
				TextAsset txtAsset = this.m_LuaLoader.LoadAsset<TextAsset>(luaPath);
				return txtAsset.bytes;
            }

			private XLua.LuaTable LoadConfig() {
				string csvListStr = this.m_CfgLoader.LoadAsset<TextAsset>(CONFIG_XLS_LIST_PATH).text;
				string[] xlsList = csvListStr.Trim().Split('\n');
				XLua.LuaTable globalTable = Game.Manager.LuaMgr.CreatLuaTable ();
				for (int i = 0; i < xlsList.Length; i++)
				{
					string csvStr = this.m_CfgLoader.LoadAsset<TextAsset>(string.Format(CONFIG_PATH_FORMAT,xlsList[i])).text;

					string[][] cellData = CsvParser.Parse(csvStr);
					XLua.LuaTable cfgTable = Game.Manager.LuaMgr.CreatLuaTable ();
					globalTable.Set<string, XLua.LuaTable>(xlsList[i], cfgTable);
					string[] titles = cellData[0];
					string[] types = cellData[1];
					for (int row = 2; row < cellData.Length; row++)
					{
						XLua.LuaTable rowTable = Game.Manager.LuaMgr.CreatLuaTable ();
						for (int col = 1; col < cellData[row].Length; col++)
						{
							rowTable.Set<string, string>(titles[col], cellData[row][col]);
						}
						cfgTable.Set<int, XLua.LuaTable>(int.Parse(cellData[row][0]), rowTable);
					}
				}
				return globalTable;
			}

			public void UnSetupConfig(){
				Game.Manager.LuaMgr.DisposeGlobalLuaTable (LUA_CONFIG_TABLE_NAME);
			}

			public void SetupConfig() {
				XLua.LuaTable globalTable = LoadConfig ();
				Game.Manager.LuaMgr.SetGlobalLuaTable (LUA_CONFIG_TABLE_NAME,globalTable);
			}
            public bool StartUpLuaFramework()
            {
				SetupConfig ();
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
