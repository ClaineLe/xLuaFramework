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
            private string _basePath;
			public string BasePath{
				get{ 
					return _basePath;
				}
			}
            private SyncLoader() { }
            public static SyncLoader Create(string relativePath)
            {
                SyncLoader loader = new SyncLoader();
                loader.assetBundleDic = new Dictionary<string, AssetBundle>();
#if UNITY_EDITOR
                loader._basePath = ResPathConst.BaseResPath + relativePath;
#else
                loader._basePath = PathConst.StreamAssetPathInAsset;
#endif
                return loader;
            }

            public T LoadAsset<T>(string assetPath) where T:Object {
                string assetBundleName = AssetPathController.GetAssetBundleName(assetPath);
                string assetName = Path.GetFileNameWithoutExtension(assetPath);
#if UNITY_EDITOR
                if (AppConst.SimulateAssetBundleInEditor)
                {
                    string[] assetPaths = UnityEditor.AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName(assetBundleName, assetName);
                    if (assetPaths.Length == 0)
                    {
                        Debug.LogError("There is no asset with name \"" + assetName + "\" in " + assetBundleName);
                        return null;
                    }
                    return UnityEditor.AssetDatabase.LoadMainAssetAtPath(assetPaths[0]) as T;
                }
                else
#endif
                {
                    AssetBundle bundle = _GetBundle(assetBundleName);
                    return bundle.LoadAsset<T>(assetName);
                }
            }

            private AssetBundle _GetBundle(string bundleName) {
                if (!this.assetBundleDic.ContainsKey(bundleName))
                {
                    this.assetBundleDic[bundleName] = AssetBundle.LoadFromFile(_basePath + bundleName);
                }
                return this.assetBundleDic[bundleName];
            }

			public void Dispose(){
				
			}
        }
    }
}