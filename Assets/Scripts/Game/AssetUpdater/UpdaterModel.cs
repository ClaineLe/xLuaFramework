using Framework.Core.Assistant;
using Framework.Core.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.IO;

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


            private const string APP_INFO_FILENAME = AppConst.FileServerAddress + AppConst.AppInfoFileName;

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
                string dstVersion = "1.1.5";
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
                return needDownLoadPackerList;
            }


            private IEnumerator DownLoadPackerList(List<PackerInfo> needDownLoadList)
            {
                while (_waitDownLoadPackerInfoList.Count > 0)
                {
                    yield return DownLoadPackerBase(_waitDownLoadPackerInfoList[0]);
                    _waitDownLoadPackerInfoList.RemoveAt(0);
                }
                yield return new WaitUntil(()=> _waitDownLoadPackerInfoList.Count == 0);
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

                string url = AppConst.FileServerAddress + packerInfo.packerName;
                string saveFullPath = PathConst.PersistentDataPath + packerInfo.packerName;

                UnityWebRequestAsyncOperation operation = NetWorkUtility.DownLoadZip(url, saveFullPath);
                DownloadHandleContinue handle = operation.webRequest.downloadHandler as DownloadHandleContinue;
                while (!operation.isDone)
                {
                    yield return null;
                    _presender.SetProgress(operation.progress);
                }
                yield return new WaitUntil(() => operation.isDone);
                CompressionHelper.DeCompress(saveFullPath, "");
                UpdateLocalAssetVersion(packerInfo);
                yield return new WaitForSeconds(0.2f);
            }

            private void UpdateLocalAssetVersion(PackerInfo packerInfo)
            {
                updateState = 2;
                _presender.SetNotice(NOTICES[updateState]);
                /*
                更新本地版本信息逻辑
                */
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
