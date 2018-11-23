using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Framework
{
    namespace Game
    {
#if UNITY_EDITOR
    #if BUNDLE_MODEL
            public class PathRoute
            {
                public static string GetAssetBundleFullPath(string bundleName)
                {
                    if (bundleName.StartsWith("#xls"))
                        return PathConst.StreamAssetPath + AppConst.GetPlatformName() + "/" + PathConst.XlsRelativePath + bundleName;
                    else if (bundleName.StartsWith("lua"))
                        return PathConst.StreamAssetPath + AppConst.GetPlatformName() + "/" + PathConst.LuaRelativePath + bundleName;
                    else
                        return PathConst.StreamAssetPath + AppConst.GetPlatformName() + "/" + PathConst.ResRelativePath + bundleName;
                }
            }
    #endif
#else
        public class PathRoute
        {
            public static string GetAssetBundleFullPath(string bundleName)
            {            
                return PathConst.StreamAssetPathInAsset + "/" + bundleName;
            }
        }
#endif
    }
}