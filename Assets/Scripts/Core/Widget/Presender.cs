using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.Core.Manager;
using Framework.Game;
using XLua;

namespace Framework.Core
{
    namespace Assistant
    {
		public class Presender : LuaCompatible<Presender>
        {
			public View m_View{ get; private set;}
            protected override string _luaPath
            {
                get
                {
                    return string.Format(ResPathConst.FORMAT_PRESENDER_NAME, Name, Name);
                }
            }
            public void SetupView(View view) {
                this.m_View = view;
                base.InitLuaTable(this.m_View.m_LuaTable);

            }
            protected override void onCreate()
            {
            }
        }
	}
}