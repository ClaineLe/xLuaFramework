using Framework.Core.Manager;
using System;
using UnityEngine;

namespace Framework
{
    namespace Game
    {
        public class StartUp : MonoBehaviour
        {
            public void Start()
            {
				if (AppConst.ActiveAssetUpdater) {
					Instantiate (Resources.Load<GameObject> ("AssetUpdater"));
				} else {
					Lanucher ();
				}
            }
        	
			private void Lanucher(){
				Instantiate (Resources.Load<GameObject> ("Lanucher"));
			}

			private void InitApp(){
				
			}
		}
    }
}

