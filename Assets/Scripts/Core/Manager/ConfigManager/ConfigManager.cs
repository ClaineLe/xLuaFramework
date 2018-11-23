using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.Core.Assistant;
using Framework.Game;


namespace Framework
{
	namespace Core.Manager
	{
		public partial class ManagerName
		{
			public const string Config = "ConfigManager";
		}
		public partial class ConfigManager : BaseManager<ConfigManager>, IManager
		{
			
			private const string LUA_CONFIG_TABLE_NAME = "C";
			private const string CONFIG_XLS_LIST_PATH = "#Xls/xls_list.txt";
			private const string CONFIG_PATH_FORMAT = "#Xls/{0}.csv";
			private SyncLoader m_Loader;
			public void Init(){
				m_Loader = SyncLoader.Create();
			}

			private XLua.LuaTable LoadConfig() {
				string csvListStr = this.m_Loader.LoadAsset<TextAsset>(CONFIG_XLS_LIST_PATH).text;
				string[] xlsList = csvListStr.Trim().Split('\n');
				XLua.LuaTable globalTable = Game.Manager.LuaMgr.CreatLuaTable ();
				for (int i = 0; i < xlsList.Length; i++)
				{
					string csvStr = this.m_Loader.LoadAsset<TextAsset>(string.Format(CONFIG_PATH_FORMAT,xlsList[i])).text;

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

			public void Tick ()
			{
			}

			public void Release ()
			{
				this.m_Loader.Dispose ();
			}

		}
	}
}