using Framework.Core.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Framework
{
    namespace Game
    {
        public class UpdaterModel : SingletonMono<UpdaterModel>
        {
            private const string APP_INFO_FILENAME = AppConst.FileServerAddress + AppConst.AppInfoFileName;

            private UpdaterPresender _presender;
            private System.Action _onUpdateAssetFinish;

            protected override void onInit()
            {
                this._presender = UpdaterPresender.Create();
            }

            public void Lanucher(System.Action onUpdateAssetFinish)
            {
                this._onUpdateAssetFinish = onUpdateAssetFinish;
                StartCoroutine(LoadServerAppInfo(str=> {
                    Debug.Log(str);
                    this.Complete();
                }));
            }

            private IEnumerator LoadServerAppInfo(System.Action<string> appInfo_callback) {
                using (UnityWebRequest www = UnityWebRequest.Get(APP_INFO_FILENAME))
                {
                    yield return www.SendWebRequest();
                    string str = string.Empty;
                    if (www.isNetworkError || www.isHttpError)
                    {
                        str = www.error;
                    }
                    else
                    {
                        str = www.downloadHandler.text;
                    }
                    if (appInfo_callback != null)
                        appInfo_callback(str);
                }
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
