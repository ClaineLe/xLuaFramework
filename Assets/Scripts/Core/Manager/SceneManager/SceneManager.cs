using Framework.Game;
using UnityEngine;
using UnityEngine.Events;

namespace Framework
{
    namespace Core.Manager
    {
        public partial class ManagerName
        {
            public const string Scene = "SceneManager";
        }

        public interface IScene
        {
            string Name { get; }
            void OnStartEnter();
            void OnEndEnter();
            void OnStartExit();
            void OnEndExit();
        }


        public class SceneManager : BaseManager<SceneManager>, IManager
        {
            private const string FORMAT_SCENE = PathConst.SCENE_ROOTPATH + "{0}.unity";
            public void Init()
            {
            }

            public void Release()
            {
            }

            public void Tick()
            {

            }

            public void LoadScene(string sceneName, bool isAdditive)
            {
                Framework.Game.Manager.AssetMgr.LoadScene(string.Format(FORMAT_SCENE,sceneName), isAdditive);
            }

            public void LoadSceneAsync(string sceneName, bool isAdditive, System.Action callback)
            {
                Framework.Game.Manager.AssetMgr.LoadSceneAsync(string.Format(FORMAT_SCENE, sceneName), isAdditive, callback);
            }

            public void UnLoadSceneAsync(string sceneName, UnityAction callback)
            {
                Framework.Game.Manager.AssetMgr.UnLoadSceneAsync(string.Format(FORMAT_SCENE, sceneName), callback);
            }
        }
    }
}