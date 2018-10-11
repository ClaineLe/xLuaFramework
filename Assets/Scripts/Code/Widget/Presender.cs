using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.Code.Manager;

namespace Framework
{
	namespace Code.Widget
	{
		public class Presender
		{
			private const string FORMAT_PRESENDER_NAME = "view.{0}Presender";

			public View m_View{ get; private set;}

			protected virtual bool m_IsLuaPresender{
				get{
					return true;	
				}
			}

			private XLua.LuaTable m_LuaPresender;
			public XLua.LuaTable LuaPresender{
				get{ 
					return m_LuaPresender;
				}
			}

			private Presender(){}
			public static Presender Create(GameObject viewGo){
				Presender presender = new Presender ();
				presender.m_View = View.Create (viewGo);;
				presender.OnCreate ();
				return presender;
			}

			private void OnCreate(){
				if (this.m_IsLuaPresender) {
					this.InitLuaPresender ();
				}
			}

			public void InitLuaPresender(){
				XLua.LuaTable luaTmp = Framework.Game.Manager.LuaMgr.TblRequire (string.Format (FORMAT_PRESENDER_NAME, this.m_View.Name));
				this.m_LuaPresender = luaTmp.Get<XLua.LuaFunction> ("Create").Call (luaTmp, this.m_View.LuaView)[0] as XLua.LuaTable;
			}
		}
	}
}