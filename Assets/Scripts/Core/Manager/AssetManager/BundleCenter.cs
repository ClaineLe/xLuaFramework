using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Framework
{
	namespace Core.Manager
	{
		using Game;

		public class BundleCenter
		{

            public void Release()
            {

            }

#if BUNDLE_MODEL || !UNITY_EDITOR
			private AssetBundleManifest m_AssetBundleManifest = null;
            
			private Dictionary<string, LoadedBundle> m_LoadedAssetBundles = new Dictionary<string, LoadedBundle> ();
			private Dictionary<string, string> m_DownloadingErrors = new Dictionary<string, string> ();
			private List<string> m_DownloadingBundles = new List<string> ();
			private List<LoadOperation> m_InProgressOperations = new List<LoadOperation> ();
			private Dictionary<string, string[]> m_Dependencies = new Dictionary<string, string[]> ();
            public LoadedBundle GetLoadedAssetBundle (string assetBundleName, out string error)
			{
				if (m_DownloadingErrors.TryGetValue (assetBundleName, out error))
					return null;

				LoadedBundle bundle = null;
				m_LoadedAssetBundles.TryGetValue (assetBundleName, out bundle);
				if (bundle == null)
					return null;

				string[] dependencies = null;
				if (!m_Dependencies.TryGetValue (assetBundleName, out dependencies))
					return bundle;

				for (int i = 0; i < dependencies.Length; i++) {
					if (m_DownloadingErrors.TryGetValue (dependencies [i], out error))
						return null;

					LoadedBundle dependentBundle;
					m_LoadedAssetBundles.TryGetValue (dependencies [i], out dependentBundle);
					if (dependentBundle == null)
						return null;
				}
				return bundle;
			}

            
            protected bool LoadAssetBundleInternal (string assetBundleName)
			{
				LoadedBundle bundle = null;
				m_LoadedAssetBundles.TryGetValue (assetBundleName, out bundle);
				if (bundle != null) {
					bundle.m_ReferencedCount++;
					return true;
				}

				try {
                    string bundleFullPath = PathRoute.GetAssetBundleFullPath(assetBundleName);
					AssetBundle assetBundle = AssetBundle.LoadFromFile (bundleFullPath);
					bundle = new LoadedBundle (assetBundle);
				} catch (System.Exception e) {
					string msg = string.Format ("Load Asset Fail. abName:{0} ErrorMsg:{1}", assetBundleName, e.Message);
					Debug.LogError (msg);
				}
				m_LoadedAssetBundles.Add (assetBundleName, bundle);
				return false;
			}

			protected void LoadDependencies (string assetBundleName)
			{
				if (m_AssetBundleManifest == null) {
					Debug.LogError ("Please initialize AssetBundleManifest by calling AssetBundleManager.Initialize()");
					return;
				}

				string[] dependencies = m_AssetBundleManifest.GetAllDependencies (assetBundleName);
				if (dependencies.Length == 0)
					return;

				m_Dependencies.Add (assetBundleName, dependencies);
				for (int i = 0; i < dependencies.Length; i++)
					LoadAssetBundleInternal (dependencies [i]);
			}

			protected void UnloadDependencies (string assetBundleName)
			{
				string[] dependencies = null;
				if (!m_Dependencies.TryGetValue (assetBundleName, out dependencies))
					return;

				for (int i = 0; i < dependencies.Length; i++) {
					UnloadAssetBundleInternal (dependencies [i]);
				}

				m_Dependencies.Remove (assetBundleName);
			}

			protected void UnloadAssetBundleInternal (string assetBundleName)
			{
				string error;
				LoadedBundle bundle = GetLoadedAssetBundle (assetBundleName, out error);
				if (bundle == null)
					return;

				if (--bundle.m_ReferencedCount == 0) {
					bundle.OnUnload ();
					m_LoadedAssetBundles.Remove (assetBundleName);

					Debug.Log (assetBundleName + " has been unloaded successfully");
				}
			}

			public void Tick ()
			{
				for (int i = 0; i < m_InProgressOperations.Count;) {
					var operation = m_InProgressOperations [i];
					if (operation.Update ()) {
						i++;
					} else {
						m_InProgressOperations.RemoveAt (i);
						ProcessFinishedOperation (operation);
					}
				}
			}

			private void ProcessFinishedOperation (LoadOperation operation)
			{
				BaseBundleLoadOperation bundleLoader = operation as BaseBundleLoadOperation;
				if (bundleLoader == null)
					return;

				if (string.IsNullOrEmpty (bundleLoader.error))
					m_LoadedAssetBundles.Add (bundleLoader.assetBundleName, bundleLoader.assetBundle);
				else {
					string msg = string.Format ("Failed downloading bundle {0} from {1}",
						             bundleLoader.assetBundleName, bundleLoader.error);
					m_DownloadingErrors.Add (bundleLoader.assetBundleName, msg);
				}

				m_DownloadingBundles.Remove (bundleLoader.assetBundleName);
			}

            protected void LoadDependenciesAsync(string assetBundleName)
            {
                if (m_AssetBundleManifest == null)
                {
                    Debug.LogError("Please initialize AssetBundleManifest by calling AssetBundleManager.Initialize()");
                    return;
                }

                string[] dependencies = m_AssetBundleManifest.GetAllDependencies(assetBundleName);
                if (dependencies.Length == 0)
                    return;

                m_Dependencies.Add(assetBundleName, dependencies);
                for (int i = 0; i < dependencies.Length; i++)
                    LoadAssetBundleInternalAsync(dependencies[i]);
            }

            protected bool LoadAssetBundleInternalAsync(string assetBundleName)
            {
                LoadedBundle bundle = null;
                m_LoadedAssetBundles.TryGetValue(assetBundleName, out bundle);
                if (bundle != null)
                {
                    bundle.m_ReferencedCount++;
                    return true;
                }

                if (m_DownloadingBundles.Contains(assetBundleName))
                    return true;

                AssetBundleCreateRequest createRequest = null;
                string bundleFullPath = PathRoute.GetAssetBundleFullPath(assetBundleName);
                createRequest = AssetBundle.LoadFromFileAsync(bundleFullPath);
                m_InProgressOperations.Add(new BundleLoadOperation(assetBundleName, createRequest));
                m_DownloadingBundles.Add(assetBundleName);
                return false;
            }
            public BundleCenter ()
			{
                string bundleFullPath = PathRoute.GetAssetBundleFullPath(PathConst.BundleDirName);
                AssetBundle manifestAssetBundle = AssetBundle.LoadFromFile (bundleFullPath);
				m_AssetBundleManifest = manifestAssetBundle.LoadAsset<AssetBundleManifest> ("AssetBundleManifest");
			}

			protected void LoadAssetBundleAsync (string assetBundleName)
			{
				bool isAlreadyProcessed = LoadAssetBundleInternalAsync (assetBundleName);
				if (!isAlreadyProcessed)
					LoadDependenciesAsync (assetBundleName);
			}

			protected void LoadAssetBundle (string assetBundleName)
			{
				bool isAlreadyProcessed = LoadAssetBundleInternal (assetBundleName);
				if (!isAlreadyProcessed)
					LoadDependencies (assetBundleName);
            }

            public void UnloadAssetBundle(string assetBundleName)
            {
                UnloadAssetBundleInternal(assetBundleName);
                UnloadDependencies(assetBundleName);
            }
#endif

            public void LoadScene(string assetBundleName, string sceneName, bool isAdditive)
            {
#if BUNDLE_MODEL || !UNITY_EDITOR
                LoadAssetBundle(assetBundleName);
#endif
                UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName, isAdditive ? UnityEngine.SceneManagement.LoadSceneMode.Additive : UnityEngine.SceneManagement.LoadSceneMode.Single);
            }

            public UnityEngine.Object LoadAsset(string assetBundleName, string assetName, System.Type type)
			{
#if BUNDLE_MODEL || !UNITY_EDITOR
                LoadAssetBundle(assetBundleName);
                string error = string.Empty;
                LoadedBundle loadedBundle = GetLoadedAssetBundle(assetBundleName, out error);
                if (string.IsNullOrEmpty(error))
                    return loadedBundle.m_AssetBundle.LoadAsset(assetName, type);
                return null;
#else
                string[] assetPaths = UnityEditor.AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName(assetBundleName, assetName);
                if (assetPaths.Length == 0)
                {
                    Debug.LogError("There is no asset with name \"" + assetName + "\" in " + assetBundleName);
                    return null;
                }
                return UnityEditor.AssetDatabase.LoadMainAssetAtPath(assetPaths[0]);
#endif
			}


            public BaseLoadAssetOperation LoadAssetAsync (string assetBundleName, string assetName, System.Type type)
			{
				//Debug.Log ("Loading " + assetName + " from " + assetBundleName + " bundle");
				BaseLoadAssetOperation operation = null;
#if BUNDLE_MODEL || !UNITY_EDITOR
                LoadAssetBundleAsync(assetBundleName);
                operation = new LoadAssetOperation(assetBundleName, assetName, type);
                m_InProgressOperations.Add(operation);
#else
                string[] assetPaths = UnityEditor.AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName(assetBundleName, assetName);
                if (assetPaths.Length == 0)
                {
                    Debug.LogError("There is no asset with name \"" + assetName + "\" in " + assetBundleName);
                    return null;
                }
                UnityEngine.Object target = UnityEditor.AssetDatabase.LoadMainAssetAtPath(assetPaths[0]);
                operation = new LoadAssetOperationSimulation(target);
#endif
                return operation;
			}

			public LoadOperation LoadSceneAsync (string assetBundleName, string sceneName, bool isAdditive)
			{
				LoadOperation operation = null;
#if BUNDLE_MODEL || !UNITY_EDITOR
                LoadAssetBundleAsync(assetBundleName);
                operation = new LoadSceneOperation(assetBundleName, sceneName, isAdditive);
                m_InProgressOperations.Add(operation);
#else
                operation = new LoadSceneSimulationOperation(assetBundleName, sceneName, isAdditive);
#endif
                return operation;
			}
		}
	}
}