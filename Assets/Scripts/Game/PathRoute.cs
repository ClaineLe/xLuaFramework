using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Framework
{
    namespace Game
    {
        public class PathRoute
        {
            public static string GetAssetBundleFullPath(string bundleName)
            {
#if UNITY_EDITOR
                if (bundleName.StartsWith("#xls"))
                    return PathConst.StreamAssetPath + AppConst.GetPlatformName() + "/" + PathConst.XlsRelativePath + bundleName;
                else if (bundleName.StartsWith("lua"))
                    return PathConst.StreamAssetPath + AppConst.GetPlatformName() + "/" + PathConst.LuaRelativePath + bundleName;
                else
                    return PathConst.StreamAssetPath + AppConst.GetPlatformName() + "/" + PathConst.ResRelativePath + bundleName;
#endif
                return PathConst.StreamAssetPathInAsset + "/" + bundleName;
            }
        }
    }
}