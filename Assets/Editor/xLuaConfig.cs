using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using XLua;

namespace Framework.Editor
{
    namespace Common
    {
        public static class xLuaConfig
        {
            [LuaCallCSharp]
            public static List<Type> lua_call_cs_list
            {
                get
                {
                    Type[] hotfixArray = new Type[by_property.Count];
                    by_property.CopyTo(hotfixArray);
                    List<Type> lua_call_cs_list_tmp = hotfixArray.ToList();
                    return lua_call_cs_list_tmp;
                }
            }

            [CSharpCallLua]
            public static List<Type> cs_call_lua_list
            {
                get
                {
                    Type[] hotfixArray = new Type[by_property.Count];
                    by_property.CopyTo(hotfixArray);
                    List<Type> cs_call_lua_list_tmp = hotfixArray.ToList();
					cs_call_lua_list_tmp.Add(typeof(UnityAction));
					cs_call_lua_list_tmp.Add(typeof(UnityAction<UnityEngine.Object>));
					cs_call_lua_list_tmp.Add(typeof(UnityAction<UnityEngine.GameObject>));
					cs_call_lua_list_tmp.Add(typeof(UnityAction<Framework.Code.Widget.WidgetNode>));
                    return cs_call_lua_list_tmp;
                }
            }

            [Hotfix]
            public static List<Type> by_property
            {
                get
                {
                    return (from type in Assembly.Load("Assembly-CSharp").GetTypes()
                            where type.Namespace != null && (
                            type.Namespace.StartsWith("Framework.Code") ||
                            type.Namespace.StartsWith("Framework.Game") ||
                            type.Namespace.StartsWith("Framework.Util") 
                            )
                            select type).ToList();
                }
            }
        }
    }
}