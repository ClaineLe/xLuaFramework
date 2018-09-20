using UnityEngine;

namespace Framework
{
    namespace Game
    { 
        public class PathConst{
			public const string ExportResDirPath = "AppAssets/";
			public static string PlayerOutPutPath {
				get {
					return Application.dataPath + "/../PlayerOutPut/";
				}
			}
        }
    }
}