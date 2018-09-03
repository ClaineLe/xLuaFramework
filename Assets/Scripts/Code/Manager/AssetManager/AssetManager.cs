using System.Collections;
using UnityEngine;
using UnityEngine.Events;

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
            public void Init()
            {
            }

            private T LoadAsset<T>(string assetPath) where T : Object
            {
#if UNITY_EDITOR
                T asset = UnityEditor.AssetDatabase.LoadAssetAtPath <T> (assetPath);
#else
#endif
                return asset;
            }

            public GameObject LoadGameObject(string assetPath) {
                return LoadAsset<GameObject>(assetPath);
            }

            public Sprite LoadSprite(string assetPath) {
                return LoadAsset<Sprite>(assetPath);
            }

            public Texture LoadTexture(string assetPath) {
                return LoadAsset<Texture>(assetPath);
            }

            public TextAsset LoadTextAsset(string assetPath) {
                return LoadAsset<TextAsset>(assetPath);
            }

            public AudioClip LoadAudioClip(string assetPath) {
                return LoadAsset<AudioClip>(assetPath);
            }

            public Shader LoadShader(string assetPath) {
                return LoadAsset<Shader>(assetPath);
            }

            public AnimationClip LoadAnimationClip(string assetPath) {
                return LoadAsset<AnimationClip>(assetPath);
            }

            public RuntimeAnimatorController LoadAnimController(string assetPath) {
                return LoadAsset<RuntimeAnimatorController>(assetPath);
            }

            public void Release()
            {

            }

            public void Tick()
            {
            }
            public void Print()
            {
                Debug.Log("AssetManager");
            }
        }
    }
}