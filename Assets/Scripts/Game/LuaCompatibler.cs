using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Framework.Core
{
	namespace Assistant
	{
		public abstract class LuaCompatibler
		{
			public XLua.LuaTable m_LuaTable;

			public bool m_IsLua;

			public string m_LuaPathFormat;

			public string m_LuaName;

			public const string m_CreateFunName = "Create";

			protected void Init(){
				
			}

			protected void InitLua(){
				string luaPath = string.Format (m_LuaPathFormat,this.m_LuaName,this.m_LuaName);
				XLua.LuaTable luaTmp = Framework.Game.Manager.LuaMgr.TblRequire (luaPath);
				this.m_LuaTable = luaTmp.Get<XLua.LuaFunction> (m_CreateFunName).Call (luaTmp, this)[0] as XLua.LuaTable;
			}
		}
	}
}