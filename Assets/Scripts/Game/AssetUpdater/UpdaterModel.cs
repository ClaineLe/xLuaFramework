﻿using Framework.Core.Assistant;
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
                    Debug.Log(isError + ":" + content);
                    if (!isError)
                    {
                        List<PackerInfo> allPackerInfoList = JsonConvert.DeserializeObject<List<PackerInfo>>(content);
                        _waitDownLoadPackerInfoList = CollectNeedUpdatePacker(allPackerInfoList);
                    }
                });
                yield return new WaitUntil(() => _waitDownLoadPackerInfoList != null);
                updateState = 1;
            }

            private List<PackerInfo> CollectNeedUpdatePacker(List<PackerInfo> allPackerList)
            {
                return null;
            }

            private IEnumerator DownLoadPackerList()
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
                string saveFullPath = PathConst.LocalAssetCacheDirPath + packerInfo.packerName;

                UnityWebRequestAsyncOperation operation = NetWorkUtility.DownLoadZip(url, saveFullPath);
                DownloadHandleContinue handle = operation.webRequest.downloadHandler as DownloadHandleContinue;
                while (!operation.isDone)
                {
                    yield return null;
                    _presender.SetProgress(operation.progress);
                }
                yield return new WaitUntil(() => operation.isDone);
                CompressionHelper.DeCompress(saveFullPath, "");
                UpdateLocalAssetVersion(packerInfo.toVersion);
                yield return new WaitForSeconds(0.2f);
            }

            private void UpdateLocalAssetVersion(string dstVersion)
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
