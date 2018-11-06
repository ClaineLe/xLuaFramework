using Framework.Core.Singleton;
using System.Collections;
using UnityEngine;

namespace Framework
{
    namespace Game
    {
		public class UpdaterModel : SingletonMono<UpdaterModel>
        {
            private UpdaterPresender _presender;
            private System.Action _onUpdateAssetFinish;
         
            protected override void onInit()
            {
                this._presender = UpdaterPresender.Create();
            }

            public void Lanucher(System.Action onUpdateAssetFinish)
            {
                this._onUpdateAssetFinish = onUpdateAssetFinish;
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