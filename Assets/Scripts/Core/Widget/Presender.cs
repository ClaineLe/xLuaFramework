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
                    return string.Format(ResPathConst.FORMAT_PRESENDER_NAME, m_Name, m_Name);
                }
            }
            public Presender SetupView(View view) {
                this.m_View = view;
                base.InitLuaTable(this.m_View.m_LuaTable);
                return this;

            }
            protected override void onCreate()
            {
            }

            protected override void onRelease()
            {
            }
        }
	}
}