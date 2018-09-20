using System.Collections.Generic;
using Framework.Util;
using UnityEditor;
using UnityEngine;

namespace Framework.Editor
{
    namespace AssetBundle
    {
        public class AssetBundleBuild
        {
            static public void BuildAssetBundle(){
                
            }

            static public void BuildPlayer(){
                
            }

            static public void BuildLua(){
                List<string> abNameList = new List<string>(AssetDatabase.GetAllAssetBundleNames());
                abNameList.RemoveAll(a => !a.StartsWith("lua"));
                for (int i = 0; i < abNameList.Count;i++){
                    Debug.Log(abNameList[i]);
                }
                BuildAssetBundleOptions options = BuildAssetBundleOptions.ChunkBasedCompression;
                string outPutPath = Application.dataPath + "/../OutPut";
                DirectoryUtility.Clear(outPutPath);
                BuildPipeline.BuildAssetBundles(outPutPath, options, BuildTarget.StandaloneOSX);
            }
        }
    }
}