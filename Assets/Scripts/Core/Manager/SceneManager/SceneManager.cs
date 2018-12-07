using Framework.Game;
using UnityEngine;

namespace Framework
{
    namespace Core.Manager
    {
        public partial class ManagerName
        {
            public const string Scene = "SceneManager";
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
        }
    }
}