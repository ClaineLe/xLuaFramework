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

            private XLua.LuaFunction m_Require;
			private SyncLoader m_LuaLoader;
			private SyncLoader m_XlsLoader;
            public void Init()
            {
                m_LuaLoader = SyncLoader.Create(ResPathConst.LuaRelativePath);
                m_XlsLoader = SyncLoader.Create(ResPathConst.XlsRelativePath);

                this.m_LuaEnv = new XLua.LuaEnv();
                this.m_LuaEnv.AddBuildin("rapidjson", XLua.LuaDLL.Lua.LoadRapidJson);
                this.m_LuaEnv.AddBuildin("lpeg", XLua.LuaDLL.Lua.LoadLpeg);
                this.m_LuaEnv.AddBuildin("pb", XLua.LuaDLL.Lua.LoadLuaProfobuf);
                this.m_LuaEnv.AddLoader(CustomLoader);
				this.m_Require = m_LuaEnv.Global.Get<XLua.LuaFunction>("require");
                this.LoadConfig();
            }

            private void UnLoadConfig() {
                XLua.LuaTable CfgTable;
                m_LuaEnv.Global.Get<string, XLua.LuaTable>("C", out CfgTable);
                if (CfgTable != null)
                {
                    CfgTable.Dispose();
                    CfgTable = null;
                    m_LuaEnv.Global.Set<string, XLua.LuaTable>("C", CfgTable);
                }
            }

            private void LoadConfig() {
                string csvListStr = this.m_XlsLoader.LoadAsset<TextAsset>("#Xls/xls_list.txt").text;
                string[] xlsList = csvListStr.Trim().Split('\n');

                XLua.LuaTable CfgTable = m_LuaEnv.NewTable();
                m_LuaEnv.Global.Set<string, XLua.LuaTable>("C", CfgTable);

                for (int i = 0; i < xlsList.Length; i++)
                {
                    string csvStr = this.m_XlsLoader.LoadAsset<TextAsset>("#Xls/" + xlsList[i] + ".csv").text;

                    string[][] cellData = CsvParser.Parse(csvStr);
                    XLua.LuaTable cfgTable = this.m_LuaEnv.NewTable();
                    m_LuaEnv.Global.Set<string, XLua.LuaTable>("Cfg_" + xlsList[i], cfgTable);
                    string[] titles = cellData[0];
                    string[] types = cellData[1];
                    for (int row = 2; row < cellData.Length; row++)
                    {
                        XLua.LuaTable rowTable = this.m_LuaEnv.NewTable();
                        for (int col = 1; col < cellData[row].Length; col++)
                        {
                            rowTable.Set<string, string>(titles[col], cellData[row][col]);
                        }
                        cfgTable.Set<int, XLua.LuaTable>(int.Parse(cellData[row][0]), rowTable);
                    }
                }
            }

            public void ReLoadConfig() {
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
				TextAsset txtAsset = this.m_LuaLoader.LoadAsset<TextAsset>(luaPath);
				return txtAsset.bytes;
            }

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
