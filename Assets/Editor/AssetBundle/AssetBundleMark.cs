using System.IO;
using Framework.Code.Manager;
using Framework.Util;
using UnityEditor;
using UnityEngine;

namespace Framework.Editor
{
    namespace AssetBundle
    {
		public class AssetBundleMark
        {
            static public void MarkAssets(){
                
            }

            static public void MarkLua(){
                string luaRootPath = Path.Combine(Application.dataPath, LuaManager.LuaRootPath);
                DirectoryInfo directoryInfo = new DirectoryInfo(luaRootPath);
                DirectoryInfo[] directoryInfos = directoryInfo.GetDirectories("*.*",SearchOption.AllDirectories);
                string subPath = "Assets/" + LuaManager.AppAssetsRootPath;
                for (int i = 0; i < directoryInfos.Length;i++){
                    string assetPath = "Assets/" + directoryInfos[i].FullName.Substring(Application.dataPath.Length+1);
                    AssetImporter assetImport = AssetImporter.GetAtPath(assetPath);
                    string abName = assetPath.Substring(subPath.Length).Replace("/", "_").Replace("\\", "_").ToLower();
                    if(!string.Equals(abName, assetImport.assetBundleName)){
                        assetImport.assetBundleName = abName;
                        assetImport.SaveAndReimport();
                    }
                }
            }

            static public void Clean()
            {
                string[] abNames = AssetDatabase.GetAllAssetBundleNames();
                for (int i = 0; i < abNames.Length;i++){
                    AssetDatabase.RemoveAssetBundleName(abNames[i], true);
                }
            }

            static private void Mark(string assetFullPath, bool singleton){
                
            }

            static private void BaseMark(string assetDir, bool singletonAsset){
                if(DirectoryUtility.Exists(assetDir))
                {
                    if(singletonAsset){
                        
                    }
                    else{
                        
                    }
                }
            }
        }
    }
}