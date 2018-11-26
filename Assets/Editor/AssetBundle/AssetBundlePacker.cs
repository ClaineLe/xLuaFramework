using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using Framework.Game;
using Framework.Core.Assistant;
using System.Linq;
using Newtonsoft.Json;
using Framework.Util;

namespace Framework.Editor
{
	namespace AssetBundle
	{
        public class AssetBundlePacker
        {
            private const string PACKER_DIR_PATH = "OutPut/";
            private const string PACKER_INFO_LIST_FILE = PACKER_DIR_PATH + "packer_list.txt";

            private const string PACKER_NAME_FORMAT = "{0}_{1}-{2}_to_{3}_packer";
            private const string BUNDLE_INFO_LIST_FILE_NAME = "bundle_info.txt";
            private const string PACKER_DIR_NAME_FORMAT = "Packer/{0}_to_{1}";

            private static  string BasePathFormat = PathConst.BuildBundleRootPath + PathConst.CurChangePlatformRelativePath + "{0}/{1}/";
                   
            public static void BuildPacker_all(string fromVer, string toVer)
            {
                string[] assetPath = {
                    "res",
                    "lua",
                    "xls",
                };

                string[] fromVerNumFlags = fromVer.Split('.');
                string[] toVerNumFlags = toVer.Split('.');

                string packerDirName = string.Format(PACKER_NAME_FORMAT, AppConst.Change, AppConst.GetPlatformName(), fromVer, toVer).ToLower();
                string packerFullPath = Path.Combine(PACKER_DIR_PATH, packerDirName);

                if (Directory.Exists(packerFullPath))
                    Directory.Delete(packerFullPath, true);
                Directory.CreateDirectory(packerFullPath);


                List<FileInfo> asset_FileInfos = new List<FileInfo>();
                List<BundleInfo> asset_BundleInfos = new List<BundleInfo>();

                for (int i = 0; i < assetPath.Length; i++)
                    asset_BundleInfos.AddRange(ExtractPacker(assetPath[i], fromVerNumFlags[i], toVerNumFlags[i], ref asset_FileInfos));

                for (int i = 0; i < asset_FileInfos.Count; i++)
                {
                    string dstPath = Path.Combine(packerFullPath, asset_FileInfos[i].Name);
                    asset_FileInfos[i].CopyTo(dstPath);
                }

                string jsonStr = JsonConvert.SerializeObject(asset_BundleInfos);
				string bundleInfoListFullPath = Path.Combine(packerFullPath, BUNDLE_INFO_LIST_FILE_NAME);
				File.WriteAllText(bundleInfoListFullPath, jsonStr);

				string zipPackerFullPath = packerFullPath + ".gzip";
				CompressionHelper.Compress(packerFullPath + "/", zipPackerFullPath);
                //Directory.Delete(packerFullPath, true);


                PackerInfo packerInfo = new PackerInfo();
                packerInfo.packerName = string.Format(PACKER_NAME_FORMAT, AppConst.Change, AppConst.GetPlatformName(), fromVer, toVer);
                packerInfo.fromVersion = fromVer;
                packerInfo.toVersion = toVer;
				packerInfo.packerSize = FileUtility.GetFileSize(zipPackerFullPath);
				packerInfo.totalSize = FileUtility.GetFileSize(bundleInfoListFullPath);
                for (int i = 0; i < asset_BundleInfos.Count; i++)
                {
                    BundleInfo bundleInfo = asset_BundleInfos[i];
                    if (bundleInfo.state >= 0)
                    {
                        packerInfo.fileCnt++;
                        packerInfo.totalSize += bundleInfo.Size;
                    }
                }
                List<PackerInfo> packerInfoList = LoadPackerInfoList();
                int index = packerInfoList.FindIndex(a => a.packerName.Equals(packerInfo.packerName));
                if (index >= 0)
                    packerInfoList[index] = packerInfo;
                else
                    packerInfoList.Add(packerInfo);

                SavePackerInfoList(packerInfoList);

            }

            public static void BuildPacker_res(string fromVer, string toVer)
            {
                string relativePath = "res";
                BuildPackerBase(relativePath, fromVer, toVer);
            }

            public static void BuildPacker_lua(string fromVer, string toVer)
            {
                string relativePath = "lua";
                BuildPackerBase(relativePath, fromVer, toVer);
            }

            public static void BuildPacker_xls(string fromVer, string toVer)
            {
                string relativePath = "xls";
                BuildPackerBase(relativePath, fromVer, toVer);
            }

            private static List<PackerInfo> LoadPackerInfoList()
            {
                if (!File.Exists(PACKER_INFO_LIST_FILE))
                    return new List<PackerInfo>();
                string jsonStr = File.ReadAllText(PACKER_INFO_LIST_FILE);
                List<PackerInfo> packerInfoList = JsonConvert.DeserializeObject<List<PackerInfo>>(jsonStr);
                return packerInfoList;
            }

            private static void SavePackerInfoList(List<PackerInfo> packerInfoList)
            {
                if (!Directory.Exists(PACKER_DIR_PATH))
                    Directory.CreateDirectory(PACKER_DIR_PATH);

                string jsonStr = JsonConvert.SerializeObject(packerInfoList);
                File.WriteAllText(PACKER_INFO_LIST_FILE, jsonStr);
            }

