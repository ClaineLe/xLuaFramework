using Framework.Core.Assistant;
using Framework.Core.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace Framework
{
	namespace Game
	{
        public class UpdaterModel : SingletonMono<UpdaterModel>
		{
			private static string[] NOTICES = new[] {
				"获取补丁包列表",
				"下载版本:{0},补丁包大小:{1}",
				"解压补丁包...",
				"更新版本:{0}完成",
				"即将进入游戏"
			};

            private AppInfo appInfo;
            public int updateState = 0;
            private UpdaterPresender _presender;
            private System.Action _onUpdateAssetFinish;

            private static string APP_INFO_FILENAME {
				get { 
					return PathConst.FileServerAddress + (PathConst.CurChangePlatformRelativePath + "_" + PathConst.BUNDLE_INFO_LIST_FILE_NAME).ToLower ();
                }
			}

			private static string APP_List_FILENAME {
				get { 
					return PathConst.FileServerAddress + (PathConst.CurChangePlatformRelativePath + "_" + PathConst.BundleDirName).ToLower();
                }
			}

            private ClientBundleInfo clientBundleInfo{
                get{
                    return AppFacade.Instance.m_ClientBundleInfo;
                }
            }

			protected override void onInit ()
			{
				this._presender = UpdaterPresender.Create ();
			}

			public void Lanucher (System.Action onUpdateAssetFinish)
			{
				this._onUpdateAssetFinish = onUpdateAssetFinish;
				updateState = 0;
				StartCoroutine (StartSyncBundle ());
			}

			public IEnumerator StartSyncBundle ()
			{
				yield return DownLoadBundleInfo ();
				yield return DownLoadBundleList ();
                UpdateClientResVersion(appInfo.resVersion);
                Complete ();
			}

			private IEnumerator DownLoadBundleInfo ()
			{
				_presender.SetNotice (NOTICES [updateState]);
				yield return NetWorkUtility.GetHttpContent (APP_INFO_FILENAME, (isError, content) => {
                    if (isError)
                    {
                        Debug.Log("url:" + APP_INFO_FILENAME + ", content:" + content);
                    }
                    else {
                        appInfo = AppInfo.ValuleOf(content);
                    }
                });
			}

            private IEnumerator DownLoadBundleList ()
			{
                string rangge = "-" + appInfo.listContentSize;
				List<BundleInfo> diffList = null;
                List<BundleInfo> serverList = null;

                yield return NetWorkUtility.GetHttpContentWithRange (APP_List_FILENAME + ".txt", rangge, (isError, serverListStr) => {

                    Debug.Log("serverResVersion:" + appInfo.resVersion);
                    Debug.Log("clientResVersion:" + clientBundleInfo.resVersion);

                    switch (clientBundleInfo.resVersion.CompareTo(appInfo.resVersion))
                    {
                        case -1:
                            {
                                Debug.Log("需要更新");
                                serverList = Parse(serverListStr);
                                serverList.RemoveAll(a => clientBundleInfo.bundleList.Exists(b => a.md5.Equals(b.md5)));
                                diffList = serverList;
                                break;
                            }
                        case 0:
                            {
                                Debug.Log("无需更新");
                                break;
                            }
                        case 1:
                            {
                                Debug.LogError("客户端资源版本号大于服务器资源版本号");
                                break;
                            }
                    }
				});
				if (diffList != null) {
					for (int i = 0; i < diffList.Count; i++) {
                        yield return DownLoadBundleBase (diffList [i]);
                        AddToLocal(diffList[i]);
                    }
				}
			}
			private void AddToLocal(BundleInfo abInfo){

                BundleBaseInfo baseInfo = new BundleBaseInfo();
                baseInfo.md5 = abInfo.md5;
                baseInfo.name = abInfo.name;

                int idx = clientBundleInfo.bundleList.FindIndex (a => a.name.Equals (baseInfo.name));
				if (idx >= 0) {
                    clientBundleInfo.bundleList[idx] = baseInfo;
				} else {
                    clientBundleInfo.bundleList.Add (baseInfo);
				}
				RefreshBundleInfoList ();
			}

			private void RefreshBundleInfoList(){
				string path = PathConst.PersistentDataPath + PathConst.BundleDirName + "/" + PathConst.BUNDLE_INFO_LIST_FILE_NAME;
				File.WriteAllText (path, clientBundleInfo.ToString());
			}

            private void UpdateClientResVersion(int resVersion) {
                clientBundleInfo.resVersion = resVersion;
                RefreshBundleInfoList();
            }

			private IEnumerator DownLoadBundleBase (BundleInfo bundleInfo)
			{
				long startPos = bundleInfo.position;
				long endPos = startPos + bundleInfo.len - 1;
				string rangge = string.Format ("{0}-{1}", startPos, endPos);
				string savePath = PathConst.PersistentDataPath + PathConst.BundleDirName + "/" + bundleInfo.name;
				yield return NetWorkUtility.GetHttpBundle (APP_List_FILENAME + ".txt", rangge, savePath);
			}

			private List<BundleInfo> Parse (string content)
			{
				List<BundleInfo> abInfos = new List<BundleInfo> ();
				if (!string.IsNullOrEmpty (content)) {
					string[] data = content.Trim ().Split ('\n');
					for (int i = 0; i < data.Length; i++) {
						abInfos.Add (BundleInfo.ValuleOf(data[i]));
					}
				}	
				return abInfos;
			}

			private IEnumerator CheckAppVersion ()
			{
				yield break;
			}

			private void Complete ()
			{
				if (_onUpdateAssetFinish != null) {
					this._onUpdateAssetFinish ();
				}
				this._presender.Release ();
				base.Release ();
			}
		}
    }
}
