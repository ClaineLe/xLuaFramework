﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Framework
{
	namespace Game
	{
		public class ResPathConst 
		{
			public const string ViewRoot_Path = "Prefabs/#View/ViewRoot.prefab";
			public const string ViewLayer_Path = "Prefabs/#View/Layer.prefab";
			public const string ViewAsset_Path = "Prefabs/#View/{0}.prefab";
			public const string FORMAT_MODEL_NAME = "model.{0}.{1}";
			public const string FORMAT_VIEW_NAME = "view.{0}.{1}";
			public const string FORMAT_LUAROOT = "Lua/";
			public const string LUA_FRAMEWORK = "#core.Framework";
			public const string FORMAT_PRESENDER_NAME = "view.{0}.{1}Presender";
		}
	}
}