using System;
using System.Collections.Generic;
using System.IO;
using Framework.Game;
using Newtonsoft.Json;
using UnityEngine;

namespace Framework
{
    namespace Core.Manager
    {
#if !UNITY_EDITOR || BUNDLE_MODEL
        public class BundleInfoCacher
        {
			public static List<BundleBaseInfo> m_BundleInfoList;

            public static void Init(){
				string localContent = File.ReadAllText (PathConst.StreamAssetPath + PathConst.BundleDirName + "/" + PathConst.BUNDLE_INFO_LIST_FILE_NAME);
                List<BundleBaseInfo>  m_local = ClientBundleInfo.ValuleOf(localContent).bundleList;

                string cacheContent = File.ReadAllText(PathConst.PersistentDataPath + PathConst.BundleDirName + "/" + PathConst.BUNDLE_INFO_LIST_FILE_NAME);
                m_BundleInfoList = ClientBundleInfo.ValuleOf(cacheContent).bundleList;
                m_BundleInfoList.RemoveAll(a=> m_local.Exists(b=> b.Equals(a)));
            }

            public static bool InCahce(string bundleName){
                if (m_BundleInfoList == null || m_BundleInfoList.Count == 0)
                    return false;
                return m_BundleInfoList.Exists(a => a.name.Equals(bundleName));
            }
        }
#endif
    }
}