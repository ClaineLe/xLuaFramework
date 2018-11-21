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
				m_LuaLoader = SyncLoader.Create("lua/" + AppConst.LuaVersion.ToString() + "/");
				m_XlsLoader = SyncLoader.Create("xls/" + AppConst.XlsVersion.ToString() + "/");
                this.m_LuaEnv = new XLua.LuaEnv();
                this.m_LuaEnv.AddBuildin("rapidjson", XLua.LuaDLL.Lua.LoadRapidJson);
                this.m_LuaEnv.AddBuildin("lpeg", XLua.LuaDLL.Lua.LoadLpeg);
                this.m_LuaEnv.AddBuildin("pb", XLua.LuaDLL.Lua.LoadLuaProfobuf);
                this.m_LuaEnv.AddLoader(CustomLoader);
				this.m_Require = m_LuaEnv.Global.Get<XLua.LuaFunction>("require");
                this.InitConfig();
            }

            private void InitConfig() {
				DirectoryInfo cfgDirInfo = new DirectoryInfo ("Assets/AppAssets/#Xls");
                ConfigData configData = new ConfigData();
                FileInfo[] csvFileInfos = cfgDirInfo.GetFiles ();
				for (int i = 0; i < csvFileInfos.Length; i++) {
					if (csvFileInfos [i].Extension == ".manifest" || csvFileInfos [i].Extension == ".meta")
						continue;
					Debug.Log ("==================" + csvFileInfos[i].Name);

                    load (configData, (UnityEditor.AssetDatabase.LoadMainAssetAtPath("Assets/AppAssets/#Xls/" + csvFileInfos[i].Name) as TextAsset).text);
                    //load (configData, this.m_XlsLoader.LoadAsset<TextAsset> ("#Xls/" + Path.GetFileNameWithoutExtension (csvFileInfos [i].Name)).text);
                    /*string[][] Array = load(configData, this.m_XlsLoader.LoadAsset<TextAsset> ("#Xls/" + Path.GetFileNameWithoutExtension (csvFileInfos [i].Name)).text);
                    for (int row = 0; row < Array.Length; row++)
                    {
                        for (int col = 0; col < Array[row].Length; col++)
                        {
                            Debug.Log(Array[row][col].Replace("</br>", "\n"));
                        }
                    }
                    XLua.LuaTable cfgTable = this.m_LuaEnv.NewTable();
					cfgTable.Set<string, string>("Name", "Cddddlaine");
					cfgTable.Set<string, int>("Age", 30);
					m_LuaEnv.Global.Set<string, XLua.LuaTable>(Array[1][1], cfgTable);
                    */
                }
            }

            public class ConfigData
            {
                public string[] Names;
                public string[] Types;
                public string[][] Datas;
            }

			void load (ConfigData configData, string csv)  
			{
                Debug.LogError(csv);
				//读取每一行的内容  
				string [] lineArray = csv.Split ("\n"[0]);  

                //创建二维数组  
                configData.Datas = new string [lineArray.Length-2][];

                configData.Names = lineArray[0].Split(',');
                configData.Types = lineArray[1].Split(',');
                configData.Datas = CsvParser.Parse(csv);
                Debug.Log(configData.Datas.Length);
                for (int i = 0; i < configData.Datas.Length; i++) {
                    for (int j = 0; j < configData.Datas[i].Length; j++)
                        Debug.Log(configData.Datas[i][j]);
                }
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
