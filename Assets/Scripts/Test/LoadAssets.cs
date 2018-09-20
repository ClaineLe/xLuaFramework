using UnityEngine;
using System.Collections;
using Framework.Game;


public class Loader{
	static public void Create<T>(string assetPath, UnityEngine.Events.UnityAction<T> callback) where T : Object{
		Manager.AssetMgr.LoadAssetSync<T> (assetPath, callback);
	}
}

public class LoadAssets : MonoBehaviour
{
	private string assetPath = "Assets/MyCube.prefab";
    IEnumerator Start()
    {
		yield break;
		//yield return AssetManager.Instance.Initialize();
		Loader.Create<GameObject> (assetPath,asset=>GameObject.Instantiate(asset));
    }
}