using System.Collections;
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
			public BundleCenter m_BundleManager;

			public void Init ()
			{
				m_BundleManager = new BundleCenter ();
			}

			private T LoadAsset<T> (string assetPath) where T : Object
			{
#if UNITY_EDITOR
				T asset = UnityEditor.AssetDatabase.LoadAssetAtPath <T> (assetPath);
				return asset;
#else

#endif
				return default(T);
			}

			public LoadedAssetBundle GetLoadedAssetBundle (string assetBundleName, out string error)
			{
				return m_BundleManager.GetLoadedAssetBundle (assetBundleName, out error);
			}

			public GameObject LoadGameObject (string assetPath)
			{
				return LoadAsset<GameObject> (assetPath);
			}

			public Sprite LoadSprite (string assetPath)
			{
				return LoadAsset<Sprite> (assetPath);
			}

			public Texture LoadTexture (string assetPath)
			{
				return LoadAsset<Texture> (assetPath);
			}

			public TextAsset LoadTextAsset (string assetPath)
			{
				return LoadAsset<TextAsset> (assetPath);
			}

			public AudioClip LoadAudioClip (string assetPath)
			{
				return LoadAsset<AudioClip> (assetPath);
			}

			public Shader LoadShader (string assetPath)
			{
				return LoadAsset<Shader> (assetPath);
			}

			public AnimationClip LoadAnimationClip (string assetPath)
			{
				return LoadAsset<AnimationClip> (assetPath);
			}

			public RuntimeAnimatorController LoadAnimController (string assetPath)
			{
				return LoadAsset<RuntimeAnimatorController> (assetPath);
			}

			public void Release ()
			{

			}

			public void Tick ()
			{
				this.m_BundleManager.Update ();
			}

			public void Print ()
			{
				Debug.Log ("AssetManager");
			}


			public void LoadAssetSync<T> (string assetPath, UnityAction<T> callback) where T : UnityEngine.Object
			{
				Debug.Log ("[LoadAssetSync<" + typeof(T) + ">]:" + assetPath);
				AppFacade.Instance.StartCoroutine (LoadAssetSyncBase (assetPath, typeof(T), request => {
					if (callback != null)
						callback (request.GetAsset<T> ());
				}));
			}

			private IEnumerator LoadAssetSyncBase (string assetPath, System.Type type, UnityAction<AssetLoadOperationBase> callback)
			{
				string assetbundlename = GetAssetBundleName (assetPath);
				Debug.Log ("assetbundlename:" + assetbundlename);
				string assetname = Path.GetFileNameWithoutExtension (assetPath);
				AssetLoadOperationBase request = this.m_BundleManager.LoadAssetAsync (assetbundlename, assetname, type);
				if (request == null)
					yield break;
				yield return request;
				if (callback != null) {
					callback (request);
				}
			}

			private string GetAssetBundleName (string assetPath)
			{
				string dirName = Path.GetDirectoryName (assetPath);
				string fileName = Path.GetFileNameWithoutExtension (assetPath);
				return (dirName + "/" + fileName).Replace ('/', '_').Replace ('\\', '_').ToLower ();
			}
		}
	}
}