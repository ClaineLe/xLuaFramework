using System.Collections;
using System.Collections.Generic;
using System.IO;
using Framework.Game;
using Framework.Util;
using Newtonsoft.Json.Linq;
using UnityEngine;


namespace Framework.Editor
{
    namespace AssetBundle
    {
        public class AssetBundleUtility
        {
            public static void CopyBundlesToStreamAsset(string dstPath, string version)
            {
                if (Directory.Exists(dstPath))
                    Directory.Delete(dstPath, true);
                Directory.CreateDirectory(dstPath);

                string bundleBasePath = PathConst.BuildBundleRootPath + PathConst.CurChangePlatformRelativePath;

                string[] filterExtensions = new[] { "*.manifest" };
                string[] versions = version.Trim().Split('.');
                FileUtility.DirCopy(bundleBasePath + "res/" + versions[0], dstPath, filterExtensions);
                FileUtility.DirCopy(bundleBasePath + "lua/" + versions[1], dstPath, filterExtensions);
                FileUtility.DirCopy(bundleBasePath + "xls/" + versions[2], dstPath, filterExtensions);

                File.WriteAllText(Path.Combine( dstPath , "AssetVersion.txt"), version);
                
            }
        }
    }
}