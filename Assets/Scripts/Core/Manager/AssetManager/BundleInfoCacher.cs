using System;
using System.Collections.Generic;
using Framework.Game;
using UnityEngine;

namespace Framework
{
    namespace Core.Manager
    {
#if !UNITY_EDITOR || BUNDLE_MODEL
        public class BundleInfoCacher
        {
            public static List<BundleInfo> m_BundleInfoList;
            public static void Init(){
                m_BundleInfoList = new List<BundleInfo>();
                m_BundleInfoList.Add(new BundleInfo() { Name = "prefabs_#view_viewroot" });
            }

            public static bool InCahce(string bundleName){
                return m_BundleInfoList.Exists(a => a.Name.Equals(bundleName));
            }
        }
#endif
    }
}