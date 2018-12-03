using System.Collections;
using System.Collections.Generic;
using System.IO;
using Framework.Util;
using UnityEngine;
using UnityEngine.Networking;

namespace Framework.Core
{
    namespace Assistant
    {
        public class NetWorkUtility
        {
            public static UnityWebRequestAsyncOperation DownLoadZip(string url,string savePath)
            {
                UnityWebRequest request = UnityWebRequest.Get(url);
                DownloadHandleContinue handle = new DownloadHandleContinue(savePath);
                request.disposeDownloadHandlerOnDispose = true;
                request.SetRequestHeader("Range", "bytes=" + handle.GetLocalSize() + "-"); //断点续传设置读取文件数据流开始索引，成功会返回206
                request.downloadHandler = handle;
                return request.SendWebRequest();
            }

			public static IEnumerator GetHttpContent(string url, System.Action<bool, string> onComplete)
			{
				using (UnityWebRequest request = UnityWebRequest.Get(url))
				{
					yield return request.SendWebRequest();
					bool isError = request.isNetworkError || request.isHttpError;
					string content = isError ? request.error : request.downloadHandler.text;
					onComplete?.Invoke(isError, content);
				}
			}

			public static IEnumerator GetHttpContentWithRange(string url,string rangeInfo, System.Action<bool, string> onComplete)
			{
				using (UnityWebRequest request = UnityWebRequest.Get(url))
				{
					request.disposeDownloadHandlerOnDispose = true;
					request.SetRequestHeader("Range", "bytes=" + rangeInfo); //断点续传设置读取文件数据流开始索引，成功会返回206
					yield return request.SendWebRequest();
					bool isError = request.isNetworkError || request.isHttpError;
					string content = isError ? (request.error + ", url:" + url) : request.downloadHandler.text;
					onComplete?.Invoke(isError, content);
				}
			}

			public static IEnumerator GetHttpBundle(string url,string rangeInfo,string savePath)
			{
				using (UnityWebRequest request = UnityWebRequest.Get(url))
				{
					request.disposeDownloadHandlerOnDispose = true;
					request.SetRequestHeader("Range", "bytes=" + rangeInfo); //断点续传设置读取文件数据流开始索引，成功会返回206
					request.downloadHandler = new DownloadHandlerFile(savePath);
					yield return request.SendWebRequest();
				}
			}
        }
    }
}