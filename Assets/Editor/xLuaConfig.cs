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
					cs_call_lua_list_tmp.Add (typeof(UnityAction));
					cs_call_lua_list_tmp.Add (typeof(UnityAction<object>));
					cs_call_lua_list_tmp.Add (typeof(UnityAction<XLua.LuaTable>));
                    cs_call_lua_list_tmp.Add (typeof(UnityAction<UnityEngine.Object>));
					cs_call_lua_list_tmp.Add (typeof(UnityAction<UnityEngine.GameObject>));
					cs_call_lua_list_tmp.Add (typeof(UnityAction<Framework.Core.Assistant.View>));
					cs_call_lua_list_tmp.Add (typeof(UnityAction<Framework.Core.Assistant.Presender>));
                    return cs_call_lua_list_tmp;
                }
            }

            [Hotfix]
            public static List<Type> by_property
            {
                get
                {
					List<Type> by_property_tmp = (from type in Assembly.Load("Assembly-CSharp").GetTypes()
						where type.Namespace != null &&
						!FilterTypeList.Exists (a => a.FullName == type.FullName) &&
						(
                            type.Namespace.StartsWith("Framework.Core") ||
                            type.Namespace.StartsWith("Framework.Game") ||
							type.Namespace.StartsWith("Framework.Util") 
						)
                            select type).ToList();
					return by_property_tmp;
                }
            }

			public static List<Type> FilterTypeList{
				get{
					List<Type> filterTypeList = new List<Type> ();
					filterTypeList.Add (typeof(Framework.Core.Manager.LoadSceneSimulationOperation));
					filterTypeList.Add (typeof(Framework.Core.Manager.LoadAssetOperationSimulation));
					return filterTypeList;
				}
			}

			[BlackList]
			public static List<List<string>> BlackList = new List<List<string>>()  {
				new List<string>(){ "Framework.Game.AppConst", "GetPlatformForAssetBundles", "UnityEditor.BuildTarget"},
                new List<string>(){ "Framework.Game.PathConst", "ProjectPath" },
			};
        }
    }
}