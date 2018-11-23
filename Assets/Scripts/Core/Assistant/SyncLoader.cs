using Framework.Core.Manager;
using Framework.Game;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace Framework.Core
{
    namespace Assistant
    {
        public class SyncLoader
        {
            private Dictionary<string, AssetBundle> assetBundleDic;
          
            private SyncLoader() { }
            public static SyncLoader Create()
            {
                SyncLoader loader = new SyncLoader();
                loader.assetBundleDic = new Dictionary<string, AssetBundle>();
                return loader;
            }

            public T LoadAsset<T>(string assetPath) where T:Object {
                string assetBundleName = AssetPathController.GetAssetBundleName(assetPath);
                string assetName = Path.GetFileNameWithoutExtension(assetPath);

                if (AppConst.AssetBundleModel){
                    AssetBundle bundle = _GetBundle(assetBundleName);
                    return bundle.LoadAsset<T>(assetName);
                }
#if UNITY_EDITOR
                else
                {
                    string[] assetPaths = UnityEditor.AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName(assetBundleName, assetName);
                    if (assetPaths.Length == 0)
                    {
                        Debug.LogError("There is no asset with name \"" + assetName + "\" in " + assetBundleName);
                        return null;
                    }
                    return UnityEditor.AssetDatabase.LoadMainAssetAtPath(assetPaths[0]) as T;
                }
#endif
                return default(T);
            }

            private AssetBundle _GetBundle(string bundleName) {
                if (!this.assetBundleDic.ContainsKey(bundleName))
                {
                    string bundleFullPath = PathRoute.GetAssetBundleFullPath(bundleName);
                    this.assetBundleDic[bundleName] = AssetBundle.LoadFromFile(bundleFullPath);
                }
                return this.assetBundleDic[bundleName];
            }

			public void Dispose(){
				
			}
        }
    }
}