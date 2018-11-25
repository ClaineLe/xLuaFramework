using System.Collections;
using System.Collections.Generic;
using Framework.Core.Manager;
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
                string bundleBasePath = string.Empty;
                if (BundleInfoCacher.InCahce(bundleName))
                    bundleBasePath = PathConst.PersistentDataPath;
                else
                    bundleBasePath = PathConst.StreamAssetPath;
                return bundleBasePath + PathConst.BundleDirName + "/" + bundleName;
            }
        }
#endif
    }
}