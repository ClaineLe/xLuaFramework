﻿using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Framework.Game;
using System.IO;

namespace Framework
{
	namespace Code.Manager
	{
		public partial class ManagerName
		{
			public const string Asset = "AssetManager";
		}

		public class AssetManager : BaseManager<AssetManager>, IManager
		{
			public BundleCenter m_BundleCenter;

			public void Init ()
			{
				m_BundleCenter = new BundleCenter ();
			}

			public LoadedBundle GetLoadedAssetBundle (string assetBundleName, out string error)
			{
				return m_BundleCenter.GetLoadedAssetBundle (assetBundleName, out error);
			}

			public void Release ()
			{
				this.m_BundleCenter = null;
			}

			public void Tick ()
			{
				this.m_BundleCenter.Tick ();
			}

			public void Print ()
			{
				Debug.Log ("AssetManager");
			}

			private IEnumerator LoadAssetAsyncBase (string assetPath, System.Type type, UnityAction<BaseLoadAssetOperation> callback)
			{
				string assetbundlename = GetAssetBundleName (assetPath);
				string assetname = Path.GetFileNameWithoutExtension (assetPath);
				BaseLoadAssetOperation request = this.m_BundleCenter.LoadAssetAsync (assetbundlename, assetname, type);
				if (request == null)
					yield break;
				yield return request;
				if (callback != null) {
					callback (request);
				}
			}

			private IEnumerator LoadSceneAsyncBase (string assetPath, bool isAdditive, UnityAction callback)
			{
				string assetbundlename = GetAssetBundleName (assetPath);
				string assetname = Path.GetFileNameWithoutExtension (assetPath);
				LoadOperation request = this.m_BundleCenter.LoadSceneAsync (assetbundlename, assetname, isAdditive);
				if (request == null)
					yield break;
				yield return request;
				if (callback != null)
					callback ();
			}

			private string GetAssetBundleName (string assetPath)
			{
				string dirName = Path.GetDirectoryName (assetPath);
				string fileName = Path.GetFileNameWithoutExtension (assetPath);
				return (dirName + "/" + fileName).Replace ('/', '_').Replace ('\\', '_').ToLower ();
			}


			public UnityEngine.Object LoadAsset(string assetPath, System.Type type){
				string assetBundleName = GetAssetBundleName (assetPath);
				string assetName = Path.GetFileNameWithoutExtension (assetPath);
				return m_BundleCenter.LoadAsset (assetBundleName, assetName, type);
			}

			public void LoadAssetAsync(string assetPath, UnityAction<UnityEngine.Object> callback, System.Type type)
			{
				AppFacade.Instance.StartCoroutine (LoadAssetAsyncBase (assetPath, type, request => {
					if (callback != null)
						callback (request.GetAsset());
				}));
			}


			public void LoadScene(string scenePath, bool isAdditive){
				string assetBundleName = GetAssetBundleName (scenePath);
				string assetName = Path.GetFileNameWithoutExtension (scenePath);
				m_BundleCenter.LoadScene (assetBundleName,assetName,isAdditive);
			}


			public void LoadSceneAsync(string scenePath, bool isAdditive, UnityAction callback){
				AppFacade.Instance.StartCoroutine(LoadSceneAsyncBase (scenePath,isAdditive,()=>{
					if(callback != null)
						callback();
				}));
			}
		}
	}
}