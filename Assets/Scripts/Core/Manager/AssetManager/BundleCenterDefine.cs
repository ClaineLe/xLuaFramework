using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Framework.Game;


namespace Framework
{
    namespace Core.Manager
    {
        public abstract class LoadOperation : IEnumerator
        {
            public object Current
            {
                get
                {
                    return null;
                }
            }

            public bool MoveNext()
            {
                return !IsDone();
            }

            public void Reset()
            {
            }

            abstract public bool Update();

            abstract public bool IsDone();
        }

        public class LoadedBundle
        {
            public AssetBundle m_AssetBundle;
            public int m_ReferencedCount;

            internal event System.Action unload;

            internal void OnUnload()
            {
                m_AssetBundle.Unload(false);
                if (unload != null)
                    unload();
            }

            public LoadedBundle(AssetBundle assetBundle)
            {
                m_AssetBundle = assetBundle;
                m_ReferencedCount = 1;
            }
        }

        public abstract class BaseLoadAssetOperation : LoadOperation
        {
            public abstract UnityEngine.Object GetAsset();
        }

#if BUNDLE_MODEL || !UNITY_EDITOR
        public abstract class BaseBundleLoadOperation : LoadOperation
        {
            bool done;

            public string assetBundleName { get; private set; }

            public LoadedBundle assetBundle { get; protected set; }

            public string error { get; protected set; }

            protected abstract bool downloadIsDone { get; }

            protected abstract void FinishDownload();

            public override bool Update()
            {
                if (!done && downloadIsDone)
                {
                    FinishDownload();
                    done = true;
                }

                return !done;
            }

            public override bool IsDone()
            {
                return done;
            }

            public BaseBundleLoadOperation(string assetBundleName)
            {
                this.assetBundleName = assetBundleName;
            }
        }

        public class BundleLoadOperation : BaseBundleLoadOperation
        {
            AssetBundleCreateRequest m_CreateRequest;

            public BundleLoadOperation(string assetBundleName, AssetBundleCreateRequest createRequest)
                : base(assetBundleName)
            {
                if (createRequest == null)
                    throw new System.ArgumentNullException("www");
                this.m_CreateRequest = createRequest;
            }

            protected override bool downloadIsDone { get { return (m_CreateRequest == null) || m_CreateRequest.isDone; } }

            protected override void FinishDownload()
            {
                if (m_CreateRequest == null)
                    return;

                AssetBundle bundle = m_CreateRequest.assetBundle;
                if (bundle == null)
                    error = string.Format("{0} is not a valid asset bundle.", assetBundleName);
                else
                    assetBundle = new LoadedBundle(m_CreateRequest.assetBundle);

                m_CreateRequest = null;
            }

        }

        public class LoadSceneOperation : LoadOperation
        {
            protected string m_AssetBundleName;
            protected string m_LevelName;
            protected bool m_IsAdditive;
            protected string m_DownloadingError;
            protected AsyncOperation m_Request;

            public LoadSceneOperation(string assetbundleName, string levelName, bool isAdditive)
            {
                m_AssetBundleName = assetbundleName;
                m_LevelName = levelName;
                m_IsAdditive = isAdditive;
            }

            public override bool Update()
            {
                if (m_Request != null)
                    return false;

                LoadedBundle bundle = Framework.Game.Manager.AssetMgr.GetLoadedAssetBundle(m_AssetBundleName, out m_DownloadingError);
                if (bundle != null)
                {
                    m_Request = SceneManager.LoadSceneAsync(m_LevelName, m_IsAdditive ? LoadSceneMode.Additive : LoadSceneMode.Single);
                    return false;
                }
                else
                    return true;
            }

            public override bool IsDone()
            {
                // Return if meeting downloading error.
                // m_DownloadingError might come from the dependency downloading.
                if (m_Request == null && m_DownloadingError != null)
                {
                    Debug.LogError(m_DownloadingError);
                    return true;
                }

                return m_Request != null && m_Request.isDone;
            }
        }

        public class LoadAssetOperation : BaseLoadAssetOperation
        {
            protected string m_AssetBundleName;
            protected string m_AssetName;
            protected string m_DownloadingError;
            protected System.Type m_Type;
            protected AssetBundleRequest m_Request = null;

            public LoadAssetOperation(string bundleName, string assetName, System.Type type)
            {
                m_AssetBundleName = bundleName;
                m_AssetName = assetName;
                m_Type = type;
            }

            public override UnityEngine.Object GetAsset()
            {
                if (m_Request != null && m_Request.isDone)
                    return m_Request.asset;
                else
                    return null;
            }

            // Returns true if more Update calls are required.
            public override bool Update()
            {
                if (m_Request != null)
                    return false;

                LoadedBundle bundle = Framework.Game.Manager.AssetMgr.GetLoadedAssetBundle(m_AssetBundleName, out m_DownloadingError);
                if (bundle != null)
                {
                    ///@TODO: When asset bundle download fails this throws an exception...
                    m_Request = bundle.m_AssetBundle.LoadAssetAsync(m_AssetName, m_Type);
                    return false;
                }
                else
                {
                    return true;
                }
            }

            public override bool IsDone()
            {
                // Return if meeting downloading error.
                // m_DownloadingError might come from the dependency downloading.
                if (m_Request == null && m_DownloadingError != null)
                {
                    Debug.LogError(m_DownloadingError);
                    return true;
                }

                return m_Request != null && m_Request.isDone;
            }
        }

#else
        public class LoadSceneSimulationOperation : LoadOperation
        {
            AsyncOperation m_Operation = null;

            public LoadSceneSimulationOperation(string assetBundleName, string levelName, bool isAdditive)
            {
                string[] levelPaths = UnityEditor.AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName(assetBundleName, levelName);
                if (levelPaths.Length == 0)
                {
                    ///@TODO: The error needs to differentiate that an asset bundle name doesn't exist
                    //        from that there right scene does not exist in the asset bundle...

                    Debug.LogError("There is no scene with name \"" + levelName + "\" in " + assetBundleName);
                    return;
                }
		#if UNITY_2017
				if(isAdditive)
				{
					m_Operation = UnityEditor.SceneManagement.EditorSceneManager.LoadSceneAsync(levelPaths[0],LoadSceneMode.Additive);	
				}
				else{
					m_Operation = UnityEditor.SceneManagement.EditorSceneManager.LoadSceneAsync(levelPaths[0],LoadSceneMode.Single);	
				}
		#elif UNITY_2018
                LoadSceneParameters loadSceneParams = new LoadSceneParameters();
                loadSceneParams.loadSceneMode = isAdditive ? LoadSceneMode.Additive : LoadSceneMode.Single;
                m_Operation = UnityEditor.SceneManagement.EditorSceneManager.LoadSceneAsyncInPlayMode(levelPaths[0], loadSceneParams);
		#endif
            }

            public override bool Update()
            {
                return false;
            }

            public override bool IsDone()
            {
                return m_Operation == null || m_Operation.isDone;
            }
        }
             public class LoadAssetOperationSimulation : BaseLoadAssetOperation
        {
            Object m_SimulatedObject;

            public LoadAssetOperationSimulation(Object simulatedObject)
            {
                m_SimulatedObject = simulatedObject;
            }

            public override UnityEngine.Object GetAsset()
            {
                return m_SimulatedObject;
            }

            public override bool Update()
            {
                return false;
            }

            public override bool IsDone()
            {
                return true;
            }
        }
#endif
    }
}