            private static List<BundleInfo> ExtractPacker(string relativePath, string fromVer, string toVer, ref List<FileInfo> bundleFileInfos)
            {
                if (fromVer.Equals(toVer))
                    return new List<BundleInfo>();

                string packerPath = string.Format(PACKER_DIR_NAME_FORMAT, fromVer, toVer);
                DirectoryInfo dirInfo = new DirectoryInfo(string.Format(BasePathFormat, relativePath, packerPath));
                if (!dirInfo.Exists)
                {
                    BuildPackerBase(relativePath, fromVer, toVer);
                }
                Debug.Log(dirInfo.GetFiles());
                bundleFileInfos.AddRange(dirInfo.GetFiles());
                int idx = bundleFileInfos.FindIndex(a => a.Name.Equals(BUNDLE_INFO_LIST_FILE_NAME));
                string jsonStr = File.ReadAllText(bundleFileInfos[idx].FullName);
                List<BundleInfo> bundleInfoList = JsonConvert.DeserializeObject<List<BundleInfo>>(jsonStr);
                bundleFileInfos.RemoveAt(idx);
                return bundleInfoList;
            }

            private static void BuildPackerBase(string relativePath, string fromVer, string toVer)
            {
                string packerPath = string.Format(PACKER_DIR_NAME_FORMAT, fromVer, toVer);
                string packerFullPath = string.Format(BasePathFormat, relativePath, packerPath);
                string scrFullPath = string.Format(BasePathFormat, relativePath, fromVer);
                string dstFullPath = string.Format(BasePathFormat, relativePath, toVer);

                List<BundleInfo> diffBundleInfoList = GetDiffBase(scrFullPath, dstFullPath);

                if (Directory.Exists(packerFullPath))
                    Directory.Delete(packerFullPath, true);
                Directory.CreateDirectory(packerFullPath);

                for (int i = 0; i < diffBundleInfoList.Count; i++)
                {
                    BundleInfo bundleInfo = diffBundleInfoList[i];
                    if (bundleInfo.state == -1)
                        continue;
                    string scrFileFullPath = Path.Combine(dstFullPath, bundleInfo.Name);
                    string dstFileFullPath = Path.Combine(packerFullPath, bundleInfo.Name);
                    File.Copy(scrFileFullPath, dstFileFullPath, true);
                }

                string jsonStr = JsonConvert.SerializeObject(diffBundleInfoList);
                File.WriteAllText(Path.Combine(packerFullPath, BUNDLE_INFO_LIST_FILE_NAME), jsonStr);
            }

            private static List<BundleInfo> CollectBundleInfo(string fullPath)
            {
                DirectoryInfo dirInfo = new DirectoryInfo(fullPath);
                List<FileInfo> bundleFileInfo = new List<FileInfo>(dirInfo.GetFiles());
                bundleFileInfo.RemoveAll(a => !string.IsNullOrEmpty(a.Extension));

                List<BundleInfo> bundleInfoList = new List<BundleInfo>();
                for (int i = 0; i < bundleFileInfo.Count; i++)
                {
                    BundleInfo bundleInfo = new BundleInfo();
                    bundleInfo.Name = Path.GetFileNameWithoutExtension(bundleFileInfo[i].Name);
                    bundleInfo.MD5 = MD5Helper.GetMD5HashFromFile(bundleFileInfo[i].FullName);
                    bundleInfo.Size = bundleFileInfo[i].Length;
                    bundleInfoList.Add(bundleInfo);
                }
                return bundleInfoList;
            }

            private static List<string> CollectBundleName(List<BundleInfo> bundleInfoList)
            {
                List<string> bundleNameList = new List<string>();
                for (int i = 0; i < bundleInfoList.Count; i++)
                {
                    bundleNameList.Add(bundleInfoList[i].Name);
                }
                return bundleNameList;
            }

            private static void SetBundleInfoState(List<BundleInfo> bundleInfoList, int state)
            {
                for (int i = 0; i < bundleInfoList.Count; i++)
                    bundleInfoList[i].state = state;
            }

            private static List<BundleInfo> GetDiffBase(string fromFullPath, string toFullPath)
            {
                Debug.Log(fromFullPath);
                Debug.Log(toFullPath);

                List<BundleInfo> fromBundleInfos = CollectBundleInfo(fromFullPath);
                List<BundleInfo> toBundleInfos = CollectBundleInfo(toFullPath);

                List<string> fromBundleName = CollectBundleName(fromBundleInfos);
                List<string> toBundleName = CollectBundleName(toBundleInfos);

                List<string> addBundleList = toBundleName.Except(fromBundleName).ToList();
                List<string> delBundleList = fromBundleName.Except(toBundleName).ToList();

                List<BundleInfo> addBundleInfoList = toBundleInfos.FindAll(a => addBundleList.Exists(b => a.Name.Equals(b)));
                List<BundleInfo> delBundleInfoList = fromBundleInfos.FindAll(a => delBundleList.Exists(b => a.Name.Equals(b)));

				List<BundleInfo> updBundleInfoList = toBundleInfos;
				updBundleInfoList.RemoveAll(a => addBundleInfoList.Exists(b => a.MD5 == b.MD5));
				updBundleInfoList.RemoveAll(a => fromBundleInfos.Exists(b => a.MD5 == b.MD5));

                SetBundleInfoState(delBundleInfoList, -1);
                SetBundleInfoState(updBundleInfoList, 0);
                SetBundleInfoState(addBundleInfoList, 1);

                List<BundleInfo> diffBundleInfoList = new List<BundleInfo>();
                diffBundleInfoList.AddRange(updBundleInfoList);
                diffBundleInfoList.AddRange(addBundleInfoList);
                diffBundleInfoList.AddRange(delBundleInfoList);
                return diffBundleInfoList;
            }
        }
	}
}