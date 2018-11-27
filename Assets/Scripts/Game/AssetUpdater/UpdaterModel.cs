using Framework.Core.Assistant;
using Framework.Core.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.IO;
using Framework.Util;

namespace Framework
{
    namespace Game
    {
        public class UpdaterModel : SingletonMono<UpdaterModel>
        {
            public int updateState = 0;
            private static string[] NOTICES = new[] {
                "获取补丁包列表",
                "下载版本:{0},补丁包大小:{1}",
                "解压补丁包...",
                "更新版本:{0}完成",
                "即将进入游戏"
            };


            private const string APP_INFO_FILENAME = PathConst.FileServerAddress + PathConst.AppInfoFileName;

            private List<PackerInfo> _waitDownLoadPackerInfoList;
            private UpdaterPresender _presender;
            private System.Action _onUpdateAssetFinish;

            protected override void onInit()
            {
                this._presender = UpdaterPresender.Create();
            }

            public void Lanucher(System.Action onUpdateAssetFinish)
            {
                this._onUpdateAssetFinish = onUpdateAssetFinish;
                updateState = 0;
                StartCoroutine(DownLoadPackerInfoList());
            }

            private IEnumerator DownLoadPackerInfoList()
            {
                _presender.SetNotice(NOTICES[updateState]);
                yield return NetWorkUtility.GetHttpContent(APP_INFO_FILENAME, (isError, content) =>
                {
                    if (!isError)
                    {
                        _waitDownLoadPackerInfoList = new List<PackerInfo>();

                        string[] jsonObj = content.Trim().Split('\n');
                        for (int i = 0; i < jsonObj.Length; i++)
                            _waitDownLoadPackerInfoList.Add(PackerInfo.ValueOf(jsonObj[i].Trim()));
                    }
                });
                yield return new WaitUntil(() => _waitDownLoadPackerInfoList != null);
                updateState = 1;

                string curVersion = AppFacade.Instance.AssetVersion;
                string dstVersion = "1.1.3";
                yield return DownLoadPackerList(CollectNeedUpdatePacker(curVersion, dstVersion, _waitDownLoadPackerInfoList));
            }

            private List<PackerInfo> CollectNeedUpdatePacker(string scrVersion, string dstVersion, List<PackerInfo> allPackerList)
            {
                string curVersion = scrVersion;
                PackerInfo tmpPacker;
                List<PackerInfo> needDownLoadPackerList = new List<PackerInfo>();
                List<PackerInfo> tmpPackerInfoList;

                while (!curVersion.Equals(dstVersion))
                {
                    tmpPackerInfoList = allPackerList.FindAll(a => a.fromVersion.Equals(curVersion));
                    tmpPacker = tmpPackerInfoList[tmpPackerInfoList.Count - 1];
                    curVersion = tmpPacker.toVersion;
                    needDownLoadPackerList.Add(tmpPacker);
                }

                for (int i = 0; i < needDownLoadPackerList.Count; i++)
                {
                    Debug.Log("NeedDownLoad:" + needDownLoadPackerList[i].packerName);
                }

                return needDownLoadPackerList;
            }


            private IEnumerator DownLoadPackerList(List<PackerInfo> needDownLoadList)
            {
                while (needDownLoadList.Count > 0)
                {
                    yield return DownLoadPackerBase(needDownLoadList[0]);
                    needDownLoadList.RemoveAt(0);
                }
                yield return new WaitUntil(()=> needDownLoadList.Count == 0);
                updateState = 4;
                _presender.SetNotice(NOTICES[updateState]);
                yield return new WaitForSeconds(0.5f);
                this.Complete();
            }

            private IEnumerator DownLoadPackerBase(PackerInfo packerInfo)
            {
                updateState = 1;
                string notice = string.Format(NOTICES[updateState], packerInfo.packerName, packerInfo.packerSize);
                _presender.SetNotice(notice);

                string packerFileName = packerInfo.packerName + ".gzip";
                string url = PathConst.FileServerAddress + packerFileName;
                string saveFullPath = PathConst.PersistentDataPath + packerFileName;

                UnityWebRequestAsyncOperation operation = NetWorkUtility.DownLoadZip(url, saveFullPath);
                DownloadHandleContinue handle = operation.webRequest.downloadHandler as DownloadHandleContinue;
                while (!operation.isDone)
                {
                    yield return null;
                    _presender.SetProgress(operation.progress);
                }

                yield return new WaitUntil(() => operation.isDone);

                if (operation.webRequest.isHttpError || operation.webRequest.isNetworkError)
                {
                    Debug.Log(operation.webRequest.error);
                }
                else
                {
                    Debug.Log("==================================================");
                    string tmpDir = PathConst.PersistentDataPath + "/tmp" + packerInfo.packerName + "/";
                    Debug.Log(tmpDir);
                    CompressionHelper.DeCompress(saveFullPath, tmpDir);
                    string baseAssetBundlePath = PathConst.PersistentDataPath + PathConst.BundleDirName + "/";
                    List<BundleInfo> oldBundleList = LoadBundleList(baseAssetBundlePath + PathConst.BUNDLE_INFO_LIST_FILE_NAME);
                    List<BundleInfo> curBundleList = LoadBundleList(tmpDir + PathConst.BUNDLE_INFO_LIST_FILE_NAME);

                    for (int i = 0; i < curBundleList.Count; i++)
                    {
                        BundleInfo curBundleInfo = curBundleList[i];
                        string dstPath = baseAssetBundlePath + curBundleList[i].Name;

                        if (curBundleInfo.state < 0)
                        {
                            if (File.Exists(dstPath))
                            {
                                File.Delete(dstPath);
                                oldBundleList.RemoveAll(a=>a.Name.Equals(curBundleInfo.Name));
                            }
                        }
                        else
                        {
                            FileUtility.FileCopy(tmpDir + curBundleList[i].Name, dstPath);
                            oldBundleList.Add(curBundleInfo);
                        }
                    }

                    SaveBundleList(baseAssetBundlePath + PathConst.BUNDLE_INFO_LIST_FILE_NAME, oldBundleList);
                    updateState = 2;
                    _presender.SetNotice(NOTICES[updateState]);
                    string relativePath = Path.Combine(PathConst.BundleDirName, PathConst.AssetVersionFileName);
                    string versionFilePath = PathConst.PersistentDataPath + relativePath;
                    File.WriteAllText(versionFilePath, packerInfo.toVersion);

                    yield return new WaitForSeconds(0.2f);
                }
            }

            private List<BundleInfo> LoadBundleList(string bundleListFilePath)
            {
                List<BundleInfo> bundleList = new List<BundleInfo>();
                if (File.Exists(bundleListFilePath))
                {
                    bundleList = JsonConvert.DeserializeObject<List<BundleInfo>>(File.ReadAllText(bundleListFilePath));
                }
                return bundleList;
            }

            private void SaveBundleList(string savePath, List<BundleInfo> bundleList)
            {
                string bundleListStr = JsonConvert.SerializeObject(bundleList);
                File.WriteAllText(savePath, bundleListStr);
            }

            private void Complete()
            {
                if (_onUpdateAssetFinish != null)
                {
                    this._onUpdateAssetFinish();
                }
                this._presender.Release();
                base.Release();
            }
        }
    }
}
