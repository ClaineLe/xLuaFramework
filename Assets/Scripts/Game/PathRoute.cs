using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Framework
{
    namespace Game
    {
#if !UNITY_EDITOR || BUNDLE_MODEL
        public class PathRoute
        {
            public static string GetAssetBundleFullPath(string bundleName)
            {
                return PathConst.StreamAssetPath + PathConst.BundleDirName + "/" + bundleName;
            }
        }
#endif
    }
}