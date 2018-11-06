using Framework.Core.Singleton;
using System.Collections;
using UnityEngine;

namespace Framework
{
    namespace Game
    {
		public class AssetUpdater: SingletonMono<AssetUpdater>
        {
            private System.Action _onUpdateAssetFinish;
            public void Run(System.Action onUpdateAssetFinish) {
                this._onUpdateAssetFinish = onUpdateAssetFinish;
                StartCoroutine(this.AAA());
            }

            public IEnumerator AAA() {
                yield return new WaitForSeconds(3.0f);
                this.Finish();
            }

            private void Finish() {
                if (_onUpdateAssetFinish != null) {
                    this._onUpdateAssetFinish();
                }
                base.Release();
            }

            private void InitPanel()
            {

            }

            protected override void onInit()
            {
            }

            protected override void onRelease()
            {
            }
        }
    }
}